using System;
using LineWars.Controllers;
using TMPro;
using UnityEngine;

namespace LineWars.Interface
{
    public class UserMoneyDrawer : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyAmount;
        [SerializeField] private TMP_Text diamondsAmount;

        private void Awake()
        {
            GameRoot.Instance.UserController.GoldChanged += i => moneyAmount.text = i.ToString();
            GameRoot.Instance.UserController.DiamondsChanged += i => diamondsAmount.text = i.ToString();

            moneyAmount.text = GameRoot.Instance.UserController.UserGold.ToString();
            diamondsAmount.text = GameRoot.Instance.UserController.UserDiamond.ToString();
        }
    }
}