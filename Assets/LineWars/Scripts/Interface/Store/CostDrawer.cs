using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class CostDrawer : MonoBehaviour
    {
        [SerializeField] private Image coinsImage;
        [SerializeField] private Image diamondsImage;
        [SerializeField] private TMP_Text costText;
        
        private readonly Color coinsColor = new (251, 184, 13);
        private readonly Color diamondsColor = new (254, 57, 59);
        
        public void DrawCost(int cost, CostType costType)
        {
            costText.text = cost.ToString();
            costText.color = costType == CostType.Gold ? coinsColor : diamondsColor;
            coinsImage.gameObject.SetActive(costType == CostType.Gold);
            diamondsImage.gameObject.SetActive(costType == CostType.Diamond);
        }
    }
}