using System.Collections;
using System.Collections.Generic;
using LineWars.Interface;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitBuyPresetInfoDrawer : MonoBehaviour
{
    [SerializeField] private TMP_Text unitName;
    [SerializeField] private TMP_Text unitDescription;

    [SerializeField] private TMP_Text armorAmount;
    [SerializeField] private TMP_Text hpAmount;
    [SerializeField] private TMP_Text actionPointsAmount;

    [SerializeField] private Image unitImage;

    public void Init(Unit unit)
    {
        unitName.text = unit.UnitName;
        unitDescription.text = unit.UnitDescription;

        armorAmount.text = unit.MaxArmor.ToString();
        hpAmount.text = unit.MaxHp.ToString();
        actionPointsAmount.text = unit.MaxActionPoints.ToString();

        unitImage.sprite = unit.Sprite;
        var rect = unitImage.rectTransform.rect;
        unitImage.rectTransform.sizeDelta = new Vector2(rect.size.x * unit.Sprite.rect.width / unit.Sprite.rect.height,
            rect.size.y);
    }
}