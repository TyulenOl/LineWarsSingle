using System.Collections.Generic;
using System.Linq;
using LineWars.Extensions;
using LineWars.Extensions.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LineWars.Model
{
    public class Point : Owned,
        IHitHandler,
        IHitCreator,
        IPointerClickHandler
    {
        [SerializeField, ReadOnlyInspector] private Unit leftUnit;
        [SerializeField, ReadOnlyInspector] private Unit rightUnit;

        private Node node;
        private Unit selectedUnit;
        private Camera mainCamera;

        private Line[] lines;
        
        
        public bool LeftIsFree => LeftUnit == null;
        public bool RightIsFree => RightUnit == null;
        public IReadOnlyCollection<Line> Lines => lines;
        
        
        public Unit LeftUnit
        {
            get => leftUnit;
            set => leftUnit = value;
        }

        public Unit RightUnit
        {
            get => rightUnit;
            set => rightUnit = value;
        }
        
        private void Awake()
        {
            node = GetComponent<Node>();
            lines = node.Edges.OfType<Edge>().GetComponentMany<Line>().ToArray();
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
    }
}