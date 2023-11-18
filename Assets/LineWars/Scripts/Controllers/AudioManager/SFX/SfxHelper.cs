using LineWars.Model;

namespace LineWars.Controllers
{
    public static class SfxHelper
    {
        public static void PlaySfx(this Unit unit, SFXData data)
        {
            if (unit.IsVisible())
            {
                SfxManager.Instance.Play(data);
            }
        }
    }
}