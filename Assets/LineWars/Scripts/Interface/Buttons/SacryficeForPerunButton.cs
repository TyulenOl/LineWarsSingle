using LineWars.Controllers;
using LineWars.Model;


namespace LineWars.Interface
{
    public class SacryficeForPerunButton : ActionButtonLogic
    {
        protected override void OnClick()
        {
            if (CommandsManager.CanSelectCurrentCommand(CommandType.SacrificePerun))
                CommandsManager.SelectCurrentCommand(CommandType.SacrificePerun);
        }

        protected override void CommandsManagerOnEnter(CommandsManagerStateType type)
        {
            base.CommandsManagerOnEnter(type);
            button.interactable = CommandsManager.CanSelectCurrentCommand(CommandType.SacrificePerun);
        }
    }
}
