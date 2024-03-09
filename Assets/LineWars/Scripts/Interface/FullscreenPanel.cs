using DataStructures;
using LineWars.Model;
using TMPro;
using UnityEngine;

namespace LineWars.Interface
{
    public class FullscreenPanel: Singleton<FullscreenPanel>
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;
        [SerializeField] private MoneyDrawerSimple moneyDrawer;
        [SerializeField] private CanvasGroup canvasGroup;

        public static void OpenPanel(TextRedrawInfo title, TextRedrawInfo description)
        {
            if (Instance != null)
                Instance._OpenPanel(title, description);
            else
                Debug.LogError($"{nameof(FullscreenPanel)} is null!");
        }

        public static void OpenPanel(TextRedrawInfo title, Money money)
        { 
            if (Instance != null)
                Instance._OpenPanel(title, money);
            else
                Debug.LogError($"{nameof(FullscreenPanel)} is null!");
        }

        public static void OpenErrorPanel()
        {
            OpenPanel(
                new TextRedrawInfo("Ой-ой", Color.white),
                new TextRedrawInfo("произошла ошибка", Color.white)
            );
        }
        
        public static void OpenSuccessPanel(Money money)
        {
            OpenPanel(new TextRedrawInfo("Успешно", Color.red), money);
        }
        
        public static void OpenSuccessPanel(Prize prize)
        {
            OpenPanel(new TextRedrawInfo("Успешно", Color.red), new Money(prize.Type.ToCostType(), prize.Amount));
        }

        public static void OpenPromoCodePanel(string promoCode)
        {
            OpenPanel(
                new TextRedrawInfo("Активация промокода", Color.red),
                new TextRedrawInfo($"Вы успешно активировали промокод {promoCode}", Color.white)
            );
        }

        public static void OpenSuccessLockAdPanel()
        {
            OpenPanel(
                new TextRedrawInfo("Отключение рекламы прошло успешно", Color.red),
                new TextRedrawInfo($"Вы успешно отключили рекламу", Color.white));
        }
        
        private void _OpenPanel(TextRedrawInfo title, TextRedrawInfo description)
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            if (this.title)
            {
                this.title.text = title.Text;
                this.title.color = title.Color;
                this.title.gameObject.SetActive(true);
            }
            if (this.description)
            {
                this.description.text = description.Text;
                this.description.color = description.Color;
                this.description.gameObject.SetActive(true);
            }
            if(moneyDrawer)
            {
                moneyDrawer.gameObject.SetActive(false);
            }
        }

        private void _OpenPanel(TextRedrawInfo title, Money money)
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            if (this.title)
            {
                this.title.text = title.Text;
                this.title.color = title.Color;
                this.title.gameObject.SetActive(true);
            }
            if (this.description)
            {
                this.description.gameObject.SetActive(false);
            }
            if(moneyDrawer)
            {
                moneyDrawer.Redraw(money);
                moneyDrawer.gameObject.SetActive(true);
            }
        }
    }
}