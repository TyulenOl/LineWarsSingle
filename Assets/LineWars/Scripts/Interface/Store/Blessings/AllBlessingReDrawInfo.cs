using UnityEngine;

namespace LineWars.Interface
{
    public class AllBlessingReDrawInfo
    {
        public string Name { get; }
        public string Description { get; }
        public int Amount { get; }
        public Color BgColor { get; }
        public Sprite Sprite { get; }

        public AllBlessingReDrawInfo(string name, string description, Color bgColor, Sprite sprite, int amount)
        {
            Name = name;
            Description = description;
            BgColor = bgColor;
            Sprite = sprite;
            Amount = amount;
        }
    }
}