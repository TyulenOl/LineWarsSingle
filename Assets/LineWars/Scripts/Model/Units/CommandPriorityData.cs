using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Commannds Priority Data", menuName = "Commands Priority Data")]
    public class CommandPriorityData : ScriptableObject
    {
        [SerializeField] private List<CommandType> commandsPriority;

        public IReadOnlyList<CommandType> Priority => commandsPriority.AsReadOnly();
    }

    public enum CommandType
    {
        Attack,
        Heal,
        Move,
        Build,
        Curse
    }
}   
