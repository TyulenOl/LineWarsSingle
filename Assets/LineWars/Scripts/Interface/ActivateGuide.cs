using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars
{
    public class ActivateGuide : MonoBehaviour
    {
        [SerializeField] private GameObject guide;
        private const string KEY = "Guide";

        public void Activate()
        {
            var isActive = PlayerPrefs.HasKey(KEY) && PlayerPrefs.GetInt(KEY) == 1;
            if (!isActive)
            {
                guide.SetActive(true);
                PlayerPrefs.SetInt(KEY, 1);
                PlayerPrefs.Save();
            }
        }
    }
}