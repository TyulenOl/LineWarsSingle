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
            Instance._OpenPanel(title, description);
        }

        public static void OpenPanel(TextRedrawInfo title, Money money)
        {
            Instance._OpenPanel(title, money);
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