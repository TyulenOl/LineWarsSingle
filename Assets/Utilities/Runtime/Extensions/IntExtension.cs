using System.Collections.Generic;

public static class IntExtension
{
    private static Dictionary<int, string> romanNumbers => new Dictionary<int, string>
    {
        {1, "I"},
        {2, "II"},
        {3, "III"},
        {4, "IV"},
        {5, "V"},
        {6,"VI"},
        {7,"VII"},
        {8,"VIII"},
        {9,"XI"},
        {10,"X"}
    };

    public static string GetRoman(this int value)
    {
        return romanNumbers[value];
    }
    public static int GetValueInRind(this int value, int module)
    {
        var temp = value % module;
        return temp < 0 ? temp + module : temp;
    }
}