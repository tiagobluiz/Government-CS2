using GovernmentCS2.Core.Configuration;
using GovernmentCS2.Core.Contracts;
using GovernmentCS2.Core.Rulesets;

namespace GovernmentCS2.Core.Presentation
{
    public sealed class GovernmentPanelShell
    {
        public GovernmentPanelViewModel Build(IGovernmentRuleset ruleset, GovernmentModelState state, GovernmentConfigurationSet configurationSet)
        {
            var panel = ruleset.BuildPanelViewModel(state, configurationSet);
            panel.ActionWarnings.Add("phase-0-panel-shell");
            return panel;
        }
    }
}
