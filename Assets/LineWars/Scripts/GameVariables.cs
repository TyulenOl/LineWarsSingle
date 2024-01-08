using UnityEngine.SceneManagement;

namespace LineWars
{
    public static class GameVariables
    {
        public static bool IsNormalStart { get; }

        static GameVariables()
        {
            var currentScene = (SceneName) SceneManager.GetActiveScene().buildIndex;
            if (currentScene == SceneName.MainMenu)
                IsNormalStart = true;
        }

        public static void Initialize()
        {
            // триггер статического конструктора
        }
    }
}