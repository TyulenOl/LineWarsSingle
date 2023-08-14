
using System;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    [RequireComponent(typeof(Unit))]
    public class UnitBackgroundDrawer : MonoBehaviour
    {
        [SerializeField] private GameObject leftPart;
        [SerializeField] private GameObject centerPart;
        [SerializeField] private GameObject rightPart;
        
        private Unit unit;
        
        private void Awake()
        {
            unit = GetComponent<Unit>();
        }

        private void OnEnable()
        {
            unit.UnitDirectionChange.AddListener(OnUnitDirectionChance);
        }

        private void OnDisable()
        {
            unit.UnitDirectionChange.RemoveListener(OnUnitDirectionChance);
        }

        private void OnUnitDirectionChance(UnitSize size, UnitDirection direction)
        {
            if (size == UnitSize.Little && direction == UnitDirection.Left)
                DrawLeft();
            else if (size == UnitSize.Little && direction == UnitDirection.Right)
                DrawRight();
            else
                DrawCenter();
        }
        
        public void DrawLeft()
        {
            if (leftPart != null)
                leftPart.SetActive(true);
            if (centerPart != null)
                centerPart.SetActive(false);
            if (rightPart != null)
                rightPart.SetActive(false);
        }

        public void DrawCenter()
        {
            if (leftPart != null)
                leftPart.SetActive(false);
            if (centerPart != null)
                centerPart.SetActive(true);
            if (rightPart != null)
                rightPart.SetActive(false);
        }

        public void DrawRight()
        {
            if (leftPart != null)
                leftPart.SetActive(false);
            if (centerPart != null)
                centerPart.SetActive(false);
            if (rightPart != null)
                rightPart.SetActive(true);
        }
    }
}