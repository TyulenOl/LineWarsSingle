using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    [CreateAssetMenu(fileName = "New CompanyData", menuName = "Data/Create CompanyData", order = 50)]
    public class CompanyData: ScriptableObject, IEquatable<CompanyData>
    {
        [SerializeField] private string companyName;
        [SerializeField] [TextArea(5,10)] private string companyDescription;
        [SerializeField] private Sprite companyImage;
        [SerializeField] private List<MissionData> missions;
        [SerializeField] private MissionsMapUI missionsMapUIPrefab;
        [SerializeField] private NationType companyNation;

        private void OnEnable()
        {
            if (missions == null)
                return;
            if (missions.Any(x => x == null))
            {
                Debug.LogWarning($"Any mission is null on {name}");
            }

            if (missions.Count != missions.Distinct().Count())
            {
                Debug.LogWarning($"Missions is duplicated on {name}");
            }

            if (missionsMapUIPrefab == null)
                Debug.LogError($"{nameof(missionsMapUIPrefab)} is null on {name}");
            //else if (missionsMapUIPrefab.UIsCount != missions.Count)
                //Debug.LogError("The number of missions does not match the number of UI");
        }

        public string Name => companyName;
        public string Description => companyDescription;
        public Sprite Image => companyImage;
        public IReadOnlyList<MissionData> Missions => missions;
        public NationType Nation => companyNation;
        public MissionsMapUI MissionsMapUIPrefab => missionsMapUIPrefab;


        public bool Equals(CompanyData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return companyName == other.companyName 
                   && companyDescription == other.companyDescription
                   && Equals(companyImage, other.companyImage) 
                   && Equals(missions, other.missions) 
                   && companyNation == other.companyNation
                   && missionsMapUIPrefab == other.missionsMapUIPrefab;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CompanyData) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                companyName,
                companyDescription,
                companyImage,
                missions,
                (int) companyNation,
                missionsMapUIPrefab);
        }
    }
}