using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Controllers
{
    [RequireComponent(typeof(Button))]
    public class ButtonPlaySFX : PlaySFX
    {
        private void Start()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(Play);
        }
    }
}
