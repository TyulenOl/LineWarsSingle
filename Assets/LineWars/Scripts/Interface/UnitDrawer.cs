using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Interface;
using LineWars.Model;
using UnityEditor.U2D.Path;
using UnityEngine;

namespace LineWars.Controllers
{
    [RequireComponent(typeof(Unit), typeof(TargetDrawer))]
    public class UnitDrawer : MonoBehaviour
    {
        [Header("Animate Settings")]
        [SerializeField] private Vector2 offset;
        [SerializeField] private Color damageColor = Color.red;
        [SerializeField] private Color armorDamageColor = Color.blue;
        [SerializeField] private Color healColor = Color.green;
        
        [Header("Reference")]
        [SerializeField] private Interface.UnitPartDrawer leftPart;
        [SerializeField] private Interface.UnitPartDrawer centerPart;
        [SerializeField] private Interface.UnitPartDrawer rightPart;
        
        [Header("CharacteristicsDrawers")]
        [SerializeField] private UnitPartDrawer leftDrawer;
        [SerializeField] private UnitPartDrawer centerDrawer;
        [SerializeField] private UnitPartDrawer rightDrawer;

        private Unit unit;
        private List<UnitPartDrawer> allDrawers;
        private TargetDrawer targetDrawer;

        private void Awake()
        {
            unit = GetComponent<Unit>();

            unit.ActionPointsChanged.AddListener((_,newValue) => ExecuteForAllDrawers(drawer => drawer.ReDrawActivity(newValue != 0)));
            unit.CanBlockChanged.AddListener((_,newBool) => ExecuteForAllDrawers(drawer => drawer.ReDrawCanBlock(newBool)));
            
            if (leftPart != null)
            {
                leftPart.offset = offset;
            }
            
            if (centerPart != null)
            {
                centerPart.offset = offset;
            }
            
            if (rightPart != null)
            {
                rightPart.offset = offset;
            }

            allDrawers = new List<UnitPartDrawer>
            { leftDrawer, rightDrawer, centerDrawer }
                .Where(x => x is not null)
                .ToList();
            
            if(leftDrawer != null)
                leftDrawer.CurrentUnit = unit;
            if(rightDrawer != null)
                rightDrawer.CurrentUnit = unit;
            if (centerDrawer != null)
                centerDrawer.CurrentUnit = unit;

            targetDrawer = GetComponent<TargetDrawer>();
            //TODO центральный отрисовщик характеристик
        }

        private void OnEnable()
        {
            unit.UnitDirectionChange.AddListener(OnUnitDirectionChange);
            unit.ArmorChanged.AddListener(OnUnitArmorChange);
            unit.HpChanged.AddListener(OnUnitHpChange);
            ReDrawCharacteristics();
        }

        private void OnDisable()
        {
            unit.UnitDirectionChange.RemoveListener(OnUnitDirectionChange);
            unit.ArmorChanged.RemoveListener(OnUnitArmorChange);
            unit.HpChanged.RemoveListener(OnUnitHpChange);
        }

        private void OnUnitDirectionChange(UnitSize size, UnitDirection direction)
        {
            if (size == UnitSize.Little && direction == UnitDirection.Left)
                DrawLeft();
            else if (size == UnitSize.Little && direction == UnitDirection.Right)
                DrawRight();
            else
                DrawCenter();
        }

        private void OnUnitArmorChange(int before, int after)
        {
            if (leftPart != null && leftPart.gameObject.activeSelf)
                leftPart.AnimateDamageText((after - before).ToString(), armorDamageColor);
            
            if (centerPart != null && centerPart.gameObject.activeSelf)
                centerPart.AnimateDamageText((after - before).ToString(), armorDamageColor);
            
            if (rightPart != null && rightPart.gameObject.activeSelf)
                rightPart.AnimateDamageText((after - before).ToString(), armorDamageColor);
            ReDrawCharacteristics();
        }
        
        private void OnUnitHpChange(int before, int after)
        {
            var diff = after - before;
            var color = diff > 0 ? healColor : damageColor; 
            
            if (leftPart != null && leftPart.gameObject.activeSelf)
                leftPart.AnimateDamageText((diff).ToString(), color);
            
            if (centerPart != null && centerPart.gameObject.activeSelf)
                centerPart.AnimateDamageText((diff).ToString(), color);
            
            if (rightPart != null && rightPart.gameObject.activeSelf)
                rightPart.AnimateDamageText((diff).ToString(), color);
            
            ReDrawCharacteristics();
        }
        
        
        public void DrawLeft()
        {
            
            if (leftPart != null)
            {
                leftPart.gameObject.SetActive(true);
                targetDrawer.image = leftDrawer.targetSprite;
            }
            if (centerPart != null)
                centerPart.gameObject.SetActive(false);
            if (rightPart != null)
                rightPart.gameObject.SetActive(false);
        }

        public void DrawCenter()
        {
            if (leftPart != null)
                leftPart.gameObject.SetActive(false);
            if (centerPart != null)
            {
                centerPart.gameObject.SetActive(true);
                targetDrawer.image = centerDrawer.targetSprite;
            }
            if (rightPart != null)
                rightPart.gameObject.SetActive(false);
        }

        public void DrawRight()
        {
            if (leftPart != null)
                leftPart.gameObject.SetActive(false);
            if (centerPart != null)
                centerPart.gameObject.SetActive(false);
            if (rightPart != null)
            {
                rightPart.gameObject.SetActive(true);
                targetDrawer.image = rightDrawer.targetSprite;
            }
        }

        private void ReDrawCharacteristics()
        {
            foreach (var drawer in allDrawers)
            {
                drawer.ReDrawCharacteristics();
            }
        }

        public void SetUnitAsExecutor(bool isExecutor)
        {
            foreach (var drawer in allDrawers)
            {
                drawer.SetUnitAsExecutor(isExecutor);
            }
        }

        private void ExecuteForAllDrawers(Action<UnitPartDrawer> action)
        {
            foreach (var drawer in allDrawers)
            {
                action.Invoke(drawer);
            }
        }
    }
}