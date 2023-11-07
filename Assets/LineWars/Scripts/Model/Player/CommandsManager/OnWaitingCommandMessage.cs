using System.Collections.Generic;
using System.Linq;
using LineWars.Model;

namespace LineWars.Controllers
{
    public class OnWaitingCommandMessage
    {
        public Node SelectedNode { get; }
        public IEnumerable<(ITarget,IActionCommand)> Data { get; }
        public IEnumerable<IActionCommand> AllCommands { get; }

        public OnWaitingCommandMessage(
            IEnumerable<(ITarget, IActionCommand)> data,
            Node selectedNode)
        {
            Data = data.ToArray();
            AllCommands = Data
                .Select(x => x.Item2)
                .ToHashSet();
            SelectedNode = selectedNode;
        }
    }
}