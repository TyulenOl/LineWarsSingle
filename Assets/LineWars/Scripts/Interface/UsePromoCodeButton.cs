using LineWars.Controllers;
using LineWars.Infrastructure;

namespace LineWars.Interface
{
    public class UsePromoCodeButton: ButtonClickHandler
    {
        protected override void OnClick()
        {
            if (GameRoot.Instance?.SdkAdapter != null)
                UIPanel.OpenPromoCodePanel(GameRoot.Instance.SdkAdapter.UsePromoCode);
        }
    }
}