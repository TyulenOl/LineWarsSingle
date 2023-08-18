using LineWars.Controllers;
using LineWars.Model;
using TMPro;
using UnityEngine;

namespace LineWars.Interface
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text currentMoneyText;
        [SerializeField] private TMP_Text currentPhaseText;

        private void Start()
        {
            Player.LocalPlayer.CurrentMoneyChanged += (PlayerOnCurrenMoneyChanged);
            PhaseManager.Instance.PhaseChanged.AddListener(OnPhaseChanged);
            PlayerOnCurrenMoneyChanged(0, Player.LocalPlayer.CurrentMoney);
        }

        private void OnDestroy()
        {
            Player.LocalPlayer.CurrentMoneyChanged -= (PlayerOnCurrenMoneyChanged);
            PhaseManager.Instance.PhaseChanged.RemoveListener(OnPhaseChanged);
        }

        private void OnPhaseChanged(PhaseType previousPhase, PhaseType currentPhase)
        {
            currentPhaseText.text = currentPhase.ToString();
        }
        void PlayerOnCurrenMoneyChanged(int before, int after)
        {
            currentMoneyText.text = $" Kоличество денег: {after}";
        }
    }
}