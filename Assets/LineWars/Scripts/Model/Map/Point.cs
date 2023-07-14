using System.Collections.Generic;
using LineWars.Extensions.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LineWars.Model
{
    public class Point : MonoBehaviour,
        INode,
        IHitHandler,
        IHitCreator,
        IPointerClickHandler
    {
        [SerializeField] private Transform leftPart;
        [SerializeField] private Transform central;
        [SerializeField] private Transform rightPart;

        [SerializeField, ReadOnlyInspector] private Unit leftUnit;
        [SerializeField, ReadOnlyInspector] private Unit rightUnit;

        [SerializeField, ReadOnlyInspector] private Player owner;

        private Node node;
        private Unit selectedUnit;
        private Camera mainCamera;

        public Vector2 Position => node.Position;
        public IReadOnlyCollection<IEdge> Edges => node.Edges;

        public Unit LeftUnit => leftUnit;
        public Unit RightUnit => rightUnit;
        public Player Owner => owner;

        public Transform LeftPart => leftPart;
        public Transform Central => central;
        public Transform RightPart => rightPart;

        private void Awake()
        {
            node = GetComponent<Node>();
            mainCamera = Camera.main;
        }

        public void Accept(Hit hit)
        {
            throw new System.NotImplementedException();
        }

        public Hit GenerateHit()
        {
            throw new System.NotImplementedException();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var mousePos = mainCamera.ScreenToWorldPoint(eventData.position);
            var relativeMousePos = mousePos - transform.position;

            if (relativeMousePos.x < 0)
                selectedUnit = leftUnit;
            else
                selectedUnit = rightUnit;
        }

        public void AddUnitToVacantPosition(Unit unit)
        {
            if (unit.Size == UnitSize.Lage
                && leftUnit == null
                && rightUnit == null)
            {
                leftUnit = rightUnit = unit;
            }
            else if (leftUnit == null)
            {
                leftUnit = unit;
            }
            else if (rightUnit == null)
            {
                rightUnit = unit;
            }
        }
    }
}