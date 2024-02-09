using System;
using System.Collections;
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
        [SerializeField] private Animator animator;
        private static readonly int startId = Animator.StringToHash("start");
        [SerializeField] private float animationTime;

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
            if (animator != null)
                Animate();
            else 
                uiStack.PushElement(mainMenu);
            
        }

        private void Animate()
        {
            animator.SetTrigger(startId);
            StartCoroutine(Waiting());
        }

        private IEnumerator Waiting()
        {
            yield return new WaitForSeconds(animationTime);
            uiStack.PushElement(mainMenu);
        }
    }
}
