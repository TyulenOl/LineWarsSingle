using System.Collections.Generic;
using LineWars.Model;
using System.Linq;
using UnityEngine;

namespace LineWars
{
    public class KingOfMountainScoreReferee: ScoreReferee
    {
        private Node mountain;
        public Node MountainNode => mountain;

        public override void Initialize(Player me, IEnumerable<BasePlayer> enemies)
        {
            base.Initialize(me, enemies);
            FindMountainNode();
            
            mountain.Replenished += () =>
            {
                if (mountain.Owner != null)
                {
                    SetScoreForPlayer(
                        mountain.Owner,
                        GetScoreForPlayer(mountain.Owner)
                        + mountain.GetComponent<NodeScore>().Score);
                }
            };
        }
        
        private void FindMountainNode()
        {
            var allMountains = MonoGraph.Instance.Nodes
                .Where(x => x.TryGetComponent<NodeScore>(out var _))
                .ToList();
            
            var countOfNodeScore = allMountains.Count;
            if (countOfNodeScore == 0)
            {
                Debug.LogWarning(@"При победе ""Царь горы"" на карте не было обнаружено ни одной ноды - горы");
                var randomNode = MonoGraph.Instance.Nodes[Random.Range(0, MonoGraph.Instance.Nodes.Count)];
                randomNode.gameObject.AddComponent<NodeScore>();
                mountain = randomNode;
            }
            else if (countOfNodeScore > 1)
            {
                Debug.LogWarning(@"При победе ""Царь горы"" на карте было обнаружено много нод-гор");
                var randomNode = allMountains[Random.Range(0, allMountains.Count)];
                foreach (var node in allMountains.Where(x => x != randomNode))
                    Destroy(node.GetComponent<NodeScore>());

                mountain = randomNode;
            }
            else
            {
                mountain = allMountains.First();
            }
        }
    }
}