using System;
using System.Collections.Generic;

namespace LineWars.Model
{
    public static class GameProjectionCreator
    {
        public static GameProjection FromMono(
            IEnumerable<BasePlayer> players,
            MonoGraph graph,
            PhaseManager phaseManager)
        {
            if (phaseManager.CurrentActor is not BasePlayer currentPlayer)
                throw new ArgumentException("Cannot get projection: IActor is in control");

            var newGame = new GameProjection();

            var playerDict = new Dictionary<BasePlayer, BasePlayerProjection>();
            var playerList = new List<BasePlayerProjection>();
            foreach (var player in players)
            {
                var playerProjection = BasePlayerProjectionCreator.FromMono(player, gameProjection: newGame);
                playerDict[player] = playerProjection;
                playerList.Add(playerProjection);
            }
            newGame.SetPlayers(playerList);
            var graphProjection = GraphProjectionCreator.FromMono(graph, playerDict, newGame);
            var currentPlayerProjection = playerDict[currentPlayer];
            var currentPlayerIndex =
                playerList.FindIndex((projection) => projection == currentPlayerProjection);

            newGame.Graph = graphProjection;
            newGame.CurrentPlayerPosition = currentPlayerIndex;
            newGame.CurrentPhase = phaseManager.CurrentPhase;
            newGame.PhaseOrder = phaseManager.OrderData;

            return newGame;
        }

        public static GameProjection FromProjection(IReadOnlyGameProjection oldProjection)
        {
            var newGame = new GameProjection();
            var oldPlayersToNew = new Dictionary<BasePlayerProjection, BasePlayerProjection>();
            var newPlayerList = new List<BasePlayerProjection>();
            foreach (var oldPlayer in oldProjection.PlayersIndexList.Values)
            {
                var newPlayerProjection = BasePlayerProjectionCreator.FromProjection(oldPlayer, gameProjection: newGame);
                newPlayerList.Add(newPlayerProjection);
                oldPlayersToNew[oldPlayer] = newPlayerProjection;

                if (newPlayerProjection.OwnedObjects.Count != 0) 
                    throw new ArgumentException();
            }

            var newGraphProjection = GraphProjectionCreator.FromProjection(oldProjection.Graph, oldPlayersToNew, newGame);

            foreach (var oldNewPlayerPair in oldPlayersToNew)
            {
                var baseId = oldNewPlayerPair.Key.Base.Id;
                var newBase = newGraphProjection.NodesIndexList[baseId];
                oldNewPlayerPair.Value.Base = newBase;
            }

            newGame.SetPlayers(newPlayerList);
            newGame.Graph = newGraphProjection;
            newGame.CurrentPhase = oldProjection.CurrentPhase;
            newGame.CurrentPlayerPosition = oldProjection.CurrentPlayerPosition;
            newGame.PhaseOrder = oldProjection.PhaseOrder;

            return newGame;
        }
    }
}
