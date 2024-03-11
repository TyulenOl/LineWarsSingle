using System;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    [RequireComponent(typeof(Button))]
    public class FinishBuyTurnButton: MonoBehaviour
    {
        private const string toggleKey = "skipFinishBuyTurnDialog";
        private static UserInfoController UserController => GameRoot.Instance?.UserController;
        private static Player Player => Player.LocalPlayer;
        private static SingleGameRoot SingleGameRoot => SingleGameRoot.Instance; 
        
        private Button button;
        [SerializeField] private GameObject buyUnitsLayerContainer;
        [SerializeField] private bool openDialogPanel = true;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void Start()
        {
            if (SingleGameRoot.GameReferee is WallToWallGameReferee 
                && Player.CanBuyAnyCard())
            {
                Player.PlayerBuyDeckCard += OnPlayerBuyDeckCardWTW;
                button.interactable = false;
            }
        }

        private void OnPlayerBuyDeckCardWTW(BasePlayer basePlayer, DeckCard deckCard)
        {
            Player.PlayerBuyDeckCard -= OnPlayerBuyDeckCardWTW;
            button.interactable = true;
        }

        private void OnEnable()
        {
            button.onClick.AddListener(OnClick);
        }
        
        private void OnDisable()
        {
            button.onClick.RemoveListener(OnClick);
        }
        
        private void OnClick()
        {
            if (!openDialogPanel || UserController == null || !Player.CanBuyAnyCard())
            {
                ClosePanelAndFinishTurn();
            }
            else
            {
                var toggleIsOn = UserController.KeyToBool[toggleKey];
                if (toggleIsOn)
                {
                    ClosePanelAndFinishTurn();
                }
                else
                {
                    OpenDialogPanel(false);
                }
            }
        }

        private void OpenDialogPanel(bool toggleIsOn)
        {
            UIPanel.OpenDialogPanel(
                "Вы уверены что хотите завершить ход?",
                "У вас еще остались монеты, для покупки какого-то юнита.",
                "да",
                "нет",
                "Не показывать это окно больше",
                toggleIsOn,
                DialogOnAccept,
                DialogOnReject
                );
        }

        private void DialogOnAccept(bool isOn)
        {
            UserController.KeyToBool[toggleKey] = isOn;
            ClosePanelAndFinishTurn();
        }

        private void DialogOnReject(bool isOn)
        {
            UserController.KeyToBool[toggleKey] = isOn;
        }
        
        private void ClosePanelAndFinishTurn()
        {
            buyUnitsLayerContainer.SetActive(false);
            Player.FinishTurn();
        }
    }
}