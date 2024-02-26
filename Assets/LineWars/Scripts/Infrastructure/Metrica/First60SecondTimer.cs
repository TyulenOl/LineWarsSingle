using System;

namespace LineWars.Infrastructure
{
    public class First60SecondTimer: TimerHandler
    {
        private void OnValidate()
        {
            timeInSeconds = 60;
        }

        protected override void TimeOut()
        {
            if (SDKAdapter != null)
                SDKAdapter.SendFirst60SecondsInGameMetrica();
        }
    }
}