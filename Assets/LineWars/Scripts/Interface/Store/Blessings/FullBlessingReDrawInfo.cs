using UnityEngine;

namespace LineWars.Interface
{
    public class FullBlessingReDrawInfo
    {
        public string Name { get; set; }
        public string Description { get; set;}
        public int Amount { get; set;}
        public Color BgColor { get; set;}
        public Sprite Sprite { get; set;}

        public FullBlessingReDrawInfo(string name, string description, Color bgColor, Sprite sprite, int amount)
        {
            Name = name;
            Description = description;
            BgColor = bgColor;
            Sprite = sprite;
            Amount = amount;
        }
    }
}