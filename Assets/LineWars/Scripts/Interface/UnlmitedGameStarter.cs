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
        [SerializeField] private Image missionImage;
        
        [SerializeField] [TextArea] private string simpleDescription;
        [SerializeField] [TextArea] private string mediumDescription;
        [SerializeField] [TextArea] private string hardDescription;


        [SerializeField] private Color easyColor;
        [SerializeField] private Color middleColor;
        [SerializeField] private Color hardColor;


        private Dictionary<InfinityGameMode, (string, Color)> tuplesDict;


        private InfinityGameMode infinityGameMode;

        private void Awake()
        {
            button.onClick.AddListener(OnButtonClick);
            missionImage.color = Color.white;

            tuplesDict = new Dictionary<InfinityGameMode, (string, Color)>
            {
                { InfinityGameMode.Simple, (simpleDescription, easyColor) },
                { InfinityGameMode.Hard, (hardDescription, hardColor) },
                { InfinityGameMode.Medium, (mediumDescription, middleColor) }
            };
        }

        private void OnButtonClick()
        {
            InfinityGame.Load(infinityGameMode);
        }

        public void ChooseEasy() => ChooseMode(InfinityGameMode.Simple);

        public void ChooseMedium() => ChooseMode(InfinityGameMode.Medium);

        public void ChooseHard() => ChooseMode(InfinityGameMode.Hard);

        private void ChooseMode(InfinityGameMode gameMode)
        {
            infinityGameMode = gameMode;
            var tuple = tuplesDict[gameMode];
            missionDescription.text = tuple.Item1;
            missionImage.color = tuple.Item2;
        }
    }
}
