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
        [SerializeField, ReadOnlyInspector] private int currentRounds;
        
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
        
        public override void Initialize([NotNull] Player player, IEnumerable<BasePlayer> enemies)
        {
            base.Initialize(player, enemies);
            PhaseManager.Instance.PhaseEntered.AddListener(OnPhaseEntered);
            PhaseManager.Instance.PhaseExited.AddListener(OnPhaseExited);
            player.OwnedRemoved += OnRemoveOwned;
        }

        private void OnPhaseExited(PhaseType previous)
        {
            if(previous != PhaseType.Buy)
                return;
            Player.LocalPlayer.PhaseExceptions.Add(PhaseType.Buy);
        }

        private void OnPhaseEntered(PhaseType current)
        {
            if (current != PhaseType.Payday)
                return;
            CurrentRounds++;
            if(CurrentRounds >= roundsToWin)
            {
                Win(); 
                PhaseManager.Instance.PhaseEntered.RemoveListener(OnPhaseEntered);
                Player.OwnedRemoved -= OnRemoveOwned;
            }
        }

        private void OnRemoveOwned(Owned removedOwned)
        {
            if (removedOwned is not Unit unit) return;
            if (Player.OwnedObjects.OfType<Unit>().Count() <= 0)
            {
                Lose();
                Player.OwnedRemoved -= OnRemoveOwned;
            }
        }
    }
}
