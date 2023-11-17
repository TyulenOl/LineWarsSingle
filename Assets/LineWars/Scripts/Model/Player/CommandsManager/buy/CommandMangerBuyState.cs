using LineWars.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars.Controllers
{
    public partial class CommandsManager
    {
        private class CommandsManagerBuyState : CommandsManagerState
        {
            private Node currentNode;
            private UnitBuyPreset currentBuyPreset;
            public CommandsManagerBuyState(CommandsManager manager) : base(manager)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                Manager.state = CommandsManagerStateType.Buy;
                Selector.ManySelectedObjectsChanged += OnSelectedObjectsChanged;
                Manager.BuyEntered?.Invoke();
            }

            public override void OnExit()
            {
                base.OnExit();
                Selector.ManySelectedObjectsChanged -= OnSelectedObjectsChanged;
            }

            private void OnSelectedObjectsChanged(IEnumerable<GameObject> _, IEnumerable<GameObject> gameObjects)
            {
                var node = gameObjects.OfType<Node>().FirstOrDefault();
                if (node == null || currentNode == node)
                {
                    currentNode = null;
                    return;
                }
                currentNode = node;
                CheckForCompleteness();
            }

            public void SetUnitPreset(UnitBuyPreset newPreset)
            {
                if(newPreset == null
                    && newPreset == null)
                {
                    currentBuyPreset = null;
                    return;
                }
                currentBuyPreset = newPreset;
                CheckForCompleteness();
            }

            private void CheckForCompleteness()
            {
                if(currentNode != null
                    && currentBuyPreset != null
                    && Manager.Player.CanBuyPreset(currentBuyPreset, currentNode))
                {
                    var newCommand = new BuyPresetOnNodeCommand(Manager.Player, currentNode, currentBuyPreset);
                    UnitsController.ExecuteCommand(newCommand);
                    currentBuyPreset = null;
                }
            }

        }
    }
}
