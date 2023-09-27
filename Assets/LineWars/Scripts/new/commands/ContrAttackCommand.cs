using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class ContrAttackCommand : ICommand
    {
        private readonly BlockAction blockAction;
        private readonly IUnit attacker;
        private readonly IUnit blocker;

        public ContrAttackCommand([NotNull] IUnit attacker, [NotNull] IUnit blocker)
        {
            this.attacker = attacker ?? throw new ArgumentNullException(nameof(attacker));
            this.blocker = blocker ?? throw new ArgumentNullException(nameof(blocker));

            blockAction = attacker.TryGetExecutorAction<BlockAction>(out var action)
                ? action
                : throw new ArgumentException($"{nameof(IUnit)} does not contain {nameof(BlockAction)}");
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
