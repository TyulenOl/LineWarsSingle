using System;
using System.Collections;
using System.Collections.Generic;
using LineWars;
using LineWars.Controllers;
using TMPro;
using UnityEngine;

public class RoundCounter : MonoBehaviour
{
    [SerializeField] private int counter;
    [SerializeField] private TMP_Text text;

    public int Counter
    {
        get => counter;
        set
        {
            counter = value;
            text.text = counter.ToString();
        }
    }

    private void Awake()
    {
        SingleGameRoot.Instance.CommandsManager.StateEntered += OnStateChanged;
    }

    private void OnStateChanged(CommandsManagerStateType obj)
    {
        if (obj == CommandsManagerStateType.Buy)
        {
            Counter++;
        }
    }
}
