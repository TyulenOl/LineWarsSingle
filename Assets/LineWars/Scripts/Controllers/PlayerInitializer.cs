using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class PlayerInitializer: MonoBehaviour
    {
        [SerializeField] private Player playerPrefab;
        
        public Player Initialize(SpawnInfo spawnInfo)
        {
            var player = Instantiate(playerPrefab);
            player.Index = spawnInfo.PlayerIndex;
            foreach (var nodeInfo in spawnInfo.NodeInfos)
            {
                var node = nodeInfo.ReferenceToNode;
                Owned.Connect(player, node);
                
                if (nodeInfo.LeftUnitPrefab != null)
                {
                    var leftUnit = Instantiate(nodeInfo.LeftUnitPrefab);
                    leftUnit.Initialize(node, UnitDirection.Left);
                    node.LeftUnit = leftUnit;
                    leftUnit.transform.position = node.transform.position;
                    Owned.Connect(player, leftUnit);
                    leftUnit.transform.SetParent(player.transform);
                    
                    if (leftUnit.Size == UnitSize.Large)
                        node.RightUnit = leftUnit;
                }

                if (nodeInfo.RightUnitPrefab != null)
                {
                    if (nodeInfo.RightUnitPrefab.Size == UnitSize.Little && node.RightIsFree
                        ||
                        nodeInfo.RightUnitPrefab.Size == UnitSize.Large && node.LeftIsFree && node.RightIsFree)
                    {
                        var rightUnit = Instantiate(nodeInfo.RightUnitPrefab);
                        rightUnit.Initialize(node, UnitDirection.Right);
                        node.RightUnit = rightUnit;
                        rightUnit.transform.position = node.transform.position;
                        Owned.Connect(player, rightUnit);
                        rightUnit.transform.SetParent(player.transform);
                    
                        if (rightUnit.Size == UnitSize.Large)
                            node.LeftUnit = rightUnit;
                    }
                }
            }

            return player;
        }
        
    }
}