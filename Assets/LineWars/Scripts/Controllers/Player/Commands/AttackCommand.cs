using LineWars.Model;

namespace LineWars
{
    public class AttackCommand: ICommand
    {
        private readonly IHitCreator hitCreator;
        private readonly IHitHandler hitHandler;

        public AttackCommand(IHitCreator hitCreator, IHitHandler hitHandler)
        {
            this.hitCreator = hitCreator;
            this.hitHandler = hitHandler;
        }

        public void Execute()
        {
            hitHandler.Accept(hitCreator.GenerateHit());
        }

        public bool CanExecute()
        {
            throw new System.NotImplementedException();
        }
    }
}