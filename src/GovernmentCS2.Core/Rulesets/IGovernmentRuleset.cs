using GovernmentCS2.Core.Configuration;
using GovernmentCS2.Core.Contracts;

namespace GovernmentCS2.Core.Rulesets
{
    public interface IGovernmentRuleset
    {
        string RulesetId { get; }

        GovernmentModelState InitializeNewCity(GovernmentConfigurationSet configurationSet, GovernmentInitializationContext initializationContext);

        GovernmentModelState SeedExistingCity(GovernmentConfigurationSet configurationSet, GovernmentInitializationContext initializationContext);

        GovernmentRuntimeState TickUpdate(GovernmentModelState currentState, GovernmentConfigurationSet configurationSet);

        GovernmentDemandEffects EvaluateDemandEffects(GovernmentModelState currentState, GovernmentConfigurationSet configurationSet);

        GovernmentActionCostResult EvaluateActionCost(string actionId, GovernmentModelState currentState, GovernmentConfigurationSet configurationSet);

        GovernmentPanelViewModel BuildPanelViewModel(GovernmentModelState currentState, GovernmentConfigurationSet configurationSet);
    }
}
