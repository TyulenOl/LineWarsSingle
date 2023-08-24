using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Commannds Priority Data", menuName = "Commands Priority Data")]
    public class CommandPriorityData : ScriptableObject
    {
        [SerializeField] private List<CommandType> commandsPriority;

        public IReadOnlyList<CommandType> Priority => commandsPriority;
    }

    public enum CommandType
    {
        None,
        Attack,
        Heal,
        Explosion,
        Fire,
        Move,
        Build
    }
}   

