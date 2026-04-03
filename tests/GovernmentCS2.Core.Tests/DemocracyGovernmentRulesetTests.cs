using GovernmentCS2.Core.Contracts;
using GovernmentCS2.Core.Persistence;
using GovernmentCS2.Core.Rulesets;
using GovernmentCS2.Core.Runtime;
using Xunit;

namespace GovernmentCS2.Core.Tests
{
    public class DemocracyGovernmentRulesetTests
    {
        [Fact]
        public void InitializeNewCity_UsesConfiguredDefaults()
        {
            var configurationSet = TestConfigFactory.CreateConfigurationSet();
            var ruleset = new DemocracyGovernmentRuleset();
            var initializationContext = new GovernmentInitializationContext
            {
                CityName = "New City",
                CurrentGameTime = 12L
            };

            var state = ruleset.InitializeNewCity(configurationSet, initializationContext);

            Assert.Equal(GovernmentRulesetIds.Democracy, state.ActiveRulesetId);
            Assert.Equal(50f, state.Approval);
            Assert.Equal(60f, state.Legitimacy);
            Assert.Equal(GovernmentUnlockLayer.Layer1, state.CurrentUnlockLayer);
            Assert.Equal(4, state.ElectionCycle.DefaultTermLengthYears);
            Assert.Equal(12L, state.ElectionCycle.CurrentTermStartGameTime);
            Assert.Equal(6, state.BlocSnapshots.Count);
            Assert.Equal(3, state.PartySnapshots.Count);
        }

        [Fact]
        public void Initialize_RegistersDemocracyAndBuildsPhaseZeroShells()
        {
            var configurationSet = TestConfigFactory.CreateConfigurationSet();
            var module = GovernmentModule.CreateDefault(configurationSet);
            var bootstrap = module.Initialize(
                GovernmentInitializationMode.NewCity,
                new GovernmentInitializationContext
                {
                    CityName = "Bootstrap"
                });

            Assert.Contains(GovernmentRulesetIds.Democracy, module.RegisteredRulesetIds);
            Assert.Contains("phase-0-foundation", bootstrap.RuntimeState.CurrentGovernmentStatusFlags);
            Assert.Contains("phase-0-runtime-pipeline", bootstrap.RuntimeState.CurrentGovernmentStatusFlags);
            Assert.Contains("phase-0-demand-effects-placeholder", bootstrap.RuntimeState.CurrentDemandEffects.ReasonCodes);
            Assert.Contains("phase-0-modifier-pipeline", bootstrap.RuntimeState.CurrentDemandEffects.ReasonCodes);
            Assert.Contains("phase-0-panel-shell", bootstrap.PanelViewModel.ActionWarnings);
            Assert.Equal("NewCity", bootstrap.DebugSnapshot.InitializationMode);
        }

        [Fact]
        public void Initialize_ExistingCityUsesSeedingPath()
        {
            var configurationSet = TestConfigFactory.CreateConfigurationSet();
            var module = GovernmentModule.CreateDefault(configurationSet);

            var bootstrap = module.Initialize(
                GovernmentInitializationMode.ExistingCity,
                new GovernmentInitializationContext
                {
                    CityName = "Existing City",
                    CurrentMilestoneLevel = 6,
                    CurrentGameTime = 42L
                });

            Assert.Equal(GovernmentUnlockLayer.Layer2, bootstrap.State.CurrentUnlockLayer);
            Assert.False(bootstrap.LoadedFromSave);
            Assert.Equal("ExistingCity", bootstrap.DebugSnapshot.InitializationMode);
        }

        [Fact]
        public void SerializeAndRestore_RoundTripsGovernmentState()
        {
            var configurationSet = TestConfigFactory.CreateConfigurationSet();
            var module = GovernmentModule.CreateDefault(configurationSet);
            var initial = module.Initialize(GovernmentInitializationMode.NewCity);
            initial.State.Approval = 73f;
            initial.State.Legitimacy = 61f;

            var serialized = module.SerializeState(initial.State);

            var restored = module.Initialize(
                GovernmentInitializationMode.ExistingCity,
                new GovernmentInitializationContext
                {
                    CityName = "Restore City",
                    CurrentMilestoneLevel = 5
                },
                serialized);

            Assert.True(restored.LoadedFromSave);
            Assert.Equal(73f, restored.State.Approval);
            Assert.Equal(61f, restored.State.Legitimacy);
            Assert.Equal(initial.State.CurrentUnlockLayer, restored.State.CurrentUnlockLayer);
        }

        [Fact]
        public void MigrationRunner_ThrowsWhenOlderSchemaHasNoRegisteredStep()
        {
            var runner = new GovernmentSaveMigrationRunner(System.Array.Empty<IGovernmentSaveMigrationStep>());
            const string legacyJson = "{ \"SchemaVersion\": 0 }";

            var exception = Assert.Throws<GovernmentSaveMigrationException>(() => runner.MigrateToVersion(legacyJson, 1));

            Assert.Contains("No migration step", exception.Message);
        }
    }
}
