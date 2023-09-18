using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Commands Priority Data", menuName = "Create Commands Priority Data")]
    public class CommandPriorityData : ScriptableObject
    {
        [SerializeField] private List<CommandType> commandsPriority;

        public IReadOnlyList<CommandType> Priority => commandsPriority;
    }

    public enum CommandType
    {
        None,
        MeleeAttack,
        Heal,
        Explosion,
        Fire,
        Move,
        Build,
        Block
    }
}   

