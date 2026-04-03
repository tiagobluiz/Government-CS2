using System;
using GovernmentCS2.Core.Configuration;
using GovernmentCS2.Core.Contracts;

namespace GovernmentCS2.Core.Runtime
{
    public sealed class GovernmentDebugInspector
    {
        public GovernmentDebugSnapshot Capture(
            GovernmentModelState state,
            GovernmentRuntimeState runtimeState,
            GovernmentPanelViewModel panelViewModel,
            GovernmentConfigurationSet configurationSet,
            GovernmentInitializationMode initializationMode)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (runtimeState == null)
            {
                throw new ArgumentNullException(nameof(runtimeState));
            }

            if (panelViewModel == null)
            {
                throw new ArgumentNullException(nameof(panelViewModel));
            }

            if (configurationSet == null)
            {
                throw new ArgumentNullException(nameof(configurationSet));
            }

            return new GovernmentDebugSnapshot
            {
                SchemaVersion = state.SchemaVersion,
                RulesetId = state.ActiveRulesetId,
                InitializationMode = initializationMode.ToString(),
                ConfigurationSourceDirectory = configurationSet.SourceDirectory,
                CurrentRiskLevel = runtimeState.CurrentRiskLevel,
                Approval = state.Approval,
                Legitimacy = state.Legitimacy,
                PoliticalCapital = state.PoliticalCapital,
                CorruptionPressure = state.CorruptionPressure,
                DemandEffects = runtimeState.CurrentDemandEffects,
                ActionFrictionSummary = runtimeState.CurrentActionFrictionSummary,
                PanelSummary = $"{panelViewModel.GovernmentTypeName}|{panelViewModel.CurrentRiskLevel}|{panelViewModel.UnlockLayerSummary}"
            };
        }
    }
}
