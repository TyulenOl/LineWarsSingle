using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Commands Priority Data", menuName = "Create Commands Priority Data")]
    public class CommandPriorityData : ScriptableObject, IReadOnlyList<CommandType>
    {
        [SerializeField] private List<CommandType> commandsPriority;

        public CommandType this[int index] => commandsPriority[index];

        public int Count => commandsPriority.Count;

        public IEnumerator<CommandType> GetEnumerator()
        {
            return commandsPriority.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return commandsPriority.GetEnumerator();
        }
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
        Block,
        SacrificePerun,
        Ram,
        BlowWithSwing,
        ShotUnit
    }
}   

