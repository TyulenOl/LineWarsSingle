using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Interface;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Interface
{
    public class CurrentUnitPanelDrawer : MonoBehaviour
    {
        [SerializeField] private UnitInfoDrawer unitDrawer;
        [SerializeField] private AllUnitsScroll allUnitsScroll;

        private void Start()
        {
            CommandsManager.Instance.ExecutorChanged += OnExecutorChanged;
            allUnitsScroll.Init();
        }

        private void OnExecutorChanged(IMonoExecutor arg1, IMonoExecutor arg2)
        {
            var obj = arg2 as MonoBehaviour;
            if (obj != null)
            {
                unitDrawer.gameObject.SetActive(true);
                ReDraw(obj.GetComponent<Unit>());
            }
            else
                unitDrawer.gameObject.SetActive(false);
        }

        public void ReDraw(Unit unit)
        {
            unitDrawer.Init(unit);
        }
    }
}
