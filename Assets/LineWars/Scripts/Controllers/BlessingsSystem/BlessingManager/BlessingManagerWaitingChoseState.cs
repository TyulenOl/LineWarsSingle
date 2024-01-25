namespace LineWars.Model
{
    public partial class BlessingManager
    {
        public class BlessingManagerWaitingChoseState: BlessingManagerState
        {
            public BlessingManagerWaitingChoseState(
                BlessingManager blessingManager,
                BlessingManagerStateType blessingManagerStateType) : base(blessingManager, blessingManagerStateType)
            {
            }
        }
    }
        
}