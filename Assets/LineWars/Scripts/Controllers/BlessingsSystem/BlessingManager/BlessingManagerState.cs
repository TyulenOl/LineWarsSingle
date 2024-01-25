namespace LineWars.Model
{
    partial class BlessingManager
    {
        public abstract class BlessingManagerState: State
        {
            protected readonly BlessingManager BlessingManager;
            protected readonly BlessingManagerStateType BlessingManagerStateType;
            
            protected BlessingManagerState(
                BlessingManager blessingManager,
                BlessingManagerStateType blessingManagerStateType)
            {
                BlessingManager = blessingManager;
                BlessingManagerStateType = blessingManagerStateType;
            }

            public override void OnEnter()
            {
                base.OnEnter();
                BlessingManager.stateType = BlessingManagerStateType;
            }
        }
    }
}