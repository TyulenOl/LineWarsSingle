namespace LineWars
{
    // нужен, если мы захотим ввести систему логгирования
    public class Invoker
    {
        public void Execute(ICommand command)
        {
            //if (command.CanExecute())
                command.Execute();
        }
    }
}

