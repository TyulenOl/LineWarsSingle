﻿using System;
using LineWars.Model;
using TMPro;
using UnityEngine;

namespace LineWars.Interface
{
    public class CaptureThePointsDrawer : ConcreteGameRefereeDrawer
    {
        [SerializeField] private TMP_Text rusScore;
        [SerializeField] private TMP_Text lizScore;
        public override Type GameRefereeType => typeof(CaptureThePointsGameReferee);

        public override void Show(GameReferee gameReferee)
        {
            if (gameReferee is CaptureThePointsGameReferee captureThePointsGameReferee)
            {
                gameObject.SetActive(true);
                
                rusScore.text = $"{captureThePointsGameReferee.GetScoreForPlayer()}/{captureThePointsGameReferee.ScoreForWin}";
                lizScore.text = $"{captureThePointsGameReferee.GetScoreForEnemies()}/{captureThePointsGameReferee.ScoreForWin}";
                captureThePointsGameReferee.ScoreChanged += OnScoreChanged;
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