using System;
using System.IO;
using System.Reflection;
using Colossal.Logging;
using GovernmentCS2.Core.Configuration;
using GovernmentCS2.Core.Contracts;
using GovernmentCS2.Core.Runtime;

namespace GovernmentCS2
{
    internal sealed class GovernmentModHost : IDisposable
    {
        private readonly ILog myLog;

        public GovernmentModHost(ILog log)
        {
            myLog = log;
        }

        public GovernmentModule GovernmentModule { get; private set; }

        public void Initialize()
        {
            var baseDirectory = GetBaseDirectory();
            var loader = new GovernmentConfigLoader();
            var configurationSet = loader.LoadFromBaseDirectory(baseDirectory);

            GovernmentModule = GovernmentModule.CreateDefault(configurationSet);
            var bootstrap = GovernmentModule.Initialize(
                GovernmentInitializationMode.NewCity,
                new GovernmentInitializationContext
                {
                    CityName = "Bootstrap",
                    CurrentGameTime = 0L
                });

            myLog.Info($"Government module initialized from '{configurationSet.SourceDirectory}'.");
            myLog.Info($"Government configuration summary: {configurationSet.Describe()}");
            myLog.Info($"Government bootstrap state: approval={bootstrap.State.Approval}, legitimacy={bootstrap.State.Legitimacy}, risk={bootstrap.RuntimeState.CurrentRiskLevel}");
            myLog.Info($"Government panel shell summary: {bootstrap.DebugSnapshot.PanelSummary}");
        }

        public void Dispose()
        {
            GovernmentModule?.Dispose();
            GovernmentModule = null;
        }

        private static string GetBaseDirectory()
        {
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            if (string.IsNullOrWhiteSpace(assemblyLocation))
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }

            return Path.GetDirectoryName(assemblyLocation) ?? AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
