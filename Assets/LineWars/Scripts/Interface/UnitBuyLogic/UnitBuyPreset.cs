using System.Collections;
using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;
[System.Serializable]
public class UnitBuyPreset
{
    [SerializeField] private UnitType firstUnitType;
    [SerializeField] private UnitType secondUnitType;
    [SerializeField] private int cost;
}
