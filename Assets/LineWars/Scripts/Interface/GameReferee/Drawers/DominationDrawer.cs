using System;
using LineWars.Model;
using TMPro;
using UnityEngine;

namespace LineWars.Interface
{
    public class DominationDrawer: ConcreteGameRefereeDrawer
    {
        [SerializeField] private TMP_Text rusScore;
        [SerializeField] private TMP_Text lizScore;
        [SerializeField] private TextAnimator roundText;
        public override Type GameRefereeType => typeof(NewDominationGameReferee);
        public override void Show(GameReferee gameReferee)
        {
            if (gameReferee is NewDominationGameReferee newDominationGameReferee)
            {
                gameObject.SetActive(true);
                rusScore.text = newDominationGameReferee.GetScoreForPlayer().ToString();
                lizScore.text = newDominationGameReferee.GetScoreForEnemies().ToString();
                roundText.SetTextImmediate(newDominationGameReferee.RoundsToWin.ToString());
                
                newDominationGameReferee.ScoreChanged += OnScoreChanged;
                newDominationGameReferee.RoundsAmountChanged += OnRoundChanged;
            }
        }

        private void OnRoundChanged(int cur, int max)
        {
            roundText.SetText($"{max - cur}");
        }

        private void OnScoreChanged(BasePlayer player, int score)
        {
            if (player == Player.LocalPlayer)
            {
                rusScore.text = score.ToString();
            }
            else
            {
                lizScore.text = score.ToString();
            }
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}