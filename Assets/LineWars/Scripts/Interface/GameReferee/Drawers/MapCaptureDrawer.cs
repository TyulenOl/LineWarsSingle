using System;
using LineWars.Model;
using TMPro;
using UnityEngine;

namespace LineWars.Interface
{
    public class MapCaptureDrawer: ConcreteGameRefereeDrawer
    {
        [SerializeField] private TMP_Text rusScore;
        [SerializeField] private TMP_Text lizScore;
        
        public override Type GameRefereeType => typeof(MapCaptureScoreReferee);
        public override void Show(GameReferee gameReferee)
        {
            if (gameReferee is MapCaptureScoreReferee mapCaptureScoreReferee)
            {
                gameObject.SetActive(true);
                
                rusScore.text = $"{mapCaptureScoreReferee.GetScoreForPlayer()}/{mapCaptureScoreReferee.ScoreForWin}";
                lizScore.text = $"{mapCaptureScoreReferee.GetScoreForEnemies()}/{mapCaptureScoreReferee.ScoreForWin}";
                
                mapCaptureScoreReferee.ScoreChanged += OnScoreChanged;
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