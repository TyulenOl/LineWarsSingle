

namespace LineWars.Controllers
{
    public static class NationController
    {
        public static INation GetNation(NationType type)
        {
            switch (type)
            {
                case NationType.Russia:
                    return new Russia();
                default:
                    return new DefaultNation();
            }
        }
    }
}