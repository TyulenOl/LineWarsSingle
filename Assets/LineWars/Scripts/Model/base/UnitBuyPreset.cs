using System.Collections;
using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Model
{
    [System.Serializable]
    public class UnitBuyPreset
    {
        [SerializeField] private string name;
        [SerializeField] private string description;
        [SerializeField] private UnitType firstUnitType;
        [SerializeField] private UnitType secondUnitType;
        [SerializeField] private int cost;
        [SerializeField] private Sprite image;


        public string Name => name;

        public string Description => description;

        public UnitType FirstUnitType => firstUnitType;
        public UnitType SecondUnitType => secondUnitType;
        public int Cost => cost;
        public Sprite Image => image;
    }

}
