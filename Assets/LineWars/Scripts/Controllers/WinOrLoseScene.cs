namespace LineWars.Controllers
{
    public static class WinOrLoseScene
    {
        public static bool IsWin { get; private set; }
        public static int MoneyAmount { get; private set; }
        public static int DiamondsAmount { get; private set; }

        public static void Load(bool isWin, int moneyAfterBattle, int diamondsAfterBattle)
        {
            IsWin = isWin;
            MoneyAmount = moneyAfterBattle;
            DiamondsAmount = diamondsAfterBattle;
            SceneTransition.LoadScene(SceneName.WinOrLoseScene);
        }
    }
}