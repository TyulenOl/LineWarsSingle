using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class BlessingsDrawer: MonoBehaviour
    {
        [FormerlySerializedAs("blessingUIElementPrefab")] [SerializeField] private BlessingDrawer blessingDrawerPrefab;
        [SerializeField] private LayoutGroup layoutGroup;

        private Dictionary<BlessingId, BlessingDrawer> blessingToUI;
        private CommandsManager CommandsManager => CommandsManager.Instance;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            blessingToUI = new Dictionary<BlessingId, BlessingDrawer>();
            var allBlessings = CommandsManager.BlessingData.ToArray();
            foreach (var (blessingData, count) in allBlessings)
            {
                var elementInstance = Instantiate(blessingDrawerPrefab, layoutGroup.transform);
                elementInstance.BlessingData = blessingData;
                elementInstance.BlessingCount = count;
                blessingToUI[blessingData] = elementInstance;
                elementInstance.OnClick += ElementOnClick;
            }
            
            CommandsManager.BlessingCountChanged += CommandsManagerOnBlessingCountChanged;
            CommandsManager.BlessingStarted += CommandsManagerOnBlessingStarted;
            
            CommandsManager.StateEntered += CommandsManagerOnStateEntered;
        }

        private void ElementOnClick(BlessingId data)
        {
            CommandsManager.ExecuteBlessing(data);
        }

        private void CommandsManagerOnStateEntered(CommandsManagerStateType state)
        {
            Redraw();
        }

        private void CommandsManagerOnBlessingStarted(BlessingMassage massage)
        {
            if (blessingToUI.TryGetValue(massage.Data, out var ui))
            {
                ui.State = BlessingDrawerState.Active;
            }
        }

        private void Redraw()
        {
            foreach (var (blessingData, uiElement) in blessingToUI)
            {
                if (CommandsManager.CanExecuteBlessing(blessingData))
                    uiElement.State = BlessingDrawerState.Unlocked;
                else
                    uiElement.State = BlessingDrawerState.Locked;
            }
        }

        private void CommandsManagerOnBlessingCountChanged(BlessingId data, int count)
        {
            if (blessingToUI.TryGetValue(data, out var ui))
                ui.BlessingCount = count;
        }

        private void OnDestroy()
        {
            if (CommandsManager != null)
            {
                CommandsManager.BlessingCountChanged -= CommandsManagerOnBlessingCountChanged;
                CommandsManager.BlessingStarted -= CommandsManagerOnBlessingStarted;
                CommandsManager.StateEntered -= CommandsManagerOnStateEntered;
            }
        }
    }
}