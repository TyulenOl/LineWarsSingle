using System.Collections.Generic;
using System.Linq;
using LineWars.Model;

namespace LineWars.Controllers
{
    public class OnWaitingCommandMessage
    {
        public Node SelectedNode { get; }
        public IEnumerable<IExecutorAction> AllActions { get; }
        public IEnumerable<CommandPreset> Data { get; }

        public OnWaitingCommandMessage(
            IEnumerable<CommandPreset> data,
            Node selectedNode)
        {
            Data = data.ToArray();
            AllActions = Data
                .Select(x => x.Action)
                .ToHashSet();
            SelectedNode = selectedNode;
        }
    }

    public class CommandPreset
    {
        public ITarget Target { get; }
        public IExecutorAction Action { get; }

        public CommandPreset(ITarget target, IExecutorAction action)
        {
            Target = target;
            Action = action;
        }
    }
}