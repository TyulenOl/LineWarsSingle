using System;
using System.Linq;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    [RequireComponent(typeof(Image))]
    public class UnitPowerDrawer: MonoBehaviour
    {
        private Image image;

        [SerializeField] private Unit unit;
        [SerializeField] private Sprite canAttackSprite;
        [SerializeField] private Sprite cantAttackSprite;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        private void Start()
        {
            if (unit.UnitCommands.Any(x => x.IsAttack()))
            {
                image.sprite = canAttackSprite;
            }
            else
            {
                image.sprite = cantAttackSprite;
            }
        }
    }
}