using System;
using LineWars.Model;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;
using System.Linq;

namespace LineWars
{
    public class SiegeGameReferee : GameReferee
    {
        [SerializeField] private int roundsToWin;
        [ReadOnlyInspector] private int currentRounds;
        
        public int CurrentRounds
        {
            get => currentRounds;
            private set
            {
                currentRounds = value;
                CurrentRoundsChanged?.Invoke(value);
            } 
        }

        public int RoundsToWin => roundsToWin;

        public event Action<int> CurrentRoundsChanged;
        
        public override void Initialize([NotNull] Player me, IEnumerable<BasePlayer> enemies)
        {
            base.Initialize(me, enemies);
            PhaseManager.Instance.PhaseChanged.AddListener(OnPhaseChanged);
            me.OwnedRemoved += OnRemoveOwned;
            me.PhaseExceptions.Add(PhaseType.Buy);
        }

        private void OnPhaseChanged(PhaseType previous, PhaseType current)
        {
            if (current != PhaseType.Replenish)
                return;
            CurrentRounds++;
            if(CurrentRounds >= roundsToWin)
            {
                Win(); 
                PhaseManager.Instance.PhaseChanged.RemoveListener(OnPhaseChanged);
                Me.OwnedRemoved -= OnRemoveOwned;
            }
        }

        private void OnRemoveOwned(Owned removedOwned)
        {
            if (removedOwned is not Unit unit) return;
            if (Me.OwnedObjects.OfType<Unit>().Count() <= 0)
            {
                Lose();
                Me.OwnedRemoved -= OnRemoveOwned;
            }
        }
    }
}
