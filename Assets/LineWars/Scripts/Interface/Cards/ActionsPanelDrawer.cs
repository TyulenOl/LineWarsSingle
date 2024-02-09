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

        private Dictionary<CommandType, ActionInfoDrawer> commandToDrawer = new();

        private bool initialized;

        private void Initialize()
        {
            initialized = true;
            
            foreach (var command in Enum.GetValues(typeof(CommandType))
                         .OfType<CommandType>()
                         .Except(nonDrawableCommandTypes)
                         .OrderBy(x => x))
            {
                var drawInfo = DrawHelper.GetReDrawInfoByCommandType(command);
                if (drawInfo == null)
                    continue;
                
                var instance = Instantiate(actionInfoDrawerPrefab, actionsLayoutGroup.transform);
                instance.ReDraw(drawInfo);
                instance.gameObject.SetActive(false);
                commandToDrawer[command] = instance;
            }
        }

        public void ReDrawActions(DeckCard deckCard)
        {
            if (!initialized)
                Initialize();
            
            var types = deckCard.Unit.UnitCommands
                .Except(nonDrawableCommandTypes)
                .ToArray();

            foreach (var value in commandToDrawer.Values)
                value.gameObject.SetActive(false);

            foreach (var type in types)
            {
                if (commandToDrawer.TryGetValue(type, out var drawer))
                    drawer.gameObject.SetActive(true);
            }
        }
    }
}
