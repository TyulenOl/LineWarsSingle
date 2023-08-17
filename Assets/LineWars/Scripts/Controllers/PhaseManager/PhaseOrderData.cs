using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace LineWars.Controllers
{
    [CreateAssetMenu(fileName = "New Phase Order Data", menuName = "Phase Order Data")]
    public class PhaseOrderData : ScriptableObject
    {
        [SerializeField] private List<PhaseType> order;

        public IReadOnlyList<PhaseType> Order => order;
    }
}
