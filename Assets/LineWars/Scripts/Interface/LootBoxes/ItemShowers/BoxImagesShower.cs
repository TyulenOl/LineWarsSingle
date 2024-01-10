using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LineWars.Controllers;
using LineWars.LootBoxes;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class BoxImagesShower : MonoBehaviour
    {
        [SerializeField] private CardShower cardShowerPrefab;
        [SerializeField] private GoldShower goldShowerPrefab;
        [SerializeField] private UpgradeCardShower upgradeCardShowerPrefab;
        [SerializeField] private DiamondsShower diamondsShowerPrefab;
        
        [SerializeField] private List<Transform> transformsToInstantiateShowers;

        private List<LootedItemShower> activeShowers = new ();

        private void Awake()
        {
            if (transformsToInstantiateShowers.Count != 5)
                throw new InvalidDataException("BoxImagesShower have to have List<Transform> length only 5");
        }

        public void ShowItems(IEnumerable<ContextedDrop> drops)
        {
            var orderedDrops = drops.OrderBy(x => x.Drop.DropType).ToArray();
            if (orderedDrops.Length > 5)
                throw new NotImplementedException("Interface allows to implement case with max 5 elements");
            var lastTransformIndex = orderedDrops.Length - 1;
            foreach (var drop in orderedDrops)
            {
                var currentTransform = transformsToInstantiateShowers[lastTransformIndex];
                switch (drop.Drop.DropType)
                {
                    case LootType.Card:
                        ShowCard(currentTransform,GameRoot.Instance.CardsDatabase.IdToValue[drop.Drop.Value]);
                        break;
                    case LootType.UpgradeCard:
                        ShowUpgradeCard(currentTransform,GameRoot.Instance.CardsDatabase.IdToValue[drop.OldDrop.Value],drop.Drop.Value);
                        break;
                    case LootType.Gold:
                        ShowGold(currentTransform, drop.Drop.Value);
                        break;
                    case LootType.Diamond:
                        ShowDiamonds(currentTransform, drop.Drop.Value);
                        break;
                }
                lastTransformIndex--;
            }
        }

        private void ShowDiamonds(Transform parentTransform, int amount)
        {
            var instance = Instantiate(diamondsShowerPrefab, parentTransform);
            instance.ShowItem(amount);
            activeShowers.Add(instance);
        }
        
        private void ShowGold(Transform parentTransform, int amount)
        {
            var instance = Instantiate(goldShowerPrefab, parentTransform);
            instance.ShowItem(amount);
            activeShowers.Add(instance);
        }
        
        private void ShowUpgradeCard(Transform parentTransform, DeckCard deckCard, int amount)
        {
            var instance = Instantiate(upgradeCardShowerPrefab, parentTransform);
            instance.ShowItem(deckCard, amount);
            activeShowers.Add(instance);
        }
        
        private void ShowCard(Transform parentTransform, DeckCard deckCard)
        {
            var instance = Instantiate(cardShowerPrefab, parentTransform);
            instance.ShowItem(deckCard);
            activeShowers.Add(instance);
        }
        
        private void DestroyAllShowers()
        {
            foreach (var activeShower in activeShowers)
            {
                Destroy(activeShower.gameObject);
            }
        }

        private void OnDisable()
        {
            DestroyAllShowers();
        }
    }
}
