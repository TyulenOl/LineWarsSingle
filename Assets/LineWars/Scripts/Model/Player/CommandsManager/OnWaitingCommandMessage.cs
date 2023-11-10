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
        public ITarget Target { get; }

        public CommandPreset(ITarget target, IExecutorAction action)
        {
            Target = target;
            Action = action;
        }
    }
}