using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineWars.Model
{
    public class MonoUpActionPointsAction :
        MonoUnitAction<UpActionPointsAction<Node, Edge, Unit>>,
        ITargetedAction<Unit>,
        IUpActionPointsAction<Node, Edge, Unit>
    {
        protected override UpActionPointsAction<Node, Edge, Unit> GetAction()
        {
            throw new NotImplementedException();
        }

        public void Execute(Unit target)
        {
            throw new NotImplementedException();
        }

        public IActionCommand GenerateCommand(Unit target)
        {
            throw new NotImplementedException();
        }

        public bool IsAvailable(Unit target)
        {
            throw new NotImplementedException();
        }
        public override void Accept(IMonoUnitActionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor)
        {
            throw new NotImplementedException();
        }
    }
}
