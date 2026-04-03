using System.Collections.Generic;
using GovernmentCS2.Core.Contracts;

namespace GovernmentCS2.Core.Configuration
{
    public sealed class GovernmentCoreConfig
    {
        public int SchemaVersion { get; set; }
        public string DefaultRulesetId { get; set; } = GovernmentRulesetIds.Democracy;
        public int SupportUpdateIntervalDays { get; set; }
        public float DefaultDemandModifierCap { get; set; }
        public GovernmentLoggingConfig Logging { get; set; } = new GovernmentLoggingConfig();
    }

    public sealed class GovernmentLoggingConfig
    {
        public bool EmitConfigurationSummary { get; set; }
    }

    public sealed class DemocracyConfig
    {
        public int SchemaVersion { get; set; }
        public int TermLengthYears { get; set; }
        public GovernmentUnlockLayer StartingUnlockLayer { get; set; }
        public float StartingApproval { get; set; }
        public float StartingLegitimacy { get; set; }
        public float StartingPoliticalCapital { get; set; }
        public float StartingCorruptionPressure { get; set; }
        public DemocracyElectionConfig Election { get; set; } = new DemocracyElectionConfig();
        public DemocracyPoliticalCapitalConfig PoliticalCapital { get; set; } = new DemocracyPoliticalCapitalConfig();
        public DemocracyDemandConfig Demand { get; set; } = new DemocracyDemandConfig();
        public DemocracyOverridePenaltyConfig OverridePenalties { get; set; } = new DemocracyOverridePenaltyConfig();
        public IList<DemocracyUnlockConfig> Unlocks { get; set; } = new List<DemocracyUnlockConfig>();
        public IList<BlocConfig> Blocs { get; set; } = new List<BlocConfig>();
        public IList<PartyConfig> Parties { get; set; } = new List<PartyConfig>();
    }

    public sealed class DemocracyElectionConfig
    {
        public float ApprovalWeight { get; set; }
        public float BlocWeight { get; set; }
        public float PartyWeight { get; set; }
        public float LegitimacyWeight { get; set; }
    }

    public sealed class DemocracyPoliticalCapitalConfig
    {
        public float BaseRegenerationPerTick { get; set; }
        public float LowApprovalPenaltyMultiplier { get; set; }
        public float LowLegitimacyPenaltyMultiplier { get; set; }
        public float DefaultMajorActionCost { get; set; }
        public float MajorActionWarningThreshold { get; set; }
    }

    public sealed class DemocracyDemandConfig
    {
        public float GlobalCap { get; set; }
        public float ConfidenceWeight { get; set; }
        public float PolicyDirectionWeight { get; set; }
        public float ResidentialMultiplier { get; set; }
        public float CommercialMultiplier { get; set; }
        public float IndustrialMultiplier { get; set; }
        public float OfficeMultiplier { get; set; }
    }

    public sealed class DemocracyOverridePenaltyConfig
    {
        public float LegitimacyPenalty { get; set; }
        public float ConfidencePenalty { get; set; }
        public float RecoveryMultiplier { get; set; }
    }

    public sealed class DemocracyUnlockConfig
    {
        public GovernmentUnlockLayer Layer { get; set; }
        public int CityMilestoneThreshold { get; set; }
        public int RequiredCompletedTerms { get; set; }
    }

    public sealed class BlocConfig
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public float BaseWeight { get; set; }
        public bool DistrictSensitive { get; set; }
        public IList<string> PrimaryDemandChannels { get; set; } = new List<string>();
    }

    public sealed class PartyConfig
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public float BaseWeight { get; set; }
        public IList<string> AffinityBlocIds { get; set; } = new List<string>();
    }

    public sealed class GovernmentConfigurationSet
    {
        public GovernmentConfigurationSet(GovernmentCoreConfig core, DemocracyConfig democracy, string sourceDirectory)
        {
            Core = core;
            Democracy = democracy;
            SourceDirectory = sourceDirectory;
        }

        public GovernmentCoreConfig Core { get; }

        public DemocracyConfig Democracy { get; }

        public string SourceDirectory { get; }

        public string Describe()
        {
            return $"ruleset={Core.DefaultRulesetId}, termYears={Democracy.TermLengthYears}, blocs={Democracy.Blocs.Count}, parties={Democracy.Parties.Count}";
        }
    }
}
