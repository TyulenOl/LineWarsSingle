namespace LineWars.Model
{
    public class MonoConsumeUnitAction :
        MonoUnitAction<ConsumeUnitAction<Node, Edge, Unit>>,
        ITargetedAction<Unit>,
        IConsumeUnitAction<Node, Edge, Unit>
    {
        protected override bool NeedAutoComplete => false;
        protected override ConsumeUnitAction<Node, Edge, Unit> GetAction()
        {
            return new ConsumeUnitAction<Node, Edge, Unit>(Executor);
        }

        public bool IsAvailable(Unit target)
        {
            return Action.IsAvailable(target);
        }

        public void Execute(Unit target)
        {
            var responses = target.GetComponent<AnimationResponses>();
            if(responses == null || 
                !responses.CanRespond(AnimationResponseType.Sacrificed) ||
                !responses.CanRespond(AnimationResponseType.ComeTo))
            {
                ExecuteInstant(target);
                return;
            }
            ExecuteAnimation(target);
        }

        private void ExecuteInstant(Unit target)
        {
            Action.Execute(target);
            Complete();
        }

        private void ExecuteAnimation(Unit target)
        {
            var responses = target.GetComponent<AnimationResponses>();
            var comeContext = new AnimationContext()
            {
                TargetNode = Executor.Node
            };
            var comeResponse = responses.Respond(AnimationResponseType.ComeTo, comeContext);
            comeResponse.Ended.AddListener(OnComeEnd);   
            void OnComeEnd(UnitAnimation comeAnim)
            {
                comeAnim.Ended.RemoveListener(OnComeEnd);
                var sacrContext = new AnimationContext()
                {
                    TargetNode = Executor.Node
                };
                var sacrResponse = responses.Respond(AnimationResponseType.Sacrificed, sacrContext);
                sacrResponse.Ended.AddListener(OnSacrEnd);
            }
            void OnSacrEnd(UnitAnimation sacrAnim)
            {
                sacrAnim.Ended.RemoveListener(OnSacrEnd);
                Action.Execute(target);
                Complete();
            }
        }

        public IActionCommand GenerateCommand(Unit target)
        {
            return new TargetedUniversalCommand<
                Unit,
                MonoConsumeUnitAction,
                Unit>(this, target);
        }


        public override void Accept(IMonoUnitActionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
