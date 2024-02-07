using System;
using LineWars.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class Introduction : MonoBehaviour
    {
        private static bool isFirstLoad = true;
        private static GameRoot GameRoot => GameRoot.Instance;

        [SerializeField] private Button button;
        [SerializeField] private Transform onLoadedText;
        [SerializeField] private Transform mainMenu;
        [SerializeField] private UIStack uiStack;

        private void Start()
        {
            if (isFirstLoad)
            {
                isFirstLoad = false;

                if (GameRoot.GameReady)
                    ReadyGame();
                else
                    GameRoot.OnGameReady += OnGameReady;

            }
            else
            {
                uiStack.PushElement(mainMenu);
            }
        }

        private void OnDestroy()
        {
            if (button != null)
                button.onClick.RemoveListener(OnClick);
        }

        private void OnGameReady()
        {
            GameRoot.OnGameReady -= OnGameReady;
            ReadyGame();
        }

        private void ReadyGame()
        {
            button.interactable = true;
            button.onClick.AddListener(OnClick);
            onLoadedText.gameObject.SetActive(true);
        }

        private void OnClick()
        {
            uiStack.PushElement(mainMenu);
        }
    }
}
