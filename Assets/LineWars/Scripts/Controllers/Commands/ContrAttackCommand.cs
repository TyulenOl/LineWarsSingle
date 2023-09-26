using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class ContrAttackCommand: ICommand
    {
        private readonly ModelComponentUnit.BlockAction blockAction;
        private readonly ModelComponentUnit attacker;
        private readonly ModelComponentUnit blocker;
        
        public ContrAttackCommand([NotNull] ModelComponentUnit attacker, [NotNull] ModelComponentUnit blocker)
        {
            this.attacker = attacker ?? throw new ArgumentNullException(nameof(attacker));
            this.blocker = blocker ?? throw new ArgumentNullException(nameof(blocker));
            
            blockAction = attacker.TryGetExecutorAction<ModelComponentUnit.BlockAction>(out var action) 
                ? action 
                : throw new ArgumentException($"{nameof(ModelComponentUnit)} does not contain {nameof(ModelComponentUnit.BlockAction)}");
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
            return $"Юнит {attacker} контратаковал юнита {blocker}";
        }
    }
}