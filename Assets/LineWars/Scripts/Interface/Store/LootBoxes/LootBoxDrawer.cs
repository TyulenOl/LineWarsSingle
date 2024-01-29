using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class LootBoxDrawer : MonoBehaviour
    {
        [SerializeField] private Image lootBoxImage;
        [SerializeField] private Image bgImage;
        [SerializeField] private TMP_Text boxName;
        [SerializeField] private TMP_Text boxDescription;
        [SerializeField] private Button button;
        [SerializeField] private CostDrawer costDrawer;

        private LootBoxInfo lootBoxInfo;
        public LootBoxInfo LootBoxInfo => lootBoxInfo;

        public void Init(LootBoxInfo lootBoxInfo, UnityAction onButtonClickAction = null)
        {
            costDrawer.DrawCost(lootBoxInfo.Cost, lootBoxInfo.CostType);
            boxName.text = lootBoxInfo.Name;
            if (boxDescription != null) 
                boxDescription.text = lootBoxInfo.Description;
            lootBoxImage.sprite = lootBoxInfo.BoxSprite;
            if(bgImage != null)
                bgImage.sprite = lootBoxInfo.BgSprite;
            if(onButtonClickAction != null)
                button.onClick.AddListener(onButtonClickAction);
            this.lootBoxInfo = lootBoxInfo;
        }
    }
}
