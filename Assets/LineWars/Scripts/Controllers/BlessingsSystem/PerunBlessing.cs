using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "Blessings/PerunBlessing")]
    public class PerunBlessing: BaseBlessing
    {
        [SerializeField] private int damage;
        private Player player => Player.LocalPlayer;
        
        public override event Action Completed;
        
        public override bool CanExecute()
        {
            return player.GetAllEnemiesUnits().Any();
        }

        public override void Execute()
        {
            // TODO анимации
            foreach (var enemy in player.GetAllEnemiesUnits())
            {
                enemy.DealDamageThroughArmor(damage);
            }

            Completed?.Invoke();
        }
    }
}