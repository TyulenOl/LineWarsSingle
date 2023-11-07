using UnityEngine;

namespace LineWars.Interface
{
    public class WinLoseUI : MonoBehaviour
    {
        public static bool isWin;
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject losePanel;

        private void Awake()
        {
            if (isWin)
            {
                winPanel.SetActive(true);
                losePanel.SetActive(false);
            }
            else
            {
                winPanel.SetActive(false);
                losePanel.SetActive(true);
            }
        }

        public void ToMainMenu() => SceneTransition.LoadScene(SceneName.MainMenu);
    }
}