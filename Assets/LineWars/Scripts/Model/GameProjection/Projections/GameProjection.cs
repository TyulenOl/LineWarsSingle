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
        public Dictionary<BasePlayer, BasePlayerProjection> OriginalToProjectionPlayers { get; set; }
        public List<int> PlayersSequence { get; set; }
        public IndexList<BasePlayerProjection> PlayersIndexList { get; set; }
        public int CurrentPlayerPosition { get; set; }
        public GraphProjection Graph { get; set; }
        public PhaseType CurrentPhase { get; set; }     
        public PhaseOrderData PhaseOrder { get; set; }

        public BasePlayerProjection CurrentPlayer => 
            PlayersIndexList[PlayersSequence[CurrentPlayerPosition]];
  
        IReadOnlyIndexList<BasePlayerProjection> IReadOnlyGameProjection.PlayersIndexList 
            => PlayersIndexList;
        IReadOnlyList<int> IReadOnlyGameProjection.PlayersSequence => PlayersSequence;

        public IndexList<NodeProjection> NodesIndexList => Graph.NodesIndexList;
        public IndexList<EdgeProjection> EdgesIndexList => Graph.EdgesIndexList;
        public IndexList<UnitProjection> UnitsIndexList => Graph.UnitsIndexList;

        public void SetPlayers(IEnumerable<BasePlayerProjection> players)
        {
            OriginalToProjectionPlayers = new Dictionary<BasePlayer, BasePlayerProjection>();
            PlayersIndexList = new IndexList<BasePlayerProjection>();
            PlayersSequence = new List<int>();
            foreach (var player in players)
            {
                PlayersIndexList.Add(player.Id, player);
                PlayersSequence.Add(player.Id);
                player.Game = this;
                OriginalToProjectionPlayers[player.Original] = player;
            }
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

                var tempPlayerId = PlayersSequence[tempPlayerPosition];
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