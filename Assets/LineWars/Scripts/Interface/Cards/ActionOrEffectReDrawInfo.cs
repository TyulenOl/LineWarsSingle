using UnityEngine;

namespace LineWars
{
    public class ActionOrEffectReDrawInfo
    {
        public string Name { get; }
        public string Description { get; }
        public Sprite Sprite { get; }

        public ActionOrEffectReDrawInfo(Sprite sprite, string name, string description)
        {
            this.Sprite = sprite;
            this.Name = name;
            this.Description = description;
        }
    }
}