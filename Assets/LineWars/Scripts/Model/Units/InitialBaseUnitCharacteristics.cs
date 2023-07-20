//
// using UnityEngine;
// using UnityEngine.Serialization;
//
// namespace LineWars.Model
// {
//     [CreateAssetMenu(fileName = "New BaseUnitCharacteristics", menuName = "UnitCharacteristics", order = 50)]
//     public class InitialBaseUnitCharacteristics: ScriptableObject
//     {
//         [SerializeField] private int initialHp;
//         [SerializeField] private int initialArmor;
//         [SerializeField] private int initialMeleeDamage;
//         [SerializeField] private int speed;
//         [SerializeField] private UnitSize unitSize;
//         [SerializeField] private Passability passability;
//         [SerializeField] [Min(0)] private int visibility; 
//
//         public int InitialHp => initialHp;
//         public int InitialArmor => initialArmor;
//         public int MeleeDamage => initialMeleeDamage;
//         public int Speed => speed;
//         public UnitSize UnitSize => unitSize;
//         public Passability Passability => passability;
//     }
// }