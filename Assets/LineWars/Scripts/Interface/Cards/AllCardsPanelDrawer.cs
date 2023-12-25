using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class AllCardsPanelDrawer : MonoBehaviour
    {
        [SerializeField] private LayoutGroup cardsLayoutGroup;
        [SerializeField] private RectTransform cardDrawInfoPrefab;
       
        [SerializeField] private CardDrawInfo bigCardInfo;
        

        private void Awake()
        {
            ReDrawAllCards();
        }

        private void ReDrawAllCards()
        {
            foreach (var drawInfo in cardsLayoutGroup.GetComponentsInChildren<CardDrawInfo>())
            {
                Destroy(drawInfo.gameObject);
            }
            
            var cards = GameRoot.Instance.CardsDatabase.Values;
            foreach (var card in cards)
            {
                var instance = Instantiate(cardDrawInfoPrefab, cardsLayoutGroup.transform);
                var cardInstance = instance.GetComponentInChildren<CardDrawInfo>();
                cardInstance.ReDraw(card);
                cardInstance.InfoButton.onClick.AddListener(() =>
                {
                    bigCardInfo.ReDraw(card);
                    bigCardInfo.gameObject.SetActive(true);
                });
            }
        }
    }
}
