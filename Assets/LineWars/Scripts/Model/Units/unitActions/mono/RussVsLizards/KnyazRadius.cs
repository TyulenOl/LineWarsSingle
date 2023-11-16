using UnityEngine;

namespace LineWars.Model
{
    [RequireComponent(typeof(Unit))]
    [DisallowMultipleComponent]
    public class KnyazRadius : MonoBehaviour
    {
        private void Start()
        {
            var unit = GetComponent<Unit>();
            if (unit.Type != UnitType.Knyaz)
                Debug.LogWarning("KnyazRadius should be on Knyaz Unit!");
        }

        [field:SerializeField] public int Radius { get; private set; }
    }
}
