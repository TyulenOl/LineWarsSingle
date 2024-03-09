using System;
using LineWars.Model;
using TMPro;
using UnityEngine;

namespace LineWars.Interface
{
    public class MoneyDrawer: MonoBehaviour
    {
        [SerializeField] private GameObject diamondsPart;
        [SerializeField] private GameObject goldPart;

        [SerializeField] private TMP_Text diamondsAmount;
        [SerializeField] private TMP_Text goldAmount;
        
        
        public void Redraw(Money money)
        {
            switch (money.Type)
            {
                case CostType.Gold:
                    diamondsPart.SetActive(false);
                    goldPart.SetActive(true);
                    goldAmount.text = money.Amount.ToString();
                    break;
                case CostType.Diamond:
                    diamondsPart.SetActive(true);
                    goldPart.SetActive(false);
                    diamondsAmount.text = money.Amount.ToString();
                    break;
                default:
                    diamondsPart.SetActive(false);
                    goldPart.SetActive(false);
                    break;
            }
        }
    }
}