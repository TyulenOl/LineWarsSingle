using LineWars.Model;

namespace LineWars
{
    //команды нужны, чтобы сохранять контекст,
    //реализация ее должна находиться на уровне ниже
    public interface ICommand
    {
        public void Execute();
        public bool CanExecute();
    }
}