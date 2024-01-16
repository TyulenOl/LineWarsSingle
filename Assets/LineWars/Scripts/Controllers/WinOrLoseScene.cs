namespace LineWars.Controllers
{
    public static class WinOrLoseScene
    {
        public static bool IsWin { get; private set; }

        public static void Load(bool isWin)
        {
            IsWin = isWin;
            SceneTransition.LoadScene(SceneName.WinOrLoseScene);
        }
    }
}