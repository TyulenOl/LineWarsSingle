using LineWars.Model;

namespace LineWars.Controllers
{
    public interface ICommandTypeConstrain
    {
        public bool IsMyCommandType(CommandType commandType);
        
    }
}