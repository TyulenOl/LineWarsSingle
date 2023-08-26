using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    [CreateAssetMenu(fileName = "PlayerRules", menuName = "Create PlayerRules", order = 56)]
    public class PlayerRules: ScriptableObject
    {
        [field: SerializeField] public int MoneyForFirstCapturingNode { get; private set; }
    }
}