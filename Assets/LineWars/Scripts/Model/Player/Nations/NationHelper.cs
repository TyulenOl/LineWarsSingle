

namespace LineWars.Model
{
    public static class NationHelper
    {
        public static INation GetNation(NationType type)
        {
            return type switch
            {
                NationType.Russia => new Russia(),
                _ => new DefaultNation()
            };
        }
    }
}