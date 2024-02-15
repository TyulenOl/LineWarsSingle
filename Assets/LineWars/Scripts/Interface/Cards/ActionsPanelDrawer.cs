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
        
        private static readonly List<EffectType> drawableEffect = new ()
            {
                EffectType.AuraPowerBuff,
                EffectType.Armored,
                EffectType.FightingSpirit,
                EffectType.Loneliness
            };

        [SerializeField] private ActionOrEffectInfoDrawer actionInfoDrawerPrefab;
        [SerializeField] private LayoutGroup actionsLayoutGroup;

        private readonly Dictionary<CommandType, ActionOrEffectInfoDrawer> commandToDrawer = new();
        private readonly Dictionary<EffectType, ActionOrEffectInfoDrawer> effectToDrawer = new();

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

            foreach (var effect in Enum.GetValues(typeof(EffectType))
                         .OfType<EffectType>()
                         .Intersect(drawableEffect)
                         .OrderBy(x => x))
            {
                var drawInfo = DrawHelper.GetReDrawInfoByEffectType(effect);
                if (drawInfo == null)
                    continue;
                
                var instance = Instantiate(actionInfoDrawerPrefab, actionsLayoutGroup.transform);
                instance.ReDraw(drawInfo);
                instance.gameObject.SetActive(false);
                effectToDrawer[effect] = instance;
            }
        }

        public void ReDrawActions(DeckCard deckCard)
        {
            if (!initialized)
                Initialize();
            
            var commandTypes = deckCard.Unit.UnitCommands
                .Except(nonDrawableCommandTypes)
                .ToArray();
            var effectTypes = deckCard.Unit.InitialEffects
                .Intersect(drawableEffect)
                .ToArray();

            foreach (var value in commandToDrawer.Values)
                value.gameObject.SetActive(false);
            foreach (var value in effectToDrawer.Values)
                value.gameObject.SetActive(false);
            

            foreach (var type in commandTypes)
            {
                if (commandToDrawer.TryGetValue(type, out var drawer))
                    drawer.gameObject.SetActive(true);
            }
            
            foreach (var type in effectTypes)
            {
                if (effectToDrawer.TryGetValue(type, out var drawer))
                    drawer.gameObject.SetActive(true);
            }
        }
    }
}
