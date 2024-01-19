using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LineWars
{
    public class WinLoseMoneyDrawer : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyAmount;
        [SerializeField] private TMP_Text diamondsAmount;

        private void Awake()
        {
            diamondsAmount.text = SingleGameRoot.Instance.WinOrLoseAction.DiamondsAfterBattle.ToString();
            moneyAmount.text = SingleGameRoot.Instance.WinOrLoseAction.MoneyAfterBattle.ToString();
        }
    }
}
