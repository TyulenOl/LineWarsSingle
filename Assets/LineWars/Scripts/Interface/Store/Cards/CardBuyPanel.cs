using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class CardBuyPanel: MonoBehaviour // лучше не наследовать от BuyPanel
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text description;
        [SerializeField] private Image sprite;
        [SerializeField] private CostDrawer costDrawer;
        [SerializeField] private Button buyButton;
        [SerializeField] private CanvasGroup buttonCanvasGroup;
        
        [SerializeField] private TMP_Text hpText;
        [SerializeField] private TMP_Text powerText;
        [SerializeField] private TMP_Text apText;

        [SerializeField] private LayoutGroup layoutGroup;
        [SerializeField] private ActionIconDrawer actionIconDrawerPrefab;

        private readonly List<ActionIconDrawer> actionIconDrawers = new ();

        public UnityEvent OnClick => buyButton.onClick;


        public void OpenWindow(DeckCard deckCard, bool buttonInteractable)
        {
            gameObject.SetActive(true);
            Redraw(deckCard);
            SetButtonInteractable(buttonInteractable);
        }
        
        private void Redraw(DeckCard deckCard)
        {
            if (actionIconDrawers.Count > 0)
            {
                foreach (var drawer in actionIconDrawers)
                    Destroy(drawer.gameObject);
                actionIconDrawers.Clear();
            }
            
            _name.text = deckCard.Name;
            description.text = deckCard.Description;
            sprite.sprite = deckCard.Image;
            
            costDrawer.DrawCost(deckCard.ShopCost, deckCard.ShopCostType);
            
            hpText.text = deckCard.Unit.MaxHp.ToString();
            powerText.text = deckCard.Unit.InitialPower.ToString();
            apText.text = deckCard.Unit.MaxActionPoints.ToString();

            foreach (var command in deckCard.Unit.UnitCommands.OrderBy(x => x))
            {
                var instance = Instantiate(actionIconDrawerPrefab, layoutGroup.transform);
                instance.Redraw(command);
                actionIconDrawers.Add(instance);
            }
        }

        private void SetButtonInteractable(bool value)
        {
            buyButton.interactable = value;
            buttonCanvasGroup.alpha = value ? 1f : 0.5f;
        }
    }
}