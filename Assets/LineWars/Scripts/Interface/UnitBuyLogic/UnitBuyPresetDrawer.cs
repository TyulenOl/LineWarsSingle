using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitBuyPresetDrawer : MonoBehaviour
{
    [SerializeField] private TMP_Text cost;
    [SerializeField] private Image image;
    [SerializeField] private Button button;

    public Button Button => button;

    private UnitBuyPreset unitBuyPreset;
    public UnitBuyPreset UnitBuyPreset
    {
        get => unitBuyPreset;
        set
        {
            unitBuyPreset = value;
            Init();
        }
    }

    private void Init()
    {
        image.sprite = unitBuyPreset.Image;
        cost.text = unitBuyPreset.Cost.ToString();
    }
}
