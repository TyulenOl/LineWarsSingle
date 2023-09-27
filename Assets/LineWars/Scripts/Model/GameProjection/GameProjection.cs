//using System.Collections.Generic;
//using System.Linq;

//namespace LineWars.Model
//{
//    public class GameProjection : IReadOnlyGameProjection
//    {
//        public PhaseManager PhaseManager { get; private set; }
//        public PhaseType CurrentPhase { get; set; }
//        public BasePlayer CurrentPlayer { get; set; }
//        public Dictionary<BasePlayer, BasePlayerProjection> Players { get; set; }
//        public GraphProjection Graph { get; set; }

//        public IReadOnlyDictionary<BasePlayer, IReadOnlyBasePlayerProjection> AllPlayers 
//            => (IReadOnlyDictionary<BasePlayer, IReadOnlyBasePlayerProjection>) Players;
//        public IReadOnlyGraphProjection GraphProjection 
//            => Graph;

//        public GameProjection(Graph graph, PhaseManager manager, IEnumerable<BasePlayer> players)
//        {
//            PhaseManager = manager;
//            Graph = new GraphProjection(graph);
//            CurrentPhase = manager.CurrentPhase;
//            if(manager.CurrentActor is not BasePlayer player)
//                CurrentPlayer = null;
//            else
//                CurrentPlayer = player;
//            Graph = new GraphProjection(graph);

//            Players = players.ToDictionary(player => player, player => new BasePlayerProjection(player));
//        }

//        public GameProjection(IReadOnlyGameProjection gameProjection) 
//        {
//            PhaseManager = gameProjection.PhaseManager;
//            CurrentPhase = gameProjection.CurrentPhase;
//            CurrentPlayer = gameProjection.CurrentPlayer;

//            Players = new Dictionary<BasePlayer, BasePlayerProjection>();
//            foreach(var playerInfo in gameProjection.AllPlayers)
//                Players[playerInfo.Key] = new BasePlayerProjection(playerInfo.Value);

//            Graph = new GraphProjection(gameProjection.GraphProjection);
//        }

//    }

//    public interface IReadOnlyGameProjection
//    {
//        public PhaseManager PhaseManager { get; }
//        public PhaseType CurrentPhase { get; }
//        public BasePlayer CurrentPlayer { get; }
//        public IReadOnlyDictionary<BasePlayer, IReadOnlyBasePlayerProjection> AllPlayers { get; }
//        public IReadOnlyGraphProjection GraphProjection { get; }
//    }
//}
