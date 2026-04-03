using System;
using System.Collections.Generic;
using System.Linq;
using GovernmentCS2.Core.Configuration;
using GovernmentCS2.Core.Contracts;
using GovernmentCS2.Core.Persistence;
using GovernmentCS2.Core.Presentation;
using GovernmentCS2.Core.Rulesets;

namespace GovernmentCS2.Core.Runtime
{
    public sealed class GovernmentModule : IDisposable
    {
        private readonly IDictionary<string, IGovernmentRuleset> myRulesets;
        private readonly GovernmentDebugInspector myDebugInspector;
        private readonly GovernmentModifierPipeline myModifierPipeline;
        private readonly GovernmentPanelShell myPanelShell;
        private readonly GovernmentPersistenceService myPersistenceService;

        public GovernmentModule(GovernmentConfigurationSet configurationSet, IEnumerable<IGovernmentRuleset> rulesets)
        {
            ConfigurationSet = configurationSet ?? throw new ArgumentNullException(nameof(configurationSet));
            myRulesets = rulesets?.ToDictionary(ruleset => ruleset.RulesetId, StringComparer.OrdinalIgnoreCase)
                ?? throw new ArgumentNullException(nameof(rulesets));

            if (myRulesets.Count == 0)
            {
                throw new ArgumentException("At least one government ruleset must be registered.", nameof(rulesets));
            }

            if (!myRulesets.ContainsKey(configurationSet.Core.DefaultRulesetId))
            {
                throw new ArgumentException($"The configured default ruleset '{configurationSet.Core.DefaultRulesetId}' was not registered.", nameof(rulesets));
            }

            myPersistenceService = new GovernmentPersistenceService(new GovernmentSaveMigrationRunner(Array.Empty<IGovernmentSaveMigrationStep>()));
            myModifierPipeline = new GovernmentModifierPipeline();
            myPanelShell = new GovernmentPanelShell();
            myDebugInspector = new GovernmentDebugInspector();
        }

        public GovernmentConfigurationSet ConfigurationSet { get; }

        public IReadOnlyCollection<string> RegisteredRulesetIds => myRulesets.Keys.ToArray();

        public static GovernmentModule CreateDefault(GovernmentConfigurationSet configurationSet)
        {
            return new GovernmentModule(configurationSet, new IGovernmentRuleset[]
            {
                new DemocracyGovernmentRuleset()
            });
        }

        public GovernmentBootstrapResult Initialize(GovernmentInitializationMode initializationMode, GovernmentInitializationContext initializationContext = null, string serializedState = null)
        {
            var context = initializationContext ?? new GovernmentInitializationContext();
            var ruleset = GetActiveRuleset();
            GovernmentModelState state;
            var loadedFromSave = false;

            if (!string.IsNullOrWhiteSpace(serializedState))
            {
                var savedRulesetId = myPersistenceService.ReadRulesetId(serializedState);
                ruleset = string.IsNullOrWhiteSpace(savedRulesetId) ? ruleset : GetRuleset(savedRulesetId);
                state = myPersistenceService.Restore(serializedState, ConfigurationSet, ruleset, context);
                loadedFromSave = true;
            }
            else if (initializationMode == GovernmentInitializationMode.ExistingCity)
            {
                state = ruleset.SeedExistingCity(ConfigurationSet, context);
            }
            else
            {
                state = ruleset.InitializeNewCity(ConfigurationSet, context);
            }

            var runtimeState = TickUpdate(state);
            var panelViewModel = myPanelShell.Build(GetRuleset(state.ActiveRulesetId), state, ConfigurationSet);
            var debugSnapshot = myDebugInspector.Capture(state, runtimeState, panelViewModel, ConfigurationSet, initializationMode);

            return new GovernmentBootstrapResult
            {
                InitializationMode = initializationMode,
                LoadedFromSave = loadedFromSave,
                State = state,
                RuntimeState = runtimeState,
                PanelViewModel = panelViewModel,
                DebugSnapshot = debugSnapshot
            };
        }

        public GovernmentModelState CreateNewCityState(GovernmentInitializationContext initializationContext = null)
        {
            return GetActiveRuleset().InitializeNewCity(ConfigurationSet, initializationContext ?? new GovernmentInitializationContext());
        }

        public GovernmentModelState SeedExistingCityState(GovernmentInitializationContext initializationContext = null)
        {
            return GetActiveRuleset().SeedExistingCity(ConfigurationSet, initializationContext ?? new GovernmentInitializationContext());
        }

        public GovernmentRuntimeState TickUpdate(GovernmentModelState currentState)
        {
            if (currentState == null)
            {
                throw new ArgumentNullException(nameof(currentState));
            }

            var rulesetRuntime = GetRuleset(currentState.ActiveRulesetId).TickUpdate(currentState, ConfigurationSet);
            rulesetRuntime.CurrentDemandEffects = myModifierPipeline.ApplyDemandEffects(rulesetRuntime.CurrentDemandEffects, ConfigurationSet);
            rulesetRuntime.CurrentGovernmentStatusFlags.Add("phase-0-runtime-pipeline");
            return rulesetRuntime;
        }

        public string SerializeState(GovernmentModelState state)
        {
            return myPersistenceService.Serialize(state);
        }

        public void Dispose()
        {
        }

        private IGovernmentRuleset GetActiveRuleset()
        {
            return GetRuleset(ConfigurationSet.Core.DefaultRulesetId);
        }

        private IGovernmentRuleset GetRuleset(string rulesetId)
        {
            if (!myRulesets.TryGetValue(rulesetId, out var ruleset))
            {
                throw new InvalidOperationException($"No government ruleset is registered for '{rulesetId}'.");
            }

            return ruleset;
        }
    }
}
