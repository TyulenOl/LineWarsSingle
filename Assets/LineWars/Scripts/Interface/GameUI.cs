using LineWars.Model;
using TMPro;
using UnityEngine;

namespace LineWars.Interface
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text currentMoneyText;

        private void Start()
        {
            Player.LocalPlayer.CurrentMoneyChanged += (PlayerOnCurrenMoneyChanged);
            PlayerOnCurrenMoneyChanged(0, Player.LocalPlayer.CurrentMoney);
        }

        private void OnDestroy()
        {
            Player.LocalPlayer.CurrentMoneyChanged -= (PlayerOnCurrenMoneyChanged);
        }

        void PlayerOnCurrenMoneyChanged(int before, int after)
        {
            currentMoneyText.text = $" Kоличество денег: {after}";
        }
    }
}