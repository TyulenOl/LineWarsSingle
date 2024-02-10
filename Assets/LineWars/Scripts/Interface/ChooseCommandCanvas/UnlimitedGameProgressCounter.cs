using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using TMPro;
using UnityEngine;

public class UnlimitedGameProgressCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text amountText;

    private void OnEnable()
    {
        if(GameRoot.Instance != null)
            amountText.text = GameRoot.Instance.UserController.PassingInfinityGameModes.ToString();
    }
}
