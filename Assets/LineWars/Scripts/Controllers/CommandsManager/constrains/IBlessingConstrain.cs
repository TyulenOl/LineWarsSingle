using LineWars.Model;

namespace LineWars.Controllers
{
    public interface IBlessingConstrain
    {
        public bool CanSelectBlessing(BlessingId blessingId);
    }
}