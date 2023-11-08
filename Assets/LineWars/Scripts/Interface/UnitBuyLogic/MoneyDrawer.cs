using System;
using System.Collections;
using System.Collections.Generic;
using LineWars;
using TMPro;
using UnityEngine;

public class MoneyDrawer : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyAmountText;

    private const string RedColorHex = "#E22B12";

    private void Start()
    {
        Player.LocalPlayer.CurrentMoneyChanged += CurrentMoneyChanged;
        ReDrawText();
    }

    private void CurrentMoneyChanged(int arg1, int arg2)
    {
        ReDrawText();
    }

    private void ReDrawText()
    {
        moneyAmountText.text =
            $"{Player.LocalPlayer.CurrentMoney} (<color={RedColorHex}>+{Player.LocalPlayer.Income.ToString()}</color>)";
    }
}