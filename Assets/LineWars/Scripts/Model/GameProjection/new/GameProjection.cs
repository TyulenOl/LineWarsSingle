using System;
using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public class GameProjection : IReadOnlyGameProjection
    {
        private static int cycleTurnFailCounter = 1000;
        private List<BasePlayerProjection> players;
        public GraphProjection Graph { get; private set; }
        public PhaseType CurrentPhase { get; private set; }    
        public PhaseOrderData PhaseOrder { get; private set; }
        public int CurrentPlayerIndex { get; private set; }

        public IReadOnlyList<BasePlayerProjection> Players => players;
        
        public GameProjection(IEnumerable<BasePlayerProjection> players, GraphProjection graph, 
            PhaseType phase, int playerIndex, PhaseOrderData orderData)
        {
            this.players = new List<BasePlayerProjection>(players);
            Graph = graph;
            CurrentPhase = phase;
            CurrentPlayerIndex = playerIndex;
            PhaseOrder = orderData;
        }

        public GameProjection(IReadOnlyGameProjection projection)
            => GetCopy(projection);

        public GameProjection(IEnumerable<BasePlayer> players, MonoGraph graph,
            PhaseManager phaseManager) => GetProjectionFromMono(players, graph, phaseManager);

        public void SimulateReplenish()
        {
            foreach(var player in players)
            {
                player.SimulateReplenish();
            }
        }

        public void CycleTurn()
        {
            CurrentPhase = FindNextViablePhaseType(CurrentPhase);
            CurrentPlayerIndex = FindNewPlayer(CurrentPhase);
        }

        private int FindNewPlayer(PhaseType phase)
        {
            var failCounter = 0;

            var tempPlayerId = CurrentPlayerIndex;
            while(true)
            {
                tempPlayerId = (tempPlayerId + 1) % players.Count; 

                var tempPlayer = players[tempPlayerId];
                if (CanPlayerPlayTurn(tempPlayer, phase))
                    break;

                failCounter++;
                if (failCounter > cycleTurnFailCounter)
                    throw new ArgumentException("Failed to find new player");
            }

            return tempPlayerId;
        }

        private PhaseType FindNextViablePhaseType(PhaseType currentPhase)
        {
            var tempPhase = currentPhase;
            var failCounter = 0;
            while (true)
            {
                tempPhase = PhaseHelper.Next(tempPhase, PhaseOrder);
                if (tempPhase == PhaseType.Replenish)
                {
                    SimulateReplenish();
                    continue;
                }
                if (PhaseHelper.TypeToMode[tempPhase] == PhaseMode.NotPlayable) continue;
                if (PhaseHelper.TypeToMode[tempPhase] == PhaseMode.Simultaneous)
                    break;
                if (IsUnitPhaseAvailable(tempPhase))
                    break;

                failCounter++;
                if (failCounter > cycleTurnFailCounter)
                    throw new ArgumentException("GameProjection failed to cycle turn!");
            }

            return tempPhase;
        }

        private bool IsUnitPhaseAvailable(PhaseType phase) 
        {
            foreach(var player in players)
            {
                if(CanPlayerPlayTurn(player, phase)) return true;
            }

            return false;
        }

        private bool CanPlayerPlayTurn(BasePlayerProjection player, PhaseType phase)
        {
            foreach(var owned in player.OwnedObjects)
            {
                if (owned is not UnitProjection unit) continue;
                if(unit.CurrentActionPoints > 0 
                    && player.PhaseExecutorsData[phase].Contains(unit.Type))
                {
                    return true;
                }
            }

            return false;
        }

        public static GameProjection GetCopy(IReadOnlyGameProjection oldProjection)
        {
            var oldPlayersToNew = new Dictionary<BasePlayerProjection, BasePlayerProjection>();
            var newPlayerList = new List<BasePlayerProjection>();
            foreach (var oldPlayer in oldProjection.Players)
            {
                var newPlayerProjection = new BasePlayerProjection(oldPlayer);
                newPlayerList.Add(newPlayerProjection);
            }

            var newGraphProjection = GraphProjection.GetCopy(oldProjection.Graph, oldPlayersToNew);
            return new GameProjection(newPlayerList, newGraphProjection, oldProjection.CurrentPhase, 
                oldProjection.CurrentPlayerIndex, oldProjection.PhaseOrder);
        }

        public static GameProjection 
            GetProjectionFromMono(IEnumerable<BasePlayer> players, MonoGraph graph, 
            PhaseManager phaseManager)
        {
            if(phaseManager.CurrentActor is not BasePlayer currentPlayer)    
                throw new ArgumentException("Cannot get projection: IActor is in control");

            var playerDict = new Dictionary<BasePlayer, BasePlayerProjection>();
            var playerList = new List<BasePlayerProjection>();
            foreach (var player in players)
            {
                var playerProjection = new BasePlayerProjection(player);
                playerDict[player] = playerProjection;
                playerList.Add(playerProjection);
            }

            var graphProjection = GraphProjection.GetProjectionFromMono(graph, playerDict);
            var currentPlayerProjection = playerDict[currentPlayer];

            var currentPlayerIndex = 
                playerList.FindIndex((projection) => projection == currentPlayerProjection);
            return new GameProjection(playerList, graphProjection, 
                phaseManager.CurrentPhase, currentPlayerIndex, phaseManager.OrderData);
        }
    }

    public interface IReadOnlyGameProjection
    {
        public int CurrentPlayerIndex { get; }
        public IReadOnlyList<BasePlayerProjection> Players { get; }
        public GraphProjection Graph { get; }
        public PhaseOrderData PhaseOrder { get; }
        public PhaseType CurrentPhase { get; }
        public BasePlayerProjection CurrentPlayer => Players[CurrentPlayerIndex];
    }
}