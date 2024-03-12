using System.Linq;

public static class FloatExtension
{
    public static string ToChars(this float d, int charsAmount)
    {
        var str = ((int)d).ToString();
        var result = "";
        if (str.Length >= charsAmount) return str;
        for (int i = 0; i < charsAmount - str.Length; i++)
        {
            result += "0";
        }
        return result + str;
    }
}
