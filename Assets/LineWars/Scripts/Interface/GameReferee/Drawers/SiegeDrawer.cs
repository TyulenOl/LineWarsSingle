using System;
using UnityEngine;

namespace LineWars.Interface
{
    public class SiegeDrawer: ConcreteGameRefereeDrawer
    {
        [SerializeField] private TextAnimator roundText;
        public override Type GameRefereeType => typeof(SiegeGameReferee);
        public override void Show(GameReferee gameReferee)
        {
            if (gameReferee is SiegeGameReferee siegeGameReferee)
            {
                gameObject.SetActive(true);
                roundText.SetTextImmediate(siegeGameReferee.RoundsToWin.ToString());
                siegeGameReferee.CurrentRoundsChanged += OnRoundChanged;
            }
        }

        private void OnRoundChanged(int cur, int max)
        {
            roundText.SetText($"{max - cur}");
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}