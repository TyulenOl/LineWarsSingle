using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Interface
{
    public class CurrentUnitDrawer : MonoBehaviour
    {
        private void OnEnable()
        {
            CommandsManager.Instance.ExecutorChanged += ExecutorChanged;
        }

        private void OnDisable()
        {
            CommandsManager.Instance.ExecutorChanged -= ExecutorChanged;
        }

        private void ExecutorChanged(IExecutor oldExecutor, IExecutor newExecutor)
        {
            if (newExecutor is Unit unit)
            {
                unit.GetComponent<UnitDrawer>().SetUnitAsExecutor(true);
            }

            if (oldExecutor is Unit oldUnit)
            {
                if (oldUnit != null) //TODO: Это заплатка дебильная, надо разобраться
                    oldUnit.GetComponent<UnitDrawer>().SetUnitAsExecutor(false);
            }
        }
    }
}