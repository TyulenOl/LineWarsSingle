//
//
// namespace LineWars.Model
// {
//     // Структура необходимая для сохранения текущих характеристик юнита
//     public struct BaseUnitCharacteristics
//     {
//         public int CurrentHp;
//         public int CurrentArmor;
//         public int MeleeDamage;
//         public int CurrentSpeedPoints;
//         public UnitSize UnitSize;
//         public Passability Passability;
//
//         public BaseUnitCharacteristics(InitialBaseUnitCharacteristics characteristics)
//         {
//             CurrentHp = characteristics.InitialHp;
//             CurrentArmor = characteristics.InitialArmor;
//             MeleeDamage = characteristics.MeleeDamage;
//             CurrentSpeedPoints = characteristics.Speed;
//             UnitSize = characteristics.UnitSize;
//             Passability = characteristics.Passability;
//         }
//
//         private BaseUnitCharacteristics(BaseUnitCharacteristics other)
//         {
//             CurrentHp = other.CurrentHp;
//             CurrentArmor = other.CurrentArmor;
//             MeleeDamage = other.MeleeDamage;
//             CurrentSpeedPoints = other.CurrentSpeedPoints;
//             UnitSize = other.UnitSize;
//             Passability = other.Passability;
//         }
//
//         public BaseUnitCharacteristics CreateCopy() => new BaseUnitCharacteristics(this);
//     }
// }