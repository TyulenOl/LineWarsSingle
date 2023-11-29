using System.Collections.Generic;
using System.Linq;
using LineWars.Model;

namespace LineWars.Controllers
{
    public class OnWaitingCommandMessage
    {
        public Node SelectedNode { get; }
        public IEnumerable<CommandPreset> Data { get; }

        public OnWaitingCommandMessage(
            IEnumerable<CommandPreset> data,
            Node selectedNode)
        {
            Data = data.ToHashSet();
            SelectedNode = selectedNode;
        }
    }

    public class CommandPreset
    {
        public IExecutorAction Action { get; }
        public IMonoTarget Target { get; }
        public IMonoExecutor Executor { get; }
        
        public bool IsActive { get; }

        public CommandPreset(
            IMonoExecutor executor,
            IMonoTarget target,
            IExecutorAction action,
            bool isActive)
        {
            Target = target;
            Action = action;
            IsActive = isActive;
            Executor = executor;
        }
    }
}