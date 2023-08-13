using System;
using UnityEngine;

namespace LineWars.Model
{
    [RequireComponent(typeof(Node))]
    public class UnitSpawner: MonoBehaviour
    {
        private Node node;

        private void Awake()
        {
            node = GetComponent<Node>();
        }

        public bool CanSpawnUnit(Unit unit)
        {
            return unit.Size == UnitSize.Large && node.LeftIsFree && node.RightIsFree
                   || unit.Size == UnitSize.Little && (node.LeftIsFree || node.RightIsFree);
        }

        public void SpawnUnit(Unit unitPrefab)
        {
            var unit = Instantiate(unitPrefab);
        }
    }
}