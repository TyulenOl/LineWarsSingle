using LineWars.Model;

namespace LineWars.Controllers
{
    public interface IExecutorConstrain
    {
        public bool CanCancelExecutor { get; }
        public bool CanSelectExecutor(IMonoExecutor executor);
    }
}