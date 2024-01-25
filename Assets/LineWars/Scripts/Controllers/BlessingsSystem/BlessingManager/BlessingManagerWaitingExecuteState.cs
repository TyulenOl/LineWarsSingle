namespace LineWars.Model
{
    public partial class BlessingManager
    {
        private class BlessingManagerWaitingExecuteState : BlessingManagerState
        {
            public BlessingManagerWaitingExecuteState(
                BlessingManager blessingManager, 
                BlessingManagerStateType blessingManagerStateType) : base(blessingManager, blessingManagerStateType)
            {
            }

            public override void OnEnter()
            {
                base.OnEnter();
                BlessingManager.commandsManager.Deactivate();
            }

            public override void OnExit()
            {
                base.OnExit();
                BlessingManager.commandsManager.Activate();
            }
        }
    }
}