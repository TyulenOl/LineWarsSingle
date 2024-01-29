using LineWars.Controllers;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class BlessingBuyDrawer : MonoBehaviour
    {
        [SerializeField] private Image blessingImage;
        [SerializeField] private CostDrawer costDrawer;
        [SerializeField] private TMP_Text blessingName;
        [SerializeField] private Button buyButton;
        
        
        public void ReDraw(BlessingId blessingId, UnityAction action)
        {
            var store = GameRoot.Instance.Store;
            var cost = -1;
            if (store.BlessingToCost.TryGetValue(blessingId, out var costValue))
                cost = costValue;
            
            costDrawer.DrawCost(cost, CostType.Gold);
            blessingImage.sprite = DrawHelper.GetSpriteByBlessingID(blessingId);
            blessingName.text = DrawHelper.GetBlessingNameByBlessingID(blessingId);
            buyButton.onClick.AddListener(action);
        }
    }
}
