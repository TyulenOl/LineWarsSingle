using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New EnemyPersonality", menuName = "EnemyAI/Create EnemyPersonality")]
    public class EnemyAIPersonality : ScriptableObject
    {
        [field: SerializeField, Min(0)] public float InvestmentCoefficient { get; private set; }
        [field: SerializeField] public StrategyCoefficient StrategyCoefficient { get; private set; }
    }
}