using UnityEngine;

namespace LineWars
{
    public class ActionReDrawInfo
    {
        public string Name { get; }
        public string Description { get; }
        public Sprite Sprite { get; }

        public ActionReDrawInfo(Sprite sprite, string name, string description)
        {
            this.Sprite = sprite;
            this.Name = name;
            this.Description = description;
        }
    }
}