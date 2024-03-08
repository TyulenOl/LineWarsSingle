using System.Collections.Generic;
using LineWars.Model;
using System.Linq;
using UnityEngine;

namespace LineWars
{
    
    /// <summary>
    /// Захвати гору, за нее каждый раунд дается одно очко.
    /// </summary>
    public class KingOfMountainScoreReferee: ScoreReferee
    {
        [SerializeField] private Node mountain;
        public Node MountainNode => mountain;

        public override void Initialize(Player player, IEnumerable<BasePlayer> enemies)
        {
            base.Initialize(player, enemies);

            if (mountain == null)
            {
                Debug.LogWarning(@"При победе ""Царь горы"" не была назначена ""гора""");
            }

            PhaseManager.Instance.PhaseEntered.AddListener(OnPhaseEntered);
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

        public void OnPhaseEntered(PhaseType phase)
        {
            {

            }
        }
    }
}