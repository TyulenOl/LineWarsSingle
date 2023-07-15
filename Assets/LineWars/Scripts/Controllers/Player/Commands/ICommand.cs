using LineWars.Model;

namespace LineWars
{
    //команды нужены, чтобы сохранять контекст,
    //реализация ее должна находиться на уровне ниже
    public interface ICommand
    {
        public void Execute();
        public bool CanExecute();
    }
}