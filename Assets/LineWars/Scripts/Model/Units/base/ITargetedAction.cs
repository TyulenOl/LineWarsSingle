namespace LineWars.Model
{
    public interface ITargetedAction
    {
        public bool IsMyTarget(ITarget target);
        public ICommand GenerateCommand(ITarget target);
    }
}