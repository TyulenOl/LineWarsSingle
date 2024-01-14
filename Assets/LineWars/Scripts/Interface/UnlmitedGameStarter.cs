using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class UnlmitedGameStarter : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text missionDescription;

        [SerializeField] [TextArea] private string simpleDescription;
        [SerializeField] [TextArea] private string mediumDescription;
        [SerializeField] [TextArea] private string hardDescription;
        
        
        private InfinityGameMode infinityGameMode;

        private void Awake()
        {
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            InfinityGame.Load(infinityGameMode);
        }

        public void ChooseEasy()
        {
            infinityGameMode = InfinityGameMode.Simple;
            missionDescription.text = simpleDescription;
        }
        
        public void ChooseMedium()
        {
            infinityGameMode = InfinityGameMode.Medium;
            missionDescription.text = mediumDescription;
        }
        
        public void ChooseHard()
        {
            infinityGameMode = InfinityGameMode.Hard;
            missionDescription.text = hardDescription;
        }
    }
}
