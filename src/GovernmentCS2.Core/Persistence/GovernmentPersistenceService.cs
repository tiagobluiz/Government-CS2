using System;
using System.Text.Json;
using GovernmentCS2.Core.Configuration;
using GovernmentCS2.Core.Contracts;
using GovernmentCS2.Core.Rulesets;

namespace GovernmentCS2.Core.Persistence
{
    public sealed class GovernmentPersistenceService
    {
        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        private readonly GovernmentSaveMigrationRunner myMigrationRunner;

        public GovernmentPersistenceService(GovernmentSaveMigrationRunner migrationRunner)
        {
            myMigrationRunner = migrationRunner ?? throw new ArgumentNullException(nameof(migrationRunner));
        }

        public string Serialize(GovernmentModelState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            return JsonSerializer.Serialize(Capture(state), SerializerOptions);
        }

        public GovernmentSaveDataV1 Capture(GovernmentModelState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            return new GovernmentSaveDataV1
            {
                SchemaVersion = state.SchemaVersion,
                RulesetId = state.ActiveRulesetId,
                UnlockLayer = state.CurrentUnlockLayer,
                Approval = state.Approval,
                Legitimacy = state.Legitimacy,
                PoliticalCapital = state.PoliticalCapital,
                CorruptionPressure = state.CorruptionPressure,
                ElectionCycleState = state.ElectionCycle,
                OverrideState = state.OverrideState,
                BlocScores = state.BlocSnapshots,
                PartyStandings = state.PartySnapshots,
                DistrictSeedAggregates = state.DistrictPoliticalSeeds
            };
        }

        public GovernmentModelState Restore(
            string serializedState,
            GovernmentConfigurationSet configurationSet,
            IGovernmentRuleset ruleset,
            GovernmentInitializationContext initializationContext)
        {
            if (string.IsNullOrWhiteSpace(serializedState))
            {
                throw new ArgumentException("Serialized government state cannot be empty.", nameof(serializedState));
            }

            if (configurationSet == null)
            {
                throw new ArgumentNullException(nameof(configurationSet));
            }

            if (ruleset == null)
            {
                throw new ArgumentNullException(nameof(ruleset));
            }

            var migratedJson = myMigrationRunner.MigrateToVersion(serializedState, configurationSet.Core.SchemaVersion);
            var saveData = JsonSerializer.Deserialize<GovernmentSaveDataV1>(migratedJson, SerializerOptions);
            if (saveData == null)
            {
                throw new InvalidOperationException("Government save payload could not be deserialized.");
            }

            var restoredState = ruleset.SeedExistingCity(configurationSet, initializationContext);
            restoredState.SchemaVersion = saveData.SchemaVersion;
            restoredState.ActiveRulesetId = saveData.RulesetId;
            restoredState.CurrentUnlockLayer = saveData.UnlockLayer;
            restoredState.Approval = saveData.Approval;
            restoredState.Legitimacy = saveData.Legitimacy;
            restoredState.PoliticalCapital = saveData.PoliticalCapital;
            restoredState.CorruptionPressure = saveData.CorruptionPressure;
            restoredState.ElectionCycle = saveData.ElectionCycleState;
            restoredState.OverrideState = saveData.OverrideState;
            restoredState.BlocSnapshots = saveData.BlocScores;
            restoredState.PartySnapshots = saveData.PartyStandings;
            restoredState.DistrictPoliticalSeeds = saveData.DistrictSeedAggregates;
            return restoredState;
        }
    }
}
