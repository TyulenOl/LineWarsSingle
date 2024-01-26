using System;

namespace LineWars.Model
{
    public interface IBlessing
    {
        public event Action Completed;
        public bool CanExecute();
        public void Execute();
    }
}