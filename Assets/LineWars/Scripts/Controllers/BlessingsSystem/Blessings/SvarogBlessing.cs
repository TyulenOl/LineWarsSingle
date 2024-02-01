﻿using System;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "Blessings/SvarogBlessing")]
    public class SvarogBlessing: BaseBlessing
    {
        [SerializeField] private int addedArmor;
        public override event Action Completed;
        public override bool CanExecute()
        {
            return Player.MyUnits.Any();
        }

        public override void Execute()
        {
            // TODO анимации
            foreach (var unit in Player.MyUnits)
            {
                unit.CurrentArmor += addedArmor;
            }
            
            Completed?.Invoke();
        }

        protected override string DefaultName => "Благословление \"Сварога\"";
        protected override string DefaultDescription => $"Подымает всем союзным юнитам <color=#12C9E2>{addedArmor}</color> ед. брони";
    }
}