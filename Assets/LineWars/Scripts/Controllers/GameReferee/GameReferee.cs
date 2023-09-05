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
        public static GameReferee Instance { get; private set; }
        
        protected Player Me;
        protected List<BasePlayer> Enemies;
        
        public event Action Wined;
        public event Action Losed;

        protected virtual void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Debug.LogError($"Больше чем два {nameof(GameReferee)} на сцене");
                Destroy(gameObject);
            }
        }

        public virtual void Initialize([NotNull] Player me, IEnumerable<BasePlayer> enemies)
        {
            Me = me ? me : throw new ArgumentNullException(nameof(me));
            Enemies = enemies
                .Where(x => x != null)
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