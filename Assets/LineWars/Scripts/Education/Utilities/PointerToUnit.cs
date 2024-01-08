using System;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Model
{
    [Serializable]
    public class PointerToUnit
    {
        [SerializeField] private Node node;
        [SerializeField] private SelectedUnit leftOrRight;
        
        public Unit GetUnit()
        {
            if (leftOrRight == SelectedUnit.Left)
                return node.LeftUnit ? node.LeftUnit : throw new NullReferenceException($"In node {node.name} there is not unit on direction {leftOrRight}");
            return node.RightUnit ? node.RightUnit : throw new NullReferenceException($"In node {node.name} there is not unit on direction {leftOrRight}");
        }
    }
}