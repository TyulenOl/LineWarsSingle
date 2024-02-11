using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public abstract class CommandsManagerConstrainsBase : MonoBehaviour,
        IExecutorConstrain,
        ICommandTypeConstrain,
        ITargetConstrain,
        ISimpleCommandConstrain,
        ICurrentCommandConstrain,
        IBuyConstrain,
        IBlessingConstrain
    {
        public abstract bool CanCancelExecutor { get; }
        public abstract bool CanSelectExecutor(IMonoExecutor executor);
        public abstract bool CanSelectTarget(int targetId, IMonoTarget target);
        public abstract bool IsMyCommandType(CommandType commandType);
        public abstract bool CanExecuteSimpleAction();
        public abstract bool CanSelectCurrentCommand();
        public abstract bool CanSelectNode(Node node);
        public abstract bool CanSelectDeckCard(DeckCard deckCard);
        public abstract bool CanSelectBlessing(BlessingId blessingId);
    }
}