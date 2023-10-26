using System;
using UnityEngine;

namespace LineWars.Model
{
    public abstract class Building: MonoBehaviour, IBuilding
    {
        [field: SerializeField] public int NodeId { get; private set; }
        public abstract BuildingType BuildingType { get; }

        public void Initialize(Node node)
        {
            NodeId = node.Id;
        }
    }
}