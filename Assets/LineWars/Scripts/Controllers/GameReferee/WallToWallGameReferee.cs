using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public class WallToWallGameReferee : GameReferee
    {
        public override void Initialize([NotNull] Player player, IEnumerable<BasePlayer> enemies)
        {
            base.Initialize(player, enemies);
            player.PhaseExceptions.Add(PhaseType.Buy);
            foreach(var enemy in enemies)
            {
                enemy.PhaseExceptions.Add(PhaseType.Buy);
                enemy.OwnedRemoved += Enemy_OwnerRemoved;
            }
            player.OwnedRemoved += Me_OwnedRemoved;
        }

        private void Me_OwnedRemoved(Owned obj)
        {
            if (Player.MyUnits.Count() == 0)
                Lose();
        }

        private void Enemy_OwnerRemoved(Owned obj)
        {
            foreach(var enemy in Enemies)
            {
                if (enemy.MyUnits.Count() != 0)
                    return;
            }
            Win();
        }
    }
}
