namespace LineWars.Model
{
    public class GetCommandsVisitor : IUnitActionVisitor<Node, Edge, Unit, Owned, BasePlayer>
    {
        public void Visit(BuildAction<Node, Edge, Unit, Owned, BasePlayer> action)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(BlockAction<Node, Edge, Unit, Owned, BasePlayer> action)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(MoveAction<Node, Edge, Unit, Owned, BasePlayer> action)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(HealAction<Node, Edge, Unit, Owned, BasePlayer> action)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(DistanceAttackAction<Node, Edge, Unit, Owned, BasePlayer> action)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(ArtilleryAttackAction<Node, Edge, Unit, Owned, BasePlayer> action)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(MeleeAttackAction<Node, Edge, Unit, Owned, BasePlayer> action)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(RLBlockAction<Node, Edge, Unit, Owned, BasePlayer> action)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(SacrificeForPerunAction<Node, Edge, Unit, Owned, BasePlayer> action)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(RamAction<Node, Edge, Unit, Owned, BasePlayer> action)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(BlowWithSwingAction<Node, Edge, Unit, Owned, BasePlayer> action)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(ShotUnitAction<Node, Edge, Unit, Owned, BasePlayer> action)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(RLBuildAction<Node, Edge, Unit, Owned, BasePlayer> action)
        {
            throw new System.NotImplementedException();
        }
    }
}