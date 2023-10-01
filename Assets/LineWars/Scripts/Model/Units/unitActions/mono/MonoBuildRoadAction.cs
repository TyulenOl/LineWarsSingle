namespace LineWars.Model
{
    public class MonoBuildRoadAction: MonoUnitAction,
        IBuildAction<Node, Edge, Unit, Owned, BasePlayer, Nation>
    {
        private BuildAction<Node, Edge, Unit, Owned, BasePlayer, Nation> BuildAction
            => (BuildAction<Node, Edge, Unit, Owned, BasePlayer, Nation>) ExecutorAction;
        protected override ExecutorAction GetAction()
        {
            return new BuildAction<Node, Edge, Unit, Owned, BasePlayer, Nation>(GetComponent<Unit>(), this);
        }

        public bool CanUpRoad(Edge edge, bool ignoreActionPointsCondition = false) =>
            BuildAction.CanUpRoad(edge, ignoreActionPointsCondition);

        public bool CanUpRoad(Edge edge, Node node, bool ignoreActionPointsCondition = false) =>
            BuildAction.CanUpRoad(edge, node, ignoreActionPointsCondition);

        public void UpRoad(Edge edge) => BuildAction.UpRoad(edge);
    }
}