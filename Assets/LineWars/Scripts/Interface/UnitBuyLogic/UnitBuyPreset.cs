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
    [SerializeField] private Sprite image;

    public UnitType FirstUnitType => firstUnitType;

    public UnitType SecondUnitType => secondUnitType;

    public int Cost => cost;

    public Sprite Image => image;
}
