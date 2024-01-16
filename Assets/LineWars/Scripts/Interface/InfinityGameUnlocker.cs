using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class InfinityGameUnlocker : MonoBehaviour
    {
        [SerializeField] private Image bgImage;
        [SerializeField] private Image ifInactiveImage;

        [SerializeField] private Color inactiveColor;
        [SerializeField] private Color activeColor;


        private void OnEnable()
        {
            if(GameRoot.Instance != null)
                ReDrawAvailability(GameRoot.Instance.CompaniesController.IsInfinityGameUnlocked);
        }

        private void ReDrawAvailability(bool isInfifnityGameActive)
        {
            bgImage.color = isInfifnityGameActive ? activeColor : inactiveColor;
            ifInactiveImage.gameObject.SetActive(!isInfifnityGameActive);
        }
    }
}
