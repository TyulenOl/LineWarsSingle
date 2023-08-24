using System.Collections;
using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

public class TargetDrawer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer image;
    public void ReDraw(CommandType commandType)
    {
        image.gameObject.SetActive(commandType == CommandType.None);
        image.sprite = DrawHelper.GetSpriteByCommandType(commandType);
    }
}
