using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacteristicDrawer : MonoBehaviour
{
    [SerializeField] private Image characteristicImage;
    [SerializeField] private TMP_Text characteristicCurrentAmount;

    public void Init(Sprite characteristicSprite, string characteristicAmount)
    {
        characteristicImage.sprite = characteristicSprite;
        characteristicCurrentAmount.text = characteristicAmount;
    }

    public void ReDraw(string characteristicAmount)
    {
        characteristicCurrentAmount.text = characteristicAmount;
    }
}