using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace LineWars
{
    [CreateAssetMenu(fileName = "New Phase Order Data", menuName = "Phase Order Data")]
    public class PhaseOrderData : ScriptableObject, IReadOnlyList<PhaseType>
    {
        [SerializeField] private List<PhaseType> order;

        public IReadOnlyList<PhaseType> Order => order;

        public PhaseType this[int index] => ((IReadOnlyList<PhaseType>)order)[index];

        public int Count => ((IReadOnlyCollection<PhaseType>)order).Count;

        public IEnumerator<PhaseType> GetEnumerator()
        {
            return ((IEnumerable<PhaseType>)order).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)order).GetEnumerator();
        }
    }
}
