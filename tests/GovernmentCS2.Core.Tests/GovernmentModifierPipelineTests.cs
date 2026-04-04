using GovernmentCS2.Core.Contracts;
using GovernmentCS2.Core.Runtime;
using Xunit;

namespace GovernmentCS2.Core.Tests
{
    public class GovernmentModifierPipelineTests
    {
        [Fact]
        public void ApplyDemandEffects_ClampsValuesToConfiguredCap()
        {
            var pipeline = new GovernmentModifierPipeline();
            var configurationSet = TestConfigFactory.CreateConfigurationSet();
            var input = new GovernmentDemandEffects
            {
                ResidentialModifier = 30f,
                CommercialModifier = -30f,
                IndustrialModifier = 3f,
                OfficeModifier = -4f,
                ConfidenceChannelContribution = 20f,
                PolicyDirectionChannelContribution = -20f
            };

            var output = pipeline.ApplyDemandEffects(input, configurationSet);

            Assert.Equal(15f, output.ResidentialModifier);
            Assert.Equal(-15f, output.CommercialModifier);
            Assert.Equal(15f, output.ConfidenceChannelContribution);
            Assert.Equal(-15f, output.PolicyDirectionChannelContribution);
            Assert.Contains("phase-0-modifier-pipeline", output.ReasonCodes);
        }
    }
}
