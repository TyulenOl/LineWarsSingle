using System;
using LineWars.Model;
using TMPro;
using UnityEngine;

namespace LineWars.Interface
{
    public class KingHillDrawer: ConcreteGameRefereeDrawer
    {
        [SerializeField] private TMP_Text rusScore;
        [SerializeField] private TMP_Text lizScore;
        
        public override Type GameRefereeType => typeof(KingOfMountainScoreReferee);
        public override void Show(GameReferee gameReferee)
        {
            if (gameReferee is KingOfMountainScoreReferee kingOfMountainScoreReferee)
            {
                gameObject.SetActive(true);
                
                rusScore.text = $"{kingOfMountainScoreReferee.GetScoreForPlayer()}/{kingOfMountainScoreReferee.ScoreForWin}";
                lizScore.text = $"{kingOfMountainScoreReferee.GetScoreForEnemies()}/{kingOfMountainScoreReferee.ScoreForWin}";
                kingOfMountainScoreReferee.ScoreChanged += OnScoreChanged;
            }
        }

        private void OnScoreChanged(BasePlayer player, int cur, int max)
        {
            if (player == Player.LocalPlayer)
            {
                rusScore.text = $"{cur}/{max}";
            }
            else
            {
                lizScore.text = $"{cur}/{max}";
            }
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}