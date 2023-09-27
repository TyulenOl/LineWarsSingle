using System;
using UnityEngine;

namespace LineWars.Model
{
    [Serializable]
    public class LineTypeCharacteristics
    {
        [SerializeField] private LineType lineType;
        [SerializeField, Min(0)] private int maxHp;
        [SerializeField] private Sprite sprite;
        [SerializeField, Min(0)] private float width = 5;

        public LineType LineType => lineType;
        public int MaxHp => maxHp;
        public Sprite Sprite => sprite;

        public float Width => width;

        public LineTypeCharacteristics(LineType type)
        {
            lineType = type;
            maxHp = 0;
        }
    }
}