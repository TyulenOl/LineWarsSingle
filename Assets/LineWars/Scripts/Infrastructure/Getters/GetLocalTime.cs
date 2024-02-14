using System;

namespace LineWars.Model
{
    public class GetLocalTime : IGetter<DateTime>
    {
        public bool CanGet()
        {
            return true;
        }

        public DateTime Get()
        {
            return DateTime.Now;
        }
    }
}
