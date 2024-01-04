using System.Collections;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class UnitTracker : MonoBehaviour
    {
        [SerializeField] private Image unitImage;
        [SerializeField] private Image BgImage;
        [SerializeField] private Button button;
        [SerializeField] private CanvasGroup canvasGroup;
        
        private Unit unit;
        
        public void Init(Unit unit)
        {
            this.unit = unit;
            unit.Died.AddListener(OnDelete);
            unit.ActionPointsChanged.AddListener(OnApChanged);
            unitImage.sprite = unit.Sprite;
            button.onClick.AddListener(OnClick);
        }

        private void OnApChanged(int old, int now)
        {
            if (now == 0)
            {
                BgImage.color = Color.red;
                unitImage.color = Color.gray;
                transform.SetAsLastSibling();
                button.interactable = false;
            }
            
            if(old == 0)
            {
                unitImage.color = Color.white;
                BgImage.color = Color.white;
                button.interactable = true;
            }
        }
        
        private void OnClick()
        {
            Debug.Log(unit.UnitName);
        }
        
        private void OnDelete(Unit arg0)
        {
            StartCoroutine(DestroyCoroutine());
        }

        private IEnumerator DestroyCoroutine()
        {
            var instance = Instantiate(this, MainCanvas.Instance.transform);
            instance.transform.position = transform.position;
            canvasGroup.alpha = 0;
            instance.canvasGroup.alpha = 100;
            for (int i = 0; i < 200; i++)
            {
                instance.transform.localPosition += Vector3.up * 0.2f;
                instance.canvasGroup.alpha -= 0.005f;
                yield return null;
            }
            Destroy(gameObject);
            Destroy(instance.gameObject);
        }
    }
}