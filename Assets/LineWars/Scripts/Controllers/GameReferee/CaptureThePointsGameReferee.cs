using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class CaptureThePointsGameReferee : ScoreReferee
    {
        [SerializeField] private List<Node> capturePoints;
        public IReadOnlyList<Node> CapturePoints => capturePoints;

        public void AddCapturePoint(Node node)
        {
            capturePoints.Add(node);
        }

        public bool RemoveCapturePoint(Node node)
        {
            return capturePoints.Remove(node);
        }

        public override void Initialize(Player me, IEnumerable<BasePlayer> enemies)
        {
            base.Initialize(me, enemies);

            if(capturePoints.Count <= 0)
            {
                Debug.LogWarning($"{nameof(CaptureThePointsGameReferee)} have not found any {nameof(capturePoints)}!");
            }

            foreach(var node in capturePoints)
            {
                node.Replenished += () =>
                {
                    if (node.Owner != null)
                    {
                        var score = 1;
                        if(TryGetComponent(out NodeScore scoreComponent))
                        {
                            score = scoreComponent.Score;
                        }
                        SetScoreForPlayer(
                            node.Owner,
                            GetScoreForPlayer(node.Owner)
                            + score);
                    }
                };
            }
        }
    }
}