using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;

namespace GovernmentCS2
{
    public class Mod : IMod
    {
        public static ILog log = LogManager.GetLogger($"{nameof(GovernmentCS2)}.{nameof(Mod)}").SetShowsErrorsInUI(false);
        private GovernmentModHost myGovernmentModHost;

        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");

            myGovernmentModHost = new GovernmentModHost(log);
            myGovernmentModHost.Initialize();
        }

        public void OnDispose()
        {
            myGovernmentModHost?.Dispose();
            myGovernmentModHost = null;
            log.Info(nameof(OnDispose));
        }
    }
}
