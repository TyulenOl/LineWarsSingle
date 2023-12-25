using UnityEngine;

namespace LineWars.Model
{
    /// <summary>
    /// Если нужна механика гаранта,
    /// когда после некоторого количества попыток начинает выпадать нужный предмет,
    /// то нужно будет сохранять пользователькие данные,
    /// пока Лутбоксы будут независимо кидать лут от предыдущей истории.
    /// </summary>
    public abstract class LootBox: ScriptableObject
    {
        public abstract DeckCard GetCard();
    }
}