using System.Collections.Generic;
using GovernmentCS2.Core.Configuration;
using GovernmentCS2.Core.Contracts;

namespace GovernmentCS2.Core.Tests
{
    internal static class TestConfigFactory
    {
        public static GovernmentConfigurationSet CreateConfigurationSet()
        {
            return new GovernmentConfigurationSet(CreateCoreConfig(), CreateDemocracyConfig(), "test-config");
        }

        public static GovernmentCoreConfig CreateCoreConfig()
        {
            return new GovernmentCoreConfig
            {
                SchemaVersion = 1,
                DefaultRulesetId = GovernmentRulesetIds.Democracy,
                SupportUpdateIntervalDays = 30,
                DefaultDemandModifierCap = 15f,
                Logging = new GovernmentLoggingConfig
                {
                    EmitConfigurationSummary = true
                }
            };
        }

        public static DemocracyConfig CreateDemocracyConfig()
        {
            return new DemocracyConfig
            {
                SchemaVersion = 1,
                TermLengthYears = 4,
                StartingUnlockLayer = GovernmentUnlockLayer.Layer1,
                StartingApproval = 50f,
                StartingLegitimacy = 60f,
                StartingPoliticalCapital = 50f,
                StartingCorruptionPressure = 5f,
                Election = new DemocracyElectionConfig
                {
                    ApprovalWeight = 0.35f,
                    BlocWeight = 0.25f,
                    PartyWeight = 0.25f,
                    LegitimacyWeight = 0.15f
                },
                PoliticalCapital = new DemocracyPoliticalCapitalConfig
                {
                    BaseRegenerationPerTick = 4f,
                    LowApprovalPenaltyMultiplier = 0.75f,
                    LowLegitimacyPenaltyMultiplier = 0.75f,
                    DefaultMajorActionCost = 15f,
                    MajorActionWarningThreshold = 15f
                },
                Demand = new DemocracyDemandConfig
                {
                    GlobalCap = 15f,
                    ConfidenceWeight = 0.5f,
                    PolicyDirectionWeight = 0.5f,
                    ResidentialMultiplier = 1f,
                    CommercialMultiplier = 1f,
                    IndustrialMultiplier = 1f,
                    OfficeMultiplier = 1f
                },
                OverridePenalties = new DemocracyOverridePenaltyConfig
                {
                    LegitimacyPenalty = 20f,
                    ConfidencePenalty = 10f,
                    RecoveryMultiplier = 0.75f
                },
                Unlocks = new List<DemocracyUnlockConfig>
                {
                    new DemocracyUnlockConfig
                    {
                        Layer = GovernmentUnlockLayer.Layer1,
                        CityMilestoneThreshold = 0,
                        RequiredCompletedTerms = 0
                    },
                    new DemocracyUnlockConfig
                    {
                        Layer = GovernmentUnlockLayer.Layer2,
                        CityMilestoneThreshold = 5,
                        RequiredCompletedTerms = 1
                    },
                    new DemocracyUnlockConfig
                    {
                        Layer = GovernmentUnlockLayer.Layer3,
                        CityMilestoneThreshold = 10,
                        RequiredCompletedTerms = 2
                    }
                },
                Blocs = new List<BlocConfig>
                {
                    CreateBloc(GovernmentBlocIds.Households, "Households", true, "residential", "commercial"),
                    CreateBloc(GovernmentBlocIds.Workers, "Workers", true, "industrial", "commercial", "residential"),
                    CreateBloc(GovernmentBlocIds.Business, "Business", true, "office", "commercial", "industrial"),
                    CreateBloc(GovernmentBlocIds.Students, "Students", true, "residential", "office"),
                    CreateBloc(GovernmentBlocIds.Seniors, "Seniors", true, "residential"),
                    CreateBloc(GovernmentBlocIds.OrderEnvironment, "Order/Environment", true, "residential", "commercial")
                },
                Parties = new List<PartyConfig>
                {
                    CreateParty(GovernmentPartyIds.Growth, "Growth", GovernmentBlocIds.Business, GovernmentBlocIds.Workers),
                    CreateParty(GovernmentPartyIds.Civic, "Civic", GovernmentBlocIds.Households, GovernmentBlocIds.Students, GovernmentBlocIds.Seniors),
                    CreateParty(GovernmentPartyIds.Order, "Order", GovernmentBlocIds.OrderEnvironment, GovernmentBlocIds.Seniors, GovernmentBlocIds.Households)
                }
            };
        }

        private static BlocConfig CreateBloc(string id, string displayName, bool districtSensitive, params string[] demandChannels)
        {
            return new BlocConfig
            {
                Id = id,
                DisplayName = displayName,
                BaseWeight = 1f,
                DistrictSensitive = districtSensitive,
                PrimaryDemandChannels = new List<string>(demandChannels)
            };
        }

        private static PartyConfig CreateParty(string id, string displayName, params string[] affinityBlocIds)
        {
            return new PartyConfig
            {
                Id = id,
                DisplayName = displayName,
                BaseWeight = 1f,
                AffinityBlocIds = new List<string>(affinityBlocIds)
            };
        }
    }
}
