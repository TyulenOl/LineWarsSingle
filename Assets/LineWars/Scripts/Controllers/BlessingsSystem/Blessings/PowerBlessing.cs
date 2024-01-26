using System;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "Blessings/PowerBlessing")]
    public class PowerBlessing: BaseBlessing // нужен еффект?
    {
        public override event Action Completed;
        public override bool CanExecute()
        {
            throw new NotImplementedException();
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}