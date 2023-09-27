using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class UnitContrAttackCommand: ICommand
    {
        private readonly ComponentUnit.BlockAction blockAction;
        private readonly ComponentUnit attacker;
        private readonly ComponentUnit blocker;
        
        public UnitContrAttackCommand([NotNull] ComponentUnit attacker, [NotNull] ComponentUnit blocker)
        {
            this.attacker = attacker ? attacker : throw new ArgumentNullException(nameof(attacker));
            this.blocker = blocker ? blocker : throw new ArgumentNullException(nameof(blocker));
            
            blockAction = attacker.TryGetExecutorAction<ComponentUnit.BlockAction>(out var action) 
                ? action 
                : throw new ArgumentException($"{nameof(ComponentUnit)} does not contain {nameof(ComponentUnit.BlockAction)}");
        }
        
        public void Execute()
        {
            blockAction.ContrAttack(blocker);
        }

        public bool CanExecute()
        {
            return blockAction.CanContrAttack(blocker);
        }

        public string GetLog()
        {
            return $"Юнит {attacker.gameObject.name} контратаковал юнита {blocker.gameObject.name}";
        }
    }
}