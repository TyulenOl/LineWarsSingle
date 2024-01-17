using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class ActionsPanelDrawer : MonoBehaviour
    {
        private static readonly List<CommandType> nonDrawableCommandTypes = new () 
            {CommandType.Move};

        [SerializeField] private ActionInfoDrawer actionInfoDrawerPrefab;
        [SerializeField] private LayoutGroup actionsLayoutGroup;

        public void ReDrawActions(DeckCard deckCard)
        {
            foreach (var drawer in actionsLayoutGroup.GetComponentsInChildren<ActionInfoDrawer>())
            {
                Destroy(drawer.gameObject);
            }
            
            var types = GetAllActionDrawInfos(deckCard);
            foreach (var drawInfo in types)
            {
                var instance = Instantiate(actionInfoDrawerPrefab, actionsLayoutGroup.transform);
                instance.ReDraw(drawInfo);
            }
        }

        private IEnumerable<ActionReDrawInfo> GetAllActionDrawInfos(DeckCard deckCard)
        {
            return deckCard.Unit.UnitCommands.Except(nonDrawableCommandTypes).Select(DrawHelper.GetReDrawInfoByCommandType);
        }
    }
}
