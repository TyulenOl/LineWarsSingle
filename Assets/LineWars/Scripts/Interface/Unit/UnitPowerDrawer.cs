using System.Linq;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Interface
{
    public class UnitPowerDrawer: MonoBehaviour
    {
        [SerializeField] private Unit unit;
        [SerializeField] private GameObject canAttackSprite;
        [SerializeField] private GameObject cantAttackSprite;

        private void Start()
        {
            if (unit.UnitCommands.Any(x => x.IsAttack()))
            {
                canAttackSprite.gameObject.SetActive(true);
                cantAttackSprite.gameObject.SetActive(false);
            }
            else
            {
                canAttackSprite.gameObject.SetActive(false);
                cantAttackSprite.gameObject.SetActive(true);
            }
        }
    }
}