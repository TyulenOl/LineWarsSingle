using System.Collections.Generic;
using LineWars.Model;
using System.Linq;
using UnityEngine;

namespace LineWars
{
    public class KingOfMountainScoreReferee: ScoreReferee
    {
        [SerializeField] private Node mountain;
        public Node MountainNode => mountain;

        public override void Initialize(Player me, IEnumerable<BasePlayer> enemies)
        {
            base.Initialize(me, enemies);

            if (mountain == null)
            {
                Debug.LogWarning(@"При победе ""Царь горы"" не была назначена ""гора""");
            }
            
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
    }
}