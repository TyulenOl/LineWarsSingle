using LineWars.Model;

namespace LineWars
{
    // нужен, если мы захотим ввести систему логгирования
    public class Invoker
    {
        public void Execute(ICommand command)
        {
            command.Execute();
        }
    }
}

