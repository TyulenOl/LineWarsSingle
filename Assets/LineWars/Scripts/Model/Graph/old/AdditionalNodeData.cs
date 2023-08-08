// using System;
// using UnityEngine;
// using UnityEngine.Serialization;
//
// namespace LineWars.Model
// {
//     [Serializable]
//     public class AdditionalNodeData
//     {
//         [SerializeField] private int ownedIndex;
//         [SerializeField] private bool isSpawn;
//         [SerializeField] private Unit leftUnitPrefab;
//         [SerializeField] private Unit rightUnitPrefab;
//
//         [SerializeField] private string groupName;
//         [FormerlySerializedAs("color")] [SerializeField] private Color groupColor;
//
//         public bool HasOwner => ownedIndex != -1;
//         
//         public AdditionalNodeData()
//         {
//             ownedIndex = -1;
//             isSpawn = false;
//             leftUnitPrefab = null;
//             rightUnitPrefab = null;
//         }
//         public AdditionalNodeData(
//             int ownedIndex,
//             bool isSpawn,
//             Unit leftUnitPrefab,
//             Unit rightUnitPrefab,
//             string groupName,
//             Color groupColor)
//         {
//             this.ownedIndex = ownedIndex;
//             this.isSpawn = isSpawn;
//             this.leftUnitPrefab = leftUnitPrefab;
//             this.rightUnitPrefab = rightUnitPrefab;
//             this.groupName = groupName;
//             this.groupColor = groupColor;
//         }
//         
//         public int OwnedIndex => ownedIndex;
//         public bool IsSpawn => isSpawn;
//         public Unit LeftUnitPrefab => leftUnitPrefab;
//         public Unit RightUnitPrefab => rightUnitPrefab;
//         public string GroupName => groupName;
//         public Color GroupColor => groupColor;
//     }
// }