using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using TMPro;
using UnityEngine;

namespace LineWars.Interface
{
    public class UnitPowerDrawer: MonoBehaviour
    {
        [SerializeField] private Unit unit;
        [SerializeField] private GameObject canAttackSprite;
        [SerializeField] private GameObject cantAttackSprite;
        [SerializeField] private TMP_Text powerText;

        private IAttackAction[] attackActions;
        
        private void Start()
        {
            if (unit.Actions.Any(x => x is IAttackAction))
            {
                canAttackSprite.gameObject.SetActive(true);
                cantAttackSprite.gameObject.SetActive(false);

                attackActions = unit.Actions
                    .OfType<IAttackAction>()
                    .ToArray();
                powerText.text = attackActions.Max(x => x.Damage).ToString();
                foreach (var action in attackActions)
                    action.DamageChanged += AttackActionOnDamageChanged;
            }
            else
            {
                canAttackSprite.gameObject.SetActive(false);
                cantAttackSprite.gameObject.SetActive(true);
                powerText.text = unit.CurrentPower.ToString();
                unit.UnitPowerChanged += ExecutorOnUnitPowerChanged;
            }
        }

        private void ExecutorOnUnitPowerChanged(Unit unit, int before, int after)
        {
            powerText.text = after.ToString();
        }

        private void AttackActionOnDamageChanged(int newVal) 
        {
            powerText.text = attackActions.Max(x => x.Damage).ToString();
        }
    }
}