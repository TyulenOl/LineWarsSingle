using UnityEngine;

namespace LineWars.Interface
{
    public class TextRedrawInfo
    {
        public string Text;
        public Color Color;

        public TextRedrawInfo(string text, Color color)
        {
            Text = text;
            Color = color;
        }
    }
}