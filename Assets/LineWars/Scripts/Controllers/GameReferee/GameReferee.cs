using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures;
using JetBrains.Annotations;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public abstract class GameReferee: MonoBehaviour
    {
        protected Player Player;
        protected List<BasePlayer> Enemies;
        public event Action Wined;
        public event Action Losed;
        
        public virtual void Initialize(
            [NotNull] Player player, 
            IEnumerable<BasePlayer> enemies)
        {
            Player = player ? player : throw new ArgumentNullException(nameof(player));
            Enemies = enemies
                .Where(x => x != null)
                .Where(x => x != Player)
                .ToList();
        }

        protected void Win()
        {
            Wined?.Invoke();
        }

        protected void Lose()
        {
            Losed?.Invoke();
        }
    }
}