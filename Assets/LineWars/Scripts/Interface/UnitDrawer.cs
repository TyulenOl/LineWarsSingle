﻿
using System;
using LineWars.Interface;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    [RequireComponent(typeof(Unit))]
    public class UnitDrawer : MonoBehaviour
    {
        [Header("Animate Settings")]
        [SerializeField] private Vector2 offset;
        [SerializeField] private Color damageColor = Color.red;
        [SerializeField] private Color armorDamageColor = Color.blue;
        [SerializeField] private Color healColor = Color.green;
        
        [Header("Reference")]
        [SerializeField] private UnitPartDrawer leftPart;
        [SerializeField] private UnitPartDrawer centerPart;
        [SerializeField] private UnitPartDrawer rightPart;
        
        [Header("CharacteristicsDrawers")]
        [SerializeField] private UnitPartCharacteristicDrawer leftCharacteristicDrawer;
        [SerializeField] private UnitPartCharacteristicDrawer rightCharacteristicDrawer;
        [SerializeField] private UnitPartCharacteristicDrawer centerCharacteristicDrawer;
        
        private Unit unit;
        
        private void Awake()
        {
            unit = GetComponent<Unit>();

            unit.ActionPointChanged.AddListener(OnActionPointsChanged);
            
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

            leftCharacteristicDrawer.CurrentUnit = unit;
            rightCharacteristicDrawer.CurrentUnit = unit;
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
                leftPart.gameObject.SetActive(true);
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
                centerPart.gameObject.SetActive(true);
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
                rightPart.gameObject.SetActive(true);
        }

        private void ReDrawCharacteristics()
        {
            if(leftCharacteristicDrawer != null)
            {
                leftCharacteristicDrawer.ReDrawCharacteristics();
            }
            if(rightCharacteristicDrawer != null)
            {
                rightCharacteristicDrawer.ReDrawCharacteristics();
            }
            if(centerCharacteristicDrawer != null)
            {
                centerCharacteristicDrawer.ReDrawCharacteristics();
            }
        }
            
        private void OnActionPointsChanged(int oldValue, int newValue)
        {
            var isActive = newValue != 0;
            
            if(leftCharacteristicDrawer != null)
            {
                leftCharacteristicDrawer.ReDrawActivity(isActive);
            }
            if(rightCharacteristicDrawer != null)
            {
                rightCharacteristicDrawer.ReDrawActivity(isActive);
            }
            if(centerCharacteristicDrawer != null)
            {
                centerCharacteristicDrawer.ReDrawActivity(isActive);
            }
        }
    }
}