using System;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "Blessings/PowerBlessing")]
    public class PowerBlessing : BaseBlessing
    {
        [SerializeField] private int powerBuff;

        [Tooltip("Раунд - это последовательность вся действий до фазы Replenish")] 
        [SerializeField] private int roundsCount;

        public override event Action Completed;

        public override bool CanExecute()
        {
            return Player.MyUnits.Any();
        }

        public override void Execute()
        {
            foreach (var unit in Player.MyUnits)
            {
                unit.AddEffect(
                    new RoundsTemporaryEffectDecorator<Node, Edge, Unit>(
                        unit,
                        new PowerBuffEffect<Node, Edge, Unit>(unit, powerBuff),
                        roundsCount)
                );
            }

            Completed?.Invoke();
        }
        
        protected override string DefaultName => "Благословление силы";
        protected override string DefaultDescription => $"Добавляет всем союзным юнитам <color=#E22B12>{powerBuff}</color> ед. силы на {roundsCount} раундов";
    }
}