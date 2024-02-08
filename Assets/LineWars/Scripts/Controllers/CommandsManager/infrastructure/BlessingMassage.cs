using LineWars.Model;

namespace LineWars.Controllers
{
    public class BlessingMassage
    {
        public BlessingMassage(BlessingId blessingData)
        {
            Data = blessingData;
        }

        public BlessingId Data { get; }
    }
}