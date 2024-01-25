namespace LineWars.Model
{
    public partial class BlessingManager
    {
        private class BlessingManagerIdleState : BlessingManagerState
        {
            public BlessingManagerIdleState(
                BlessingManager blessingManager,
                BlessingManagerStateType blessingManagerStateType) : base(blessingManager, blessingManagerStateType)
            {
            }
        }
    }
}