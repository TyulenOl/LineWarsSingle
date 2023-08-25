using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New EnemyPersonality", menuName = "EnemyAI/Create EnemyPersonality")]
    public class EnemyAIPersonality : ScriptableObject
    {
        [field: SerializeField, Range(0, 10)] public float Defensiveness { get; private set; }
        [field: SerializeField, Range(0, 10)] public float Aggressiveness { get; private set; }
        [field: SerializeField, Range(0, 10)] public float InvestmentCoefficient { get; private set; }
    }
}