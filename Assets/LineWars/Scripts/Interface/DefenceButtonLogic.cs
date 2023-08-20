using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

public class DefenceButtonLogic : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        var executor = CommandsManager.Instance.Executor;
        if(executor is Unit unit)
            UnitsController.ExecuteCommand(new EnableBlockCommand(unit),false);
    }
}
