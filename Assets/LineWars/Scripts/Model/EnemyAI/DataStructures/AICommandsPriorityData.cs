using UnityEngine;
using AYellowpaper.SerializedCollections;
using System;

namespace LineWars.Model
{
    public class AICommandsPriorityData : ScriptableObject
    {
        [SerializeField] private int defaultPriority;
        [SerializeField] private SerializedDictionary<CommandType, int> commandPriorities;

        public int GetPriority(CommandType commandType)
        {
            if (commandPriorities.ContainsKey(commandType))
                return commandPriorities[commandType]; 
            return defaultPriority;
        }

        private void OnValidate()
        {
            var allTypes = Enum.GetValues(typeof(CommandType));
            foreach (CommandType commandType in allTypes)
            {
                if (!commandPriorities.ContainsKey(commandType))
                    Debug.LogWarning($"{this} doesn't contain {commandType}");
            }
        }
    }
}
