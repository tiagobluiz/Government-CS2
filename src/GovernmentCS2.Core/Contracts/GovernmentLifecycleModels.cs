namespace GovernmentCS2.Core.Contracts
{
    public enum GovernmentInitializationMode
    {
        NewCity = 0,
        ExistingCity = 1
    }

    public sealed class GovernmentInitializationContext
    {
        // Reserved for future phases where initialization may reflect city identity in UI and diagnostics.
        public string CityName { get; set; } = string.Empty;

        // Reserved for future phases where seeding will use population-based starting conditions.
        public int PopulationEstimate { get; set; }
        public int CurrentMilestoneLevel { get; set; }
        public long CurrentGameTime { get; set; }
    }

    public sealed class GovernmentDebugSnapshot
    {
        public int SchemaVersion { get; set; }
        public string RulesetId { get; set; } = string.Empty;
        public string InitializationMode { get; set; } = string.Empty;
        public string ConfigurationSourceDirectory { get; set; } = string.Empty;
        public string CurrentRiskLevel { get; set; } = string.Empty;
        public float Approval { get; set; }
        public float Legitimacy { get; set; }
        public float PoliticalCapital { get; set; }
        public float CorruptionPressure { get; set; }
        public GovernmentDemandEffects DemandEffects { get; set; } = new GovernmentDemandEffects();
        public GovernmentActionCostResult ActionFrictionSummary { get; set; } = new GovernmentActionCostResult();
        public string PanelSummary { get; set; } = string.Empty;
    }

    public sealed class GovernmentBootstrapResult
    {
        public GovernmentInitializationMode InitializationMode { get; set; }
        public bool LoadedFromSave { get; set; }
        public GovernmentModelState State { get; set; } = new GovernmentModelState();
        public GovernmentRuntimeState RuntimeState { get; set; } = new GovernmentRuntimeState();
        public GovernmentPanelViewModel PanelViewModel { get; set; } = new GovernmentPanelViewModel();
        public GovernmentDebugSnapshot DebugSnapshot { get; set; } = new GovernmentDebugSnapshot();
    }
}
