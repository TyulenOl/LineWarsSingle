using DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    public class GameProjection : IReadOnlyGameProjection
    { 
        private static int cycleTurnFailCounter = 1000;
        private List<int> playersSequence;
        private Dictionary<BasePlayer, BasePlayerProjection> originalToProjectionPlayers;
        public GraphProjection Graph { get; private set; }
        public PhaseType CurrentPhase { get; private set; }     
        public PhaseOrderData PhaseOrder { get; private set; }
        public int CurrentPlayerPosition { get; private set; }
        public IndexList<BasePlayerProjection> PlayersIndexList { get; private set; }

        public BasePlayerProjection CurrentPlayer => 
            PlayersIndexList[playersSequence[CurrentPlayerPosition]];
        public IReadOnlyList<int> PlayersSequence => playersSequence;
        IReadOnlyIndexList<BasePlayerProjection> IReadOnlyGameProjection.PlayersIndexList 
            => PlayersIndexList;

        public IndexList<NodeProjection> NodesIndexList => Graph.NodesIndexList;
        public IndexList<EdgeProjection> EdgesIndexList => Graph.EdgesIndexList;
        public IndexList<UnitProjection> UnitsIndexList => Graph.UnitsIndexList;
        public IReadOnlyDictionary<BasePlayer, BasePlayerProjection> OriginalToProjectionPlayers 
            => originalToProjectionPlayers;

        public GameProjection(IEnumerable<BasePlayerProjection> players, GraphProjection graph,
            PhaseType phase, int playerPosition, PhaseOrderData orderData)
        {
            originalToProjectionPlayers = new Dictionary<BasePlayer, BasePlayerProjection>();
            PlayersIndexList = new IndexList<BasePlayerProjection>();
            playersSequence = new List<int>();
            foreach (var player in players)
            {
                PlayersIndexList.Add(player.Id, player);
                playersSequence.Add(player.Id);
                player.Game = this;
                originalToProjectionPlayers[player.Original] = player;
            }

            Graph = graph;
            CurrentPhase = phase;
            CurrentPlayerPosition = playerPosition;
            PhaseOrder = orderData;
        }
        public void SimulateReplenish()
        {
            foreach(var player in PlayersIndexList.Values)
            {
                player.SimulateReplenish();
            }
        }

        public void CycleTurn()
        {
            CurrentPhase = FindNextViablePhaseType(CurrentPhase);
            if (PhaseHelper.TypeToMode[CurrentPhase] != PhaseMode.Simultaneous)
                CyclePlayers(CurrentPhase);
        }

        public void CyclePlayers() => CyclePlayers(CurrentPhase);
        public void CyclePlayers(PhaseType phase)
        {
            var failCounter = 0;

            var tempPlayerPosition = CurrentPlayerPosition;
            while(true)
            {
                tempPlayerPosition = (tempPlayerPosition + 1) % PlayersIndexList.Count; 

                var tempPlayerId = playersSequence[tempPlayerPosition];
                var tempPlayer = PlayersIndexList[tempPlayerId];
                if (CanPlayerPlayTurn(tempPlayer, phase))
                    break;

                failCounter++;
                if (failCounter > cycleTurnFailCounter)
                    throw new ArgumentException($"Failed to find new player {phase}");
            }

            CurrentPlayerPosition = tempPlayerPosition;
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
                    throw new ArgumentException($"GameProjection failed to cycle turn! {currentPhase}");
            }

            return tempPhase;
        }

        public bool IsUnitPhaseAvailable() => IsUnitPhaseAvailable(CurrentPhase);
        public bool IsUnitPhaseAvailable(PhaseType phase) 
        {
            foreach(var player in PlayersIndexList.Values)
            {
                if(CanPlayerPlayTurn(player, phase)) return true;
            }

            return false;
        }

        public bool CanPlayerPlayTurn(BasePlayerProjection player, PhaseType phase)
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
            foreach (var oldPlayer in oldProjection.PlayersIndexList.Values)
            {
                var newPlayerProjection = new BasePlayerProjection(oldPlayer);
                newPlayerList.Add(newPlayerProjection);
                oldPlayersToNew[oldPlayer] = newPlayerProjection;

                if (newPlayerProjection.OwnedObjects.Count != 0) throw new ArgumentException("YOU FUCKING IDIOT");
            }


            var newGraphProjection = GraphProjection.GetCopy(oldProjection.Graph, oldPlayersToNew);

            foreach(var oldNewPlayerPair in oldPlayersToNew)
            {
                var baseId = oldNewPlayerPair.Key.Base.Id;
                var newBase = newGraphProjection.NodesIndexList[baseId];
                oldNewPlayerPair.Value.Base = newBase;
            }
            return new GameProjection(newPlayerList, newGraphProjection, oldProjection.CurrentPhase, 
                oldProjection.CurrentPlayerPosition, oldProjection.PhaseOrder);
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
        public int CurrentPlayerPosition { get; }
        public IReadOnlyIndexList<BasePlayerProjection> PlayersIndexList { get; }
        public IReadOnlyList<int> PlayersSequence { get; }
        public GraphProjection Graph { get; }
        public PhaseOrderData PhaseOrder { get; }
        public PhaseType CurrentPhase { get; }
        public BasePlayerProjection CurrentPlayer 
            => PlayersIndexList[PlayersSequence[CurrentPlayerPosition]];
    }
}