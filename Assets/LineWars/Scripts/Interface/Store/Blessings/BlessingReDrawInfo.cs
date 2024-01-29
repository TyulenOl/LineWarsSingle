using UnityEngine;

namespace LineWars
{
    public class BlessingReDrawInfo
    {
        private string name;
        private string description;
        private Color bgColor;
        private Sprite sprite;
        private int amount;

        public int Amont => amount;
        
        public string Name => name;

        public string Description => description;

        public Color BgColor => bgColor;

        public Sprite Sprite => sprite;

        public BlessingReDrawInfo(string name, string description, Color bgColor, Sprite sprite, int amount)
        {
            this.name = name;
            this.description = description;
            this.bgColor = bgColor;
            this.sprite = sprite;
            this.amount = amount;
        }
    }
}