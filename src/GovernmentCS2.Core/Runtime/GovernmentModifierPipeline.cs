using System;
using GovernmentCS2.Core.Configuration;
using GovernmentCS2.Core.Contracts;

namespace GovernmentCS2.Core.Runtime
{
    public sealed class GovernmentModifierPipeline
    {
        public GovernmentDemandEffects ApplyDemandEffects(GovernmentDemandEffects input, GovernmentConfigurationSet configurationSet)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (configurationSet == null)
            {
                throw new ArgumentNullException(nameof(configurationSet));
            }

            var cap = Math.Min(configurationSet.Core.DefaultDemandModifierCap, configurationSet.Democracy.Demand.GlobalCap);

            return new GovernmentDemandEffects
            {
                ResidentialModifier = Clamp(input.ResidentialModifier, cap),
                CommercialModifier = Clamp(input.CommercialModifier, cap),
                IndustrialModifier = Clamp(input.IndustrialModifier, cap),
                OfficeModifier = Clamp(input.OfficeModifier, cap),
                ConfidenceChannelContribution = Clamp(input.ConfidenceChannelContribution, cap),
                PolicyDirectionChannelContribution = Clamp(input.PolicyDirectionChannelContribution, cap),
                ReasonCodes = MergeReasonCodes(input)
            };
        }

        private static float Clamp(float value, float cap)
        {
            return Math.Max(-cap, Math.Min(cap, value));
        }

        private static System.Collections.Generic.IList<string> MergeReasonCodes(GovernmentDemandEffects input)
        {
            var reasons = new System.Collections.Generic.List<string>(input.ReasonCodes);
            reasons.Add("phase-0-modifier-pipeline");
            return reasons;
        }
    }
}
