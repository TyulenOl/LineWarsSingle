using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Interface
{
    public class WinLoseUI : MonoBehaviour
    {
        private static bool IsWin => WinOrLoseScene.IsWin;
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject losePanel;

        private void Awake()
        {
            if (IsWin)
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