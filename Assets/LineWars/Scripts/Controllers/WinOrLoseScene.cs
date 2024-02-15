namespace LineWars.Controllers
{
    public static class WinOrLoseScene
    {
        public static bool IsWin { get; private set; }
        public static int GoldAmount { get; private set; }
        public static int DiamondsAmount { get; private set; }
        public static bool CanDoublicateGold { get; private set; }

        public static void Load(
            bool isWin,
            int goldAfterBattle,
            int diamondsAfterBattle,
            bool canDoublicateGold)
        {
            IsWin = isWin;
            GoldAmount = goldAfterBattle;
            DiamondsAmount = diamondsAfterBattle;
            CanDoublicateGold = canDoublicateGold;
            
            SceneTransition.LoadScene(SceneName.WinOrLoseScene);
        }
    }
}