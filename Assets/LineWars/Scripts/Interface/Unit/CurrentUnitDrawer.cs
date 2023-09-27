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
            CommandsManager.Instance.ExecutorChanged.AddListener(ExecutorChanged);
        }

        private void OnDisable()
        {
            CommandsManager.Instance.ExecutorChanged.RemoveListener(ExecutorChanged);
        }

        private void ExecutorChanged(IReadOnlyExecutor oldExecutor, IReadOnlyExecutor newExecutor)
        {
            if (newExecutor is ComponentUnit unit)
            {
                unit.GetComponent<UnitDrawer>().SetUnitAsExecutor(true);
            }

            if (oldExecutor is ComponentUnit oldUnit)
            {
                if (oldUnit != null) //TODO: Это заплатка дебильная, надо разобраться
                    oldUnit.GetComponent<UnitDrawer>().SetUnitAsExecutor(false);
            }
        }
    }
}
