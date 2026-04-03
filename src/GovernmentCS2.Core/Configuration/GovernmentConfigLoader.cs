using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using GovernmentCS2.Core.Contracts;

namespace GovernmentCS2.Core.Configuration
{
    public sealed class GovernmentConfigException : Exception
    {
        public GovernmentConfigException(string message)
            : base(message)
        {
        }
    }

    public sealed class GovernmentConfigLoader
    {
        private const string CoreFileName = "core.json";
        private const string DemocracyFileName = "democracy.json";

        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        public GovernmentConfigurationSet LoadFromBaseDirectory(string baseDirectory)
        {
            if (string.IsNullOrWhiteSpace(baseDirectory))
            {
                throw new GovernmentConfigException("Base directory for government configuration cannot be empty.");
            }

            var configDirectory = Path.Combine(baseDirectory, "config", "government");
            var corePath = Path.Combine(configDirectory, CoreFileName);
            var democracyPath = Path.Combine(configDirectory, DemocracyFileName);

            if (!File.Exists(corePath))
            {
                throw new GovernmentConfigException($"Government core configuration file was not found at '{corePath}'.");
            }

            if (!File.Exists(democracyPath))
            {
                throw new GovernmentConfigException($"Democracy configuration file was not found at '{democracyPath}'.");
            }

            return LoadFromJson(File.ReadAllText(corePath), File.ReadAllText(democracyPath), configDirectory);
        }

        public GovernmentConfigurationSet LoadFromJson(string coreJson, string democracyJson, string sourceDirectory)
        {
            var core = Deserialize<GovernmentCoreConfig>(coreJson, CoreFileName);
            var democracy = Deserialize<DemocracyConfig>(democracyJson, DemocracyFileName);

            var errors = Validate(core, democracy);
            if (errors.Count > 0)
            {
                throw new GovernmentConfigException("Government configuration validation failed:" + Environment.NewLine + string.Join(Environment.NewLine, errors.Select(error => "- " + error)));
            }

            return new GovernmentConfigurationSet(core, democracy, sourceDirectory);
        }

        private static T Deserialize<T>(string json, string logicalName)
        {
            try
            {
                var value = JsonSerializer.Deserialize<T>(json, SerializerOptions);
                if (value == null)
                {
                    throw new GovernmentConfigException($"Configuration file '{logicalName}' was empty or could not be deserialized.");
                }

                return value;
            }
            catch (JsonException exception)
            {
                throw new GovernmentConfigException($"Configuration file '{logicalName}' is not valid JSON: {exception.Message}");
            }
        }

        private static List<string> Validate(GovernmentCoreConfig core, DemocracyConfig democracy)
        {
            var errors = new List<string>();

            if (core.SchemaVersion <= 0)
            {
                errors.Add("core.json must declare a positive schemaVersion.");
            }

            if (!string.Equals(core.DefaultRulesetId, GovernmentRulesetIds.Democracy, StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("core.json defaultRulesetId must be 'democracy' for Phase 1.");
            }

            if (core.SupportUpdateIntervalDays <= 0)
            {
                errors.Add("core.json supportUpdateIntervalDays must be greater than 0.");
            }

            if (core.DefaultDemandModifierCap <= 0f)
            {
                errors.Add("core.json defaultDemandModifierCap must be greater than 0.");
            }

            if (democracy.SchemaVersion != core.SchemaVersion)
            {
                errors.Add("democracy.json schemaVersion must match core.json schemaVersion.");
            }

            if (democracy.TermLengthYears <= 0)
            {
                errors.Add("democracy.json termLengthYears must be greater than 0.");
            }

            if ((int)democracy.StartingUnlockLayer < 1 || (int)democracy.StartingUnlockLayer > 3)
            {
                errors.Add("democracy.json startingUnlockLayer must be between 1 and 3.");
            }

            ValidateRange(errors, democracy.StartingApproval, 0f, 100f, "democracy.json startingApproval");
            ValidateRange(errors, democracy.StartingLegitimacy, 0f, 100f, "democracy.json startingLegitimacy");
            ValidateRange(errors, democracy.StartingPoliticalCapital, 0f, 100f, "democracy.json startingPoliticalCapital");
            ValidateRange(errors, democracy.StartingCorruptionPressure, 0f, 100f, "democracy.json startingCorruptionPressure");

            ValidatePositive(errors, democracy.PoliticalCapital.BaseRegenerationPerTick, "democracy.json politicalCapital.baseRegenerationPerTick");
            ValidatePositive(errors, democracy.PoliticalCapital.DefaultMajorActionCost, "democracy.json politicalCapital.defaultMajorActionCost");
            ValidatePositive(errors, democracy.PoliticalCapital.MajorActionWarningThreshold, "democracy.json politicalCapital.majorActionWarningThreshold");
            ValidatePositive(errors, democracy.Demand.GlobalCap, "democracy.json demand.globalCap");
            ValidatePositive(errors, democracy.OverridePenalties.RecoveryMultiplier, "democracy.json overridePenalties.recoveryMultiplier");

            ValidateNonNegative(errors, democracy.Election.ApprovalWeight, "democracy.json election.approvalWeight");
            ValidateNonNegative(errors, democracy.Election.BlocWeight, "democracy.json election.blocWeight");
            ValidateNonNegative(errors, democracy.Election.PartyWeight, "democracy.json election.partyWeight");
            ValidateNonNegative(errors, democracy.Election.LegitimacyWeight, "democracy.json election.legitimacyWeight");

            if (democracy.Election.ApprovalWeight + democracy.Election.BlocWeight + democracy.Election.PartyWeight + democracy.Election.LegitimacyWeight <= 0f)
            {
                errors.Add("democracy.json election weights must sum to more than 0.");
            }

            ValidateUnlocks(errors, democracy.Unlocks);
            ValidateBlocs(errors, democracy.Blocs);
            ValidateParties(errors, democracy.Parties, democracy.Blocs.Select(bloc => bloc.Id).ToArray());

            return errors;
        }

        private static void ValidateUnlocks(List<string> errors, IList<DemocracyUnlockConfig> unlocks)
        {
            var expectedLayers = new[]
            {
                GovernmentUnlockLayer.Layer1,
                GovernmentUnlockLayer.Layer2,
                GovernmentUnlockLayer.Layer3
            };

            if (unlocks == null || unlocks.Count != expectedLayers.Length)
            {
                errors.Add("democracy.json unlocks must define exactly 3 layers.");
                return;
            }

            foreach (var expectedLayer in expectedLayers)
            {
                if (!unlocks.Any(unlock => unlock.Layer == expectedLayer))
                {
                    errors.Add($"democracy.json unlocks is missing layer '{expectedLayer}'.");
                }
            }
        }

        private static void ValidateBlocs(List<string> errors, IList<BlocConfig> blocs)
        {
            var expectedBlocIds = new[]
            {
                GovernmentBlocIds.Households,
                GovernmentBlocIds.Workers,
                GovernmentBlocIds.Business,
                GovernmentBlocIds.Students,
                GovernmentBlocIds.Seniors,
                GovernmentBlocIds.OrderEnvironment
            };

            if (blocs == null || blocs.Count != expectedBlocIds.Length)
            {
                errors.Add("democracy.json blocs must define exactly 6 blocs.");
                return;
            }

            foreach (var expectedBlocId in expectedBlocIds)
            {
                if (!blocs.Any(bloc => string.Equals(bloc.Id, expectedBlocId, StringComparison.OrdinalIgnoreCase)))
                {
                    errors.Add($"democracy.json blocs is missing required bloc '{expectedBlocId}'.");
                }
            }

            foreach (var bloc in blocs)
            {
                if (string.IsNullOrWhiteSpace(bloc.DisplayName))
                {
                    errors.Add($"democracy.json bloc '{bloc.Id}' must have a displayName.");
                }

                ValidatePositive(errors, bloc.BaseWeight, $"democracy.json bloc '{bloc.Id}' baseWeight");
            }
        }

        private static void ValidateParties(List<string> errors, IList<PartyConfig> parties, IList<string> validBlocIds)
        {
            var expectedPartyIds = new[]
            {
                GovernmentPartyIds.Growth,
                GovernmentPartyIds.Civic,
                GovernmentPartyIds.Order
            };

            if (parties == null || parties.Count != expectedPartyIds.Length)
            {
                errors.Add("democracy.json parties must define exactly 3 parties.");
                return;
            }

            foreach (var expectedPartyId in expectedPartyIds)
            {
                if (!parties.Any(party => string.Equals(party.Id, expectedPartyId, StringComparison.OrdinalIgnoreCase)))
                {
                    errors.Add($"democracy.json parties is missing required party '{expectedPartyId}'.");
                }
            }

            foreach (var party in parties)
            {
                if (string.IsNullOrWhiteSpace(party.DisplayName))
                {
                    errors.Add($"democracy.json party '{party.Id}' must have a displayName.");
                }

                ValidatePositive(errors, party.BaseWeight, $"democracy.json party '{party.Id}' baseWeight");

                foreach (var blocId in party.AffinityBlocIds)
                {
                    if (!validBlocIds.Contains(blocId, StringComparer.OrdinalIgnoreCase))
                    {
                        errors.Add($"democracy.json party '{party.Id}' references unknown bloc '{blocId}'.");
                    }
                }
            }
        }

        private static void ValidateRange(List<string> errors, float value, float minimum, float maximum, string fieldName)
        {
            if (value < minimum || value > maximum)
            {
                errors.Add($"{fieldName} must be between {minimum} and {maximum}.");
            }
        }

        private static void ValidatePositive(List<string> errors, float value, string fieldName)
        {
            if (value <= 0f)
            {
                errors.Add($"{fieldName} must be greater than 0.");
            }
        }

        private static void ValidateNonNegative(List<string> errors, float value, string fieldName)
        {
            if (value < 0f)
            {
                errors.Add($"{fieldName} must be greater than or equal to 0.");
            }
        }
    }
}
