using System.Collections.Generic;
using System.Linq;
using LineWars.Model;

namespace LineWars.Controllers
{
    public class OnWaitingCommandMessage
    {
        public Node SelectedNode { get; }
        public IEnumerable<ITargetedAction> AllActions { get; }
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
        public ITargetedAction Action { get; }

        public CommandPreset(ITarget target, ITargetedAction action)
        {
            Target = target;
            Action = action;
        }
    }
}