using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public abstract class GameReferee: MonoBehaviour
    {
        protected Player Player;
        protected List<BasePlayer> Enemies;
        public event Action Won;
        public event Action Lost;

        protected virtual bool noBaseWinCondition => true;
        
        public virtual void Initialize(
            [NotNull] Player player, 
            IEnumerable<BasePlayer> enemies)
        {
            Player = player ? player : throw new ArgumentNullException(nameof(player));
            Enemies = enemies
                .Where(x => x != null)
                .Where(x => x != Player)
                .ToList();

            if(noBaseWinCondition) 
            {
                InitializeNoBaseCondition();
            }
        }

        private void InitializeNoBaseCondition()
        {
            Player.OwnedRemoved += OnPlayerOwnedRemoved;
            foreach(var enemy in Enemies)
            {
                enemy.OwnedRemoved += OnEnemyOwnedRemoved;
            }
        }

        private void OnPlayerOwnedRemoved(Owned owned)
        {
            var baseCount = Player.MyNodes.Where((node) => node.IsBase).Count();
            var unitCount = Player.MyUnits.Count();
            if (baseCount <= 0 && unitCount <= 0)
            {
                Lose();
                Player.OwnedRemoved -= OnPlayerOwnedRemoved;
            }
        }

        private void OnEnemyOwnedRemoved(Owned owned)
        {
            foreach(var enemy in Enemies)
            {
                var baseCount = enemy.MyNodes.Where(x => x.IsBase).Count();
                var unitCount = enemy.MyUnits.Count();
                if(baseCount <= 0 && unitCount <= 0)
                {
                    Win();
                    foreach(var enemy1 in Enemies)
                    {
                        enemy1.OwnedRemoved -= OnEnemyOwnedRemoved;
                    }
                    return;
                }
            }
        }

        protected void Win()
        {
            Won?.Invoke();
        }

        protected void Lose()
        {
            Lost?.Invoke();
        }
    }
}