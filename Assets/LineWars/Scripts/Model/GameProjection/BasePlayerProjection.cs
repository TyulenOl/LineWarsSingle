//using System.Collections.Generic;
//using System.Linq;

//namespace LineWars.Model
//{
//    public class BasePlayerProjection : IReadOnlyBasePlayerProjection
//    {
//        public BasePlayer Player { get; private set; }
//        public int Money { get; set; }
//        public int Income { get; set; }
//        public List<Node> Nodes { get; set; }
//        public List<Unit> Units { get; set; }

//        public IReadOnlyList<Node> AllNodes => Nodes;
//        public IReadOnlyList<Unit> AllUnits => Units;

//        public BasePlayerProjection(BasePlayer basePlayer)
//        {
//            Player = basePlayer;
//            Money = basePlayer.CurrentMoney;
//            Income = basePlayer.Income;
//            Nodes = basePlayer.OwnedObjects
//                .Where(owned => owned is Node)
//                .Select(owned => (Node) owned)
//                .ToList();
//            Units = basePlayer.OwnedObjects
//                .Where(owned => owned is Unit)
//                .Select(owned => (Unit)owned)
//                .ToList();
//        }

//        public BasePlayerProjection(IReadOnlyBasePlayerProjection basePlayerProjection)
//        {
//            Player = basePlayerProjection.Player;
//            Money = basePlayerProjection.Money;
//            Income = basePlayerProjection.Income;
//            Nodes = new List<Node>(basePlayerProjection.AllNodes);
//            Units = new List<Unit>(basePlayerProjection.AllUnits);
//        }
//    }

//    public interface IReadOnlyBasePlayerProjection
//    {
//        BasePlayer Player { get; }
//        int Money { get; }
//        int Income { get; }
//        IReadOnlyList<Node> AllNodes { get; }
//        IReadOnlyList<Unit> AllUnits { get; } 
//    }
//}

