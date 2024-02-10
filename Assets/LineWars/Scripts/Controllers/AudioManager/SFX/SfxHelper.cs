using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public static class SfxHelper
    {
        public static void PlaySfx(this Unit unit, SFXData data)
        {
            if(data == null)
            {
                Debug.LogWarning("data is null! " + unit.name);
                return;
            }
            if(data.Clip == null)
            {
                Debug.LogWarning("Clip is null! " + unit.name + " " + data.name);
            }
            if (unit.IsVisible)
            {
                SfxManager.Instance.Play(data);
            }
        }
    }
}