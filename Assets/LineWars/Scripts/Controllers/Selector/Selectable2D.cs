﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LineWars.Controllers
{
    public class Selectable2D: MonoBehaviour, IPointerClickHandler
    {
        ///<summary>
        /// Вызывается, когда на объект нажимают мышкой. Использовать для того, чтобы изменить присвоение селектора.
        ///<param name = "Selected Object"> Объект Selectable2D.</param>
        ///<param name = "Pointer Event Data">Информация о клике</param>
        ///<returns> Возвращаемое значение: Объект, выбранный Selector </returns>
        ///</summary>
        public event Func<GameObject, PointerEventData, GameObject> PointerClicked;

        private void OnValidate()
        {
            if (GetComponent<Collider2D>() == null)
                Debug.LogWarning($"У {gameObject.name} нет коллайдера!");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(PointerClicked != null)
            {
                Selector.SelectedObject = PointerClicked.Invoke(this.gameObject, eventData);
            }
            else
            {
                Selector.SelectedObject = gameObject;
            }
        }


    }
}