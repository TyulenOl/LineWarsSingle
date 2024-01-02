using LineWars.Controllers;
using LineWars.Model;


namespace LineWars.Interface
{
    public class SacryficeForPerunButton : ActionButtonLogic
    {
        protected override void OnClick()
        {
            CommandsManager.Instance.SelectCurrentCommand(CommandType.SacrificePerun);
        }
    }
}
