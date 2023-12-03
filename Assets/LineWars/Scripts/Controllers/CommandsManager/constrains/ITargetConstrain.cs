using LineWars.Model;

namespace LineWars.Controllers
{
    public interface ITargetConstrain
    {
        public bool CanSelectTarget(int targetId, IMonoTarget target);
    }
}