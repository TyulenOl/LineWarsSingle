using System.Collections;
using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class TargetDrawer : MonoBehaviour
    {
        [field: SerializeField] public SpriteRenderer image { get; set; }

        public virtual void ReDrawCommads(CommandType commandType)
        {
            image.enabled = commandType != CommandType.None;
            image.sprite = DrawHelper.GetOrderIconByCommandType(commandType);
        }
    }
}