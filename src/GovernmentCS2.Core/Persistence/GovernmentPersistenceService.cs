using System;
using System.Collections.Generic;
using System.Linq;
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
            restoredState.BlocSnapshots = ReconcileBlocSnapshots(restoredState.BlocSnapshots, saveData.BlocScores);
            restoredState.PartySnapshots = ReconcilePartySnapshots(restoredState.PartySnapshots, saveData.PartyStandings);
            restoredState.DistrictPoliticalSeeds = ReconcileDistrictSeeds(restoredState.DistrictPoliticalSeeds, saveData.DistrictSeedAggregates);
            return restoredState;
        }

        public string ReadRulesetId(string serializedState)
        {
            if (string.IsNullOrWhiteSpace(serializedState))
            {
                throw new ArgumentException("Serialized government state cannot be empty.", nameof(serializedState));
            }

            using (var document = JsonDocument.Parse(serializedState))
            {
                if (document.RootElement.TryGetProperty("RulesetId", out var rulesetIdProperty))
                {
                    return rulesetIdProperty.GetString() ?? string.Empty;
                }

                if (document.RootElement.TryGetProperty("rulesetId", out rulesetIdProperty))
                {
                    return rulesetIdProperty.GetString() ?? string.Empty;
                }
            }

            return string.Empty;
        }

        private static IList<PoliticalBlocSnapshot> ReconcileBlocSnapshots(
            IList<PoliticalBlocSnapshot> baseline,
            IList<PoliticalBlocSnapshot> saved)
        {
            var baselineById = (baseline ?? new List<PoliticalBlocSnapshot>())
                .ToDictionary(item => item.BlocId, StringComparer.OrdinalIgnoreCase);

            if (saved != null)
            {
                foreach (var snapshot in saved)
                {
                    if (snapshot != null && baselineById.ContainsKey(snapshot.BlocId))
                    {
                        baselineById[snapshot.BlocId] = snapshot;
                    }
                }
            }

            return baselineById.Values.ToList();
        }

        private static IList<PartyStandingSnapshot> ReconcilePartySnapshots(
            IList<PartyStandingSnapshot> baseline,
            IList<PartyStandingSnapshot> saved)
        {
            var baselineById = (baseline ?? new List<PartyStandingSnapshot>())
                .ToDictionary(item => item.PartyId, StringComparer.OrdinalIgnoreCase);

            if (saved != null)
            {
                foreach (var snapshot in saved)
                {
                    if (snapshot != null && baselineById.ContainsKey(snapshot.PartyId))
                    {
                        baselineById[snapshot.PartyId] = snapshot;
                    }
                }
            }

            return baselineById.Values.ToList();
        }

        private static IDictionary<string, float> ReconcileDistrictSeeds(
            IDictionary<string, float> baseline,
            IDictionary<string, float> saved)
        {
            var merged = new Dictionary<string, float>(baseline ?? new Dictionary<string, float>(), StringComparer.OrdinalIgnoreCase);
            if (saved == null)
            {
                return merged;
            }

            foreach (var entry in saved)
            {
                merged[entry.Key] = entry.Value;
            }

            return merged;
        }
    }
}
