

namespace LineWars
{
    public static class NationController
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