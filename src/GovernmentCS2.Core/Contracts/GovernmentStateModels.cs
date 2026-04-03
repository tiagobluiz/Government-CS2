using System.Collections.Generic;

namespace GovernmentCS2.Core.Contracts
{
    public enum GovernmentUnlockLayer
    {
        Layer1 = 1,
        Layer2 = 2,
        Layer3 = 3
    }

    public enum PoliticalTrend
    {
        Stable = 0,
        Rising = 1,
        Falling = 2
    }

    public enum WarningLevel
    {
        None = 0,
        Low = 1,
        Moderate = 2,
        High = 3
    }

    public enum DistrictSensitivityMode
    {
        None = 0,
        Light = 1,
        High = 2
    }

    public sealed class GovernmentOverrideState
    {
        public bool IsElectionOverrideActive { get; set; }
        public bool PenaltiesDisabled { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    public sealed class GovernmentModelState
    {
        public string ActiveRulesetId { get; set; } = string.Empty;
        public int SchemaVersion { get; set; }
        public GovernmentUnlockLayer CurrentUnlockLayer { get; set; }
        public float Approval { get; set; }
        public float Legitimacy { get; set; }
        public float PoliticalCapital { get; set; }
        public float CorruptionPressure { get; set; }
        public ElectionCycleState ElectionCycle { get; set; } = new ElectionCycleState();
        public GovernmentOverrideState OverrideState { get; set; } = new GovernmentOverrideState();
        public IList<PoliticalBlocSnapshot> BlocSnapshots { get; set; } = new List<PoliticalBlocSnapshot>();
        public IList<PartyStandingSnapshot> PartySnapshots { get; set; } = new List<PartyStandingSnapshot>();
        public IDictionary<string, float> DistrictPoliticalSeeds { get; set; } = new Dictionary<string, float>();
        public object RulesetRuntimeState { get; set; }
    }

    public sealed class GovernmentRuntimeState
    {
        public string CurrentRiskLevel { get; set; } = "Stable";
        public long LastSupportUpdateTimestamp { get; set; }
        public GovernmentDemandEffects CurrentDemandEffects { get; set; } = new GovernmentDemandEffects();
        public GovernmentActionCostResult CurrentActionFrictionSummary { get; set; } = new GovernmentActionCostResult();
        public IList<string> CurrentGovernmentStatusFlags { get; set; } = new List<string>();
    }

    public sealed class DemocracyRuntimeState
    {
        public float ElectionPressureScore { get; set; }
        public string RecentPolicyDirectionSummary { get; set; } = "Neutral";
        public string ConsultationState { get; set; } = "Locked";
        public string RecentBlocShiftSummary { get; set; } = "No shifts recorded yet.";
        public string RecentPartyShiftSummary { get; set; } = "No shifts recorded yet.";
        public string RecentOverridePenaltyState { get; set; } = "No override penalties active.";
    }

    public sealed class GovernmentSaveDataV1
    {
        public int SchemaVersion { get; set; }
        public string RulesetId { get; set; } = string.Empty;
        public GovernmentUnlockLayer UnlockLayer { get; set; }
        public float Approval { get; set; }
        public float Legitimacy { get; set; }
        public float PoliticalCapital { get; set; }
        public float CorruptionPressure { get; set; }
        public ElectionCycleState ElectionCycleState { get; set; } = new ElectionCycleState();
        public GovernmentOverrideState OverrideState { get; set; } = new GovernmentOverrideState();
        public IList<PoliticalBlocSnapshot> BlocScores { get; set; } = new List<PoliticalBlocSnapshot>();
        public IList<PartyStandingSnapshot> PartyStandings { get; set; } = new List<PartyStandingSnapshot>();
        public IDictionary<string, float> DistrictSeedAggregates { get; set; } = new Dictionary<string, float>();
    }

    public sealed class ElectionCycleState
    {
        public int CurrentTermIndex { get; set; }
        public long CurrentTermStartGameTime { get; set; }
        public long CurrentTermEndGameTime { get; set; }
        public int DefaultTermLengthYears { get; set; }
        public ElectionResolutionResult LastElectionResult { get; set; } = new ElectionResolutionResult();
    }

    public sealed class GovernmentDemandEffects
    {
        public float ResidentialModifier { get; set; }
        public float CommercialModifier { get; set; }
        public float IndustrialModifier { get; set; }
        public float OfficeModifier { get; set; }
        public float ConfidenceChannelContribution { get; set; }
        public float PolicyDirectionChannelContribution { get; set; }
        public IList<string> ReasonCodes { get; set; } = new List<string>();
    }

    public sealed class GovernmentActionCostResult
    {
        public float PoliticalCapitalCost { get; set; }
        public float ApprovalRisk { get; set; }
        public float LegitimacyRisk { get; set; }
        public IList<string> BlocReactionSummary { get; set; } = new List<string>();
        public IList<string> PartyReactionSummary { get; set; } = new List<string>();
        public WarningLevel WarningLevel { get; set; }
        public bool IsHardBlocked { get; set; }
    }

    public sealed class ElectionResolutionResult
    {
        public bool IncumbentWon { get; set; }
        public float ApprovalContribution { get; set; }
        public float LegitimacyContribution { get; set; }
        public IDictionary<string, float> BlocContributionSummary { get; set; } = new Dictionary<string, float>();
        public IDictionary<string, float> PartyContributionSummary { get; set; } = new Dictionary<string, float>();
        public float FinalScore { get; set; }
        public IList<string> RiskWarnings { get; set; } = new List<string>();
        public string OutcomeReasonSummary { get; set; } = string.Empty;
    }

    public sealed class PoliticalBlocSnapshot
    {
        public string BlocId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public float CurrentSupport { get; set; }
        public PoliticalTrend Trend { get; set; }
        public IList<string> TopPositiveDrivers { get; set; } = new List<string>();
        public IList<string> TopNegativeDrivers { get; set; } = new List<string>();
        public IList<string> PrimaryDemandChannels { get; set; } = new List<string>();
        public DistrictSensitivityMode DistrictSensitivityMode { get; set; }
    }

    public sealed class PartyStandingSnapshot
    {
        public string PartyId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public float CurrentStanding { get; set; }
        public PoliticalTrend Trend { get; set; }
        public IList<string> BlocAffinitySummary { get; set; } = new List<string>();
        public IList<string> IssueAlignmentSummary { get; set; } = new List<string>();
        public IList<string> TopPositiveDrivers { get; set; } = new List<string>();
        public IList<string> TopNegativeDrivers { get; set; } = new List<string>();
    }

    public sealed class GovernmentPanelViewModel
    {
        public string GovernmentTypeName { get; set; } = string.Empty;
        public float Approval { get; set; }
        public float Legitimacy { get; set; }
        public string ElectionCountdown { get; set; } = string.Empty;
        public string CurrentRiskLevel { get; set; } = string.Empty;
        public float PoliticalCapital { get; set; }
        public float CorruptionPressure { get; set; }
        public IList<PoliticalBlocSnapshot> BlocCards { get; set; } = new List<PoliticalBlocSnapshot>();
        public IList<PartyStandingSnapshot> PartyCards { get; set; } = new List<PartyStandingSnapshot>();
        public GovernmentDemandEffects DemandEffectSummary { get; set; } = new GovernmentDemandEffects();
        public string OverrideStateSummary { get; set; } = string.Empty;
        public string UnlockLayerSummary { get; set; } = string.Empty;
        public IList<string> ActionWarnings { get; set; } = new List<string>();
    }
}
