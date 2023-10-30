using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LineWars.Model
{
    public static class GameProjectionInfo
    {
        public static void LogInfo(GameProjection projection)
        {
            Debug.Log("----GAME INFO----");
            foreach(var playerId in projection.PlayersSequence)
            {
                Debug.Log($"--Checking Player {playerId}--");
                var playerProjection = projection.PlayersIndexList[playerId];

                var unitsCount = projection.UnitsIndexList.Where(pair => pair.Value.Owner == playerProjection).Count();
                Debug.Log($"Units count: {unitsCount}");

                var nodesCount = projection.NodesIndexList.Where(pair => pair.Value.Owner == playerProjection).Count();
                Debug.Log($"Nodes count: {nodesCount}");
                Debug.Log($"Money: {playerProjection.CurrentMoney}");
            }
            Debug.Log("----END----");
            
        }
    }
}