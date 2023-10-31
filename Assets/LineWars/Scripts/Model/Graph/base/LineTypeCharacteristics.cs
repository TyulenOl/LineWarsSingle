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

        public LineType LineType
        {
            get => lineType;
            set => lineType = value;
        }

        public int MaxHp
        {
            get => maxHp;
            set => maxHp = value;
        }

        public Sprite Sprite
        {
            get => sprite;
            set => sprite = value;
        }

        public float Width
        {
            get => width;
            set => width = value;
        }

        public LineTypeCharacteristics(LineType type)
        {
            lineType = type;
            maxHp = 0;
        }
    }
}