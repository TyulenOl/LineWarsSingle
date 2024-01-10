using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.LootBoxes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LineWars
{
    public class LootBoxDrawer : MonoBehaviour
    {
        [SerializeField] private TMP_Text costText;
        [SerializeField] private Image lootBoxImage;
        [SerializeField] private Image coinsImage;
        [SerializeField] private Image diamondsImage;
        [SerializeField] private Image bgImage;
        [SerializeField] private TMP_Text boxName;
        [SerializeField] private TMP_Text boxDescription;
        [SerializeField] private Button button;
        

        private readonly Color coinsColor = new (251, 184, 13);
        private readonly Color diamondsColor = new (254, 57, 59);
        
        public void Init(LootBoxInfo lootBoxInfo, UnityAction onButtonClickAction = null)
        {
            costText.text = lootBoxInfo.Cost.ToString();
            costText.color = lootBoxInfo.CostType == CostType.Gold ? coinsColor : diamondsColor;
            coinsImage.gameObject.SetActive(lootBoxInfo.CostType == CostType.Gold);
            diamondsImage.gameObject.SetActive(lootBoxInfo.CostType == CostType.Diamond);
            boxName.text = lootBoxInfo.Name;
            if (boxDescription != null) 
                boxDescription.text = lootBoxInfo.Description;
            lootBoxImage.sprite = lootBoxInfo.BoxSprite;
            if(bgImage != null)
                bgImage.sprite = lootBoxInfo.BgSprite;
            if(onButtonClickAction != null)
                button.onClick.AddListener(onButtonClickAction);
        }

    }
}
