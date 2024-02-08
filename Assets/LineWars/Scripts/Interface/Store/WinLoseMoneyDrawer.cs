using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using TMPro;
using UnityEngine;

namespace LineWars.Interface
{
    public class WinLoseMoneyDrawer : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyAmount;
        [SerializeField] private TMP_Text diamondsAmount;

        private void Awake()
        {
            diamondsAmount.text = "+ " + WinOrLoseScene.DiamondsAmount;
            moneyAmount.text = "+ " + WinOrLoseScene.MoneyAmount;
        }
    }
}
