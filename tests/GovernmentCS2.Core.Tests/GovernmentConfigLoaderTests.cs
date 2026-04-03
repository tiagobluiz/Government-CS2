using System.Text.Json;
using GovernmentCS2.Core.Configuration;
using GovernmentCS2.Core.Contracts;
using Xunit;

namespace GovernmentCS2.Core.Tests
{
    public class GovernmentConfigLoaderTests
    {
        private readonly GovernmentConfigLoader myLoader = new GovernmentConfigLoader();

        [Fact]
        public void LoadFromJson_LoadsValidConfigurationSet()
        {
            var configurationSet = myLoader.LoadFromJson(CreateCoreJson(), CreateDemocracyJson(), "test-config");

            Assert.Equal(GovernmentRulesetIds.Democracy, configurationSet.Core.DefaultRulesetId);
            Assert.Equal(4, configurationSet.Democracy.TermLengthYears);
            Assert.Equal(6, configurationSet.Democracy.Blocs.Count);
            Assert.Equal(3, configurationSet.Democracy.Parties.Count);
        }

        [Fact]
        public void LoadFromJson_RejectsMissingBlocDefinition()
        {
            var democracy = TestConfigFactory.CreateDemocracyConfig();
            democracy.Blocs.RemoveAt(democracy.Blocs.Count - 1);

            var exception = Assert.Throws<GovernmentConfigException>(() =>
                myLoader.LoadFromJson(CreateCoreJson(), JsonSerializer.Serialize(democracy), "test-config"));

            Assert.Contains("exactly 6 blocs", exception.Message);
        }

        [Fact]
        public void LoadFromJson_RejectsInvalidDemandCap()
        {
            var democracy = TestConfigFactory.CreateDemocracyConfig();
            democracy.Demand.GlobalCap = 0f;

            var exception = Assert.Throws<GovernmentConfigException>(() =>
                myLoader.LoadFromJson(CreateCoreJson(), JsonSerializer.Serialize(democracy), "test-config"));

            Assert.Contains("demand.globalCap", exception.Message);
        }

        private static string CreateCoreJson()
        {
            return JsonSerializer.Serialize(TestConfigFactory.CreateCoreConfig());
        }

        private static string CreateDemocracyJson()
        {
            return JsonSerializer.Serialize(TestConfigFactory.CreateDemocracyConfig());
        }
    }
}
