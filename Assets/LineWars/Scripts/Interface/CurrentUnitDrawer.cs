using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;

public class CurrentUnitDrawer : MonoBehaviour
{
    private void Awake()
    {
        CommandsManager.Instance.ExecutorChanged.AddListener(ExecutorChanged);
    }

    private void ExecutorChanged(IExecutor oldExecutor, IExecutor newExecutor)
    {
        if (newExecutor is Unit unit)
        {
            unit.GetComponent<UnitDrawer>().SetUnitAsExecutor(true);
        }
        if (oldExecutor is Unit oldUnit)
        {
            oldUnit.GetComponent<UnitDrawer>().SetUnitAsExecutor(false);
        }
    }
}
