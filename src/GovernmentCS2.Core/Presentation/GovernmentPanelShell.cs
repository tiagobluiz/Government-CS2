using GovernmentCS2.Core.Configuration;
using GovernmentCS2.Core.Contracts;
using GovernmentCS2.Core.Rulesets;
using System;

namespace GovernmentCS2.Core.Presentation
{
    public sealed class GovernmentPanelShell
    {
        public GovernmentPanelViewModel Build(IGovernmentRuleset ruleset, GovernmentModelState state, GovernmentConfigurationSet configurationSet)
        {
            if (ruleset == null)
            {
                throw new ArgumentNullException(nameof(ruleset));
            }

            var panel = ruleset.BuildPanelViewModel(state, configurationSet);
            if (panel == null)
            {
                throw new InvalidOperationException("Ruleset returned a null government panel view model.");
            }

            panel.ActionWarnings.Add("phase-0-panel-shell");
            return panel;
        }
    }
}
