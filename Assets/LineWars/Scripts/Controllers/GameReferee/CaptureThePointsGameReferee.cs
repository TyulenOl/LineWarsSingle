using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    /// <summary>
    /// Захват ключевых точек, каждый раунд за кахжую точку дается 1 очко
    /// </summary>
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

        public override void Initialize(Player player, IEnumerable<BasePlayer> enemies)
        {
            base.Initialize(player, enemies);

            if(capturePoints.Count <= 0)
            {
                Debug.LogWarning($"{nameof(CaptureThePointsGameReferee)} have not found any {nameof(capturePoints)}!");
            }

            PhaseManager.Instance.PhaseEntered.AddListener(OnPhaseEntered);
        }

        private void OnPhaseEntered(PhaseType phaseEntered)
        {
            if (phaseEntered != PhaseType.Replenish)
                return;
            foreach (var node in capturePoints)
            {
                if (node.Owner != null)
                {
                    var score = 1;
                    if (TryGetComponent(out NodeScore scoreComponent))
                    {
                        score = scoreComponent.Score;
                    }
                    SetScoreForPlayer(
                        node.Owner,
                        GetScoreForPlayer(node.Owner)
                        + score);
                }
            }
        }
    }
}