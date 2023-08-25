using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    [CreateAssetMenu(fileName = "BasePlayerDifficultySettings", menuName = "Create BasePlayerDifficultySettings", order = 56)]
    public class BasePlayerDifficultySettings: ScriptableObject
    {
        [field: SerializeField] public int MoneyForFirstCapturingNode { get; private set; }
    }
}