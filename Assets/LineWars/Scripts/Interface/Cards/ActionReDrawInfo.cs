using UnityEngine;

namespace LineWars
{
    public class ActionReDrawInfo
    {
        private readonly Sprite sprite;
        private readonly string name;
        private readonly string description;

        public Sprite Sprite => sprite;

        public string Name => name;

        public string Description => description;

        public ActionReDrawInfo(Sprite sprite, string name, string description)
        {
            this.sprite = sprite;
            this.name = name;
            this.description = description;
        }
    }
}