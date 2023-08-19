using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Enemy Difficulty", menuName = "EnemyAI/Enemy Difficulty")]
    public class EnemyDifficulty : ScriptableObject
    {
        [SerializeField] private AnimationCurve curve;

        public AnimationCurve Curve => curve;
    }
}
