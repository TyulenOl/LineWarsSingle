using System;
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
            private DeckCard deckCard;
            public CommandsManagerBuyState(CommandsManager manager) : base(manager)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                Manager.State = CommandsManagerStateType.Buy;
                Selector.ManySelectedObjectsChanged += OnSelectedObjectsChanged;
                Manager.SendBuyReDrawMessage(GetNodes());
                currentNode = null;
                deckCard = null;
            }

            private IEnumerable<Node> GetNodes()
            {
                return MonoGraph.Instance.Nodes.Where(node => node.IsQualifiedForSpawn(Manager.Player));
            }
            
            public override void OnExit()
            {
                base.OnExit();
                Selector.ManySelectedObjectsChanged -= OnSelectedObjectsChanged;
                currentNode = null;
                deckCard = null;
            }

            private void OnSelectedObjectsChanged(IEnumerable<GameObject> _, IEnumerable<GameObject> gameObjects)
            {
                if (!Manager.ActiveSelf)
                    return;
                
                if(deckCard == null)
                    return;
                var node = gameObjects.GetComponentMany<Node>().FirstOrDefault();
                if (Manager.HaveConstrains && !Manager.Constrains.CanSelectNode(node))
                    return;
                if (node == null || currentNode == node)
                {
                    currentNode = null;
                    return;
                }
                if (node.IsQualifiedForSpawn(Player.LocalPlayer))
                    currentNode = node;
                CheckForCompleteness();
            }


            public void SetDeckCard(DeckCard card)
            {
                if(card == null)
                {
                    deckCard = null;
                    return;
                }
                deckCard = card;
                //CheckForCompleteness();
            }

            private void CheckForCompleteness()
            {
                if(currentNode != null
                    && deckCard != null
                    && Manager.Player.CanBuyDeckCard(currentNode, deckCard))
                {
                    var newCommand = new BuyDeckCardOnNodeCommand(Manager.Player, currentNode, deckCard);
                    UnitsController.ExecuteCommand(newCommand);
                    Manager.InvokeAction(() => Manager.CommandIsExecuted?.Invoke(newCommand));
                    currentNode = null;
                    Manager.SendBuyReDrawMessage(GetNodes());
                }
            }
        }
    }
}
