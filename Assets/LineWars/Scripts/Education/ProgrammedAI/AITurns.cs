using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars
{
    public class AITurns : MonoBehaviour
    {
        [SerializeField, ReadOnlyInspector] private int currentIndex;

        public AITurn GetNextTurn()
        {
            var turns = GetComponents<AITurn>();
            if (currentIndex < turns.Length)
            {
                currentIndex++;
                return turns[--currentIndex];;
            }

            throw new InvalidOperationException("Закончились ходы");
        }
        
        public bool TryGetNextTurn(out AITurn turn)
        {
            var turns = GetComponents<AITurn>();
            if (currentIndex < turns.Length)
            {
                turn = turns[currentIndex];
                currentIndex++;
                return true;
            }

            turn = null;
            return false;
        }

        public bool ContainsNextTurn()
        {
            var turns = GetComponents<AITurn>();
            return currentIndex < turns.Length;
        }
    }
}