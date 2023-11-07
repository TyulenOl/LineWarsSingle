using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Interface;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Interface
{
    [RequireComponent(typeof(Unit), typeof(TargetDrawer))]
    public class UnitDrawer : MonoBehaviour
    {
        [Header("Animate Settings")] [SerializeField]
        private Vector2 offset;

        [SerializeField] private Color damageColor = Color.red;
        [SerializeField] private Color armorDamageColor = Color.blue;
        [SerializeField] private Color healColor = Color.green;

        [Header("Reference")] [SerializeField] private UnitDamageAnimator leftPart;
        [SerializeField] private UnitDamageAnimator rightPart;

        [Header("CharacteristicsDrawers")] [SerializeField]
        private UnitPartDrawer leftDrawer;

        [SerializeField] private UnitPartDrawer rightDrawer;

        private Unit unit;
        private List<UnitPartDrawer> allDrawers;
        private TargetDrawer targetDrawer;

        private void Awake()
        {
            unit = GetComponent<Unit>();

            unit.ActionPointsChanged.AddListener((_, newValue) => ExecuteForAllDrawers(drawer =>
            {
                drawer.ReDrawActivity(newValue != 0);
                ReDrawCharacteristics();
            }));
            if (unit.TryGetUnitAction<MonoBlockAction>(out var action))
                action.CanBlockChanged +=
                    (_, newBool) => ExecuteForAllDrawers(drawer => drawer.ReDrawCanBlock(newBool));

            if (leftPart != null)
            {
                leftPart.offset = offset;
            }

            if (rightPart != null)
            {
                rightPart.offset = offset;
            }

            allDrawers = new List<UnitPartDrawer>
                    {leftDrawer, rightDrawer}
                .Where(x => x is not null)
                .ToList();

            if (leftDrawer != null)
                leftDrawer.CurrentUnit = unit;
            if (rightDrawer != null)
                rightDrawer.CurrentUnit = unit;

            targetDrawer = GetComponent<TargetDrawer>();
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
            else
                DrawRight();
        }

        private void OnUnitArmorChange(int before, int after)
        {
            if (leftPart != null && leftPart.gameObject.activeSelf)
                leftPart.AnimateDamageText((after - before).ToString(), armorDamageColor);

            if (rightPart != null && rightPart.gameObject.activeSelf)
                rightPart.AnimateDamageText((after - before).ToString(), armorDamageColor);
            ReDrawCharacteristics();
        }

        private void OnUnitHpChange(int before, int after)
        {
            if (before == after) return;
            var diff = after - before;
            var color = diff > 0 ? healColor : damageColor;

            if (leftPart != null && leftPart.gameObject.activeSelf)
                leftPart.AnimateDamageText((diff).ToString(), color);

            if (rightPart != null && rightPart.gameObject.activeSelf)
                rightPart.AnimateDamageText((diff).ToString(), color);

            ReDrawCharacteristics();
        }


        public void ReDrawAvailability(bool available)
        {
            ExecuteForAllDrawers((drawer => drawer.ReDrawAvailability(available)));
        }

        public void DrawLeft()
        {
            if (leftPart != null)
            {
                leftPart.gameObject.SetActive(true);
                targetDrawer.image = leftDrawer.targetSprite;
            }

            if (rightPart != null)
                rightPart.gameObject.SetActive(false);
        }

        public void DrawRight()
        {
            if (leftPart != null)
                leftPart.gameObject.SetActive(false);
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