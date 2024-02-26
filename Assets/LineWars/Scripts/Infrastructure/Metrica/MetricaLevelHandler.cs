using LineWars.Controllers;

namespace LineWars.Infrastructure
{
    public class MetricaLevelHandler: LevelEventsHandler
    {
        private static SDKAdapterBase SDKAdapter => GameRoot.Instance?.SdkAdapter;
        
        protected override void LevelStarted()
        {
            if (SDKAdapter != null)
                SDKAdapter.SendStartLevelMetrica(Scene);
        }

        protected override void LevelFinished(LevelFinishStatus levelFinishStatus)
        {
            if (SDKAdapter != null)
            {
                SDKAdapter.SendFinishLevelMetrica(Scene, levelFinishStatus);
                if (levelFinishStatus != LevelFinishStatus.Exit)
                    SDKAdapter.SendFinishLevelMetrica(Scene, LevelFinishStatus.Exit);
            }
        }
    }
}