using System;
using LineWars.Model;
using TMPro;
using UnityEngine;

namespace LineWars.Interface
{
    public class WallToWallDrawer: ConcreteGameRefereeDrawer
    {
        [SerializeField] private TMP_Text rusScore;
        [SerializeField] private TMP_Text lizScore;
        public override Type GameRefereeType => typeof(WallToWallGameReferee);
        public override void Show(GameReferee gameReferee)
        {
            if (gameReferee is WallToWallGameReferee wallToWallGameReferee)
            {
                gameObject.SetActive(true);
                
                rusScore.text = wallToWallGameReferee.GetScoreForPlayer().ToString();
                lizScore.text = wallToWallGameReferee.GetScoreForEnemies().ToString();
                
                wallToWallGameReferee.ScoreChanged += OnScoreChanged;
            }
        }

        private void OnScoreChanged(BasePlayer player, int cur)
        {
            if (player == Player.LocalPlayer)
            {
                rusScore.text = $"{cur}";
            }
            else
            {
                lizScore.text = $"{cur}";
            }
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}