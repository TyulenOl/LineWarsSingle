using System;
using UnityEngine;

namespace LineWars.Model
{
    [Serializable]
    public class AdditionalEdgeData
    {
        [SerializeField] private LineType lineType;
        public LineType LineType => lineType;

        public AdditionalEdgeData()
        {
            this.lineType = LineType.InfantryRoad;
        }
        
        public AdditionalEdgeData(LineType lineType)
        {
            this.lineType = lineType;
        }
    }
}