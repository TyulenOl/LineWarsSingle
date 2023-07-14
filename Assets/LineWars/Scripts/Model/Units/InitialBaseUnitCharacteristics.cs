
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New BaseUnitCharacteristics", menuName = "UnitCharacteristics", order = 50)]
    public class InitialBaseUnitCharacteristics: ScriptableObject
    {
        [SerializeField] private int maxHp;
        [SerializeField] private int maxArmor;
        [SerializeField] private int meleeDamage;
        [SerializeField] private int speed;
        [SerializeField] private UnitSize unitSize;
        [SerializeField] private LineType movingLineType;

        public int MaxHp => maxHp;
        public int MaxArmor => maxArmor;
        public int MeleeDamage => meleeDamage;
        public int Speed => speed;
        public UnitSize UnitSize => unitSize;
        public LineType MovingLineType => movingLineType;
    }
}