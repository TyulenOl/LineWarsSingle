using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Controllers;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class BlessingsDrawer: MonoBehaviour
    {
        [SerializeField] private TMP_Text totalBlessingsCount;
        [SerializeField] private BlessingFightDrawer blessingUIElementPrefab;
        [SerializeField] private LayoutGroup layoutGroup;

        private Dictionary<BlessingId, BlessingFightDrawer> blessingToUI;
        private CommandsManager CommandsManager => CommandsManager.Instance;
        private IBlessingsPull GlobalBlessingsPull => GameRoot.Instance?.UserController;
        private IBlessingsPull LocalBlessingsPull => SingleGameRoot.Instance?.LocalBlessingPull;

        private bool isOpened;
        
        private void Start()
        {
            Initialize();
        }

        public void SetOpen()
        {
            isOpened = !isOpened;
            gameObject.SetActive(isOpened);
        }
        
        public void Initialize()
        {
            blessingToUI = new Dictionary<BlessingId, BlessingFightDrawer>();
            var allBlessings = LocalBlessingsPull.ToArray();
            foreach (var (id, count) in allBlessings)
            {
                var elementInstance = Instantiate(blessingUIElementPrefab, layoutGroup.transform);
                elementInstance.BlessingData = id;
                elementInstance.BlessingCount = GlobalBlessingsPull?[id] ?? -1;
                blessingToUI[id] = elementInstance;
                elementInstance.OnClick += ElementOnClick;
            }
            
            CommandsManager.BlessingStarted += CommandsManagerOnBlessingStarted;
            CommandsManager.StateEntered += CommandsManagerOnStateEntered;

            if (GlobalBlessingsPull != null)
            {
                GlobalBlessingsPull.BlessingCountChanged += GlobalBlessingsPullOnBlessingCountChanged;
            }

            if (LocalBlessingsPull is LimitingBlessingPool limitingBlessingPool)
            {
                limitingBlessingPool.CurrentTotalCountChanged += LimitingBlessingPoolOnTotalCountChanged;
                totalBlessingsCount.text = $"{limitingBlessingPool.CurrentTotalCount}/{limitingBlessingPool.TotalCount}";
            }
            else
            {
                totalBlessingsCount.text = "-";
            }
        }
        
        private void OnDestroy()
        {
            if (CommandsManager != null)
            {
                CommandsManager.BlessingStarted -= CommandsManagerOnBlessingStarted;
                CommandsManager.StateEntered -= CommandsManagerOnStateEntered;
            }

            if (LocalBlessingsPull is LimitingBlessingPool limitingBlessingPool)
            {
                limitingBlessingPool.CurrentTotalCountChanged -= LimitingBlessingPoolOnTotalCountChanged;
            }
            
            if (GlobalBlessingsPull != null)
            {
                GlobalBlessingsPull.BlessingCountChanged -= GlobalBlessingsPullOnBlessingCountChanged;
            }
        }

        private void LimitingBlessingPoolOnTotalCountChanged(int current, int total)
        {
            totalBlessingsCount.text = $"{current}/{total}";
        }

        private void ElementOnClick(BlessingId data)
        {
            CommandsManager.ExecuteBlessing(data);
        }

        private void CommandsManagerOnStateEntered(CommandsManagerStateType state)
        {
            Redraw();
        }
        
        private void GlobalBlessingsPullOnBlessingCountChanged(BlessingId id, int count)
        {
            if (blessingToUI.TryGetValue(id, out var ui))
                ui.BlessingCount = count;
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
    }
}