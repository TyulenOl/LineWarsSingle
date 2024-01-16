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

        private bool isActive;
        
        public void Init(Unit unit)
        {
            this.unit = unit;
            unit.Died.AddListener(OnDelete);
            unit.ActionPointsChanged.AddListener(OnApChanged);
            unitImage.sprite = unit.Sprite;
            button.onClick.AddListener(OnClick);
            CommandsManager.Instance.ExecutorChanged += OnExecutorChanged;
            CommandsManager.Instance.CommandIsExecuted += OnCommandExecuted;
            isActive = true;
        }

        private void OnCommandExecuted(ICommand obj)
        {
            if(!isActive)
                return;
            var inter = false;
            if (CommandsManager.Instance.Executor as Unit == unit)
                inter = true;
            else
            {
                inter = CommandsManager.Instance.Executor as Unit != unit && CommandsManager.Instance.CanSetExecutor();
            }
            BgImage.color = inter ? Color.white : Color.gray;
            button.interactable = inter;
        }

        private void OnExecutorChanged(IMonoExecutor arg1, IMonoExecutor arg2)
        {
            var isSelected = unit.Equals(arg2);
            if (isSelected)
            {
                BgImage.color = Color.green; 
            }
            else
            {
                if (unit.CurrentActionPoints != 0)
                {
                    BgImage.color = Color.white;
                    button.interactable = true;
                }
                    
            }
        }

        private void OnApChanged(int old, int now)
        {
            if (now == 0)
            {
                BgImage.color = Color.red;
                unitImage.color = Color.gray;
                transform.SetAsLastSibling();
                button.interactable = false;
                isActive = false;
            }
            
            if(old == 0)
            {
                unitImage.color = Color.white;
                BgImage.color = Color.white;
                button.interactable = true;
                isActive = true;
            }
        }
        
        
        
        
        
        private void OnClick()
        {
            if (!CommandsManager.Instance.CanSetExecutor()) return;
            CameraController.Instance.MoveTo(unit.Node.Position);
            CommandsManager.Instance.SetExecutor(unit);
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