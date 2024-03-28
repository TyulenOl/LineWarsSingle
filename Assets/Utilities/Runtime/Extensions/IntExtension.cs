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
        {10,"X"},
        {11, "XI"},
        {12, "XII" },
        {13, "XIII" },
        {14, "XIV" },
        {15, "XV" },
        {16, "XVI" },
        {17, "XVII" },
        {18, "XVIII" },
        {19, "XIX" },
        {20, "XX" }
    };

    public static string GetRoman(this int value)
    {
        if (!romanNumbers.ContainsKey(value))
            return value.ToString();
        return romanNumbers[value];
    }
    public static int GetValueInRind(this int value, int module)
    {
        var temp = value % module;
        return temp < 0 ? temp + module : temp;
    }

    public static string ToText(this int value)
    {
        switch (value)
        {
            case 0:
                return "Ноль";
            case 1:
                return "Один";
            case 2:
                return "Два";
            case 3:
                return "Три";
            case 4:
                return "Четыре";
            case 5:
                return "Пять";
            case 6:
                return "Шесть";
            case 7:
                return "Семь";
            case 8:
                return "Восемь";
            case 9:
                return "Девять";
            case 10:
                return "Десять";
            case 11:
                return "Одиннадцать";
            case 12:
                return "Двенадцать";
            case 13:
                return "Тринадцать";
            case 14:
                return "Четырнадцать";
            case 15:
                return "Пятнадцать";
            case 16:
                return "Шестнадцать";
            case 17:
                return "Семнадцать";
            case 18:
                return "Восемнадцать";
            default:
                return value.ToString() + "Incorrect";
        }
    }
    /// <summary>
    /// Возвращает слова в падеже, зависимом от заданного числа
    /// </summary>
    /// <param name="number">Число от которого зависит выбранное слово</param>
    /// <param name="nominativ">Именительный падеж слова. Например "день"</param>
    /// <param name="genetiv">Родительный падеж слова. Например "дня"</param>
    /// <param name="plural">Множественное число слова. Например "дней"</param>
    /// <returns></returns>
    public static string Pluralize(this int number, string nominativ, string genetiv, string plural) {
        var titles = new[] {nominativ, genetiv, plural};
        var cases = new[] {2, 0, 1, 1, 1, 2};
        return titles[number % 100 > 4 && number % 100 < 20 ? 2 : cases[(number % 10 < 5) ? number % 10 : 5]];
    }
    
    public static string ToChars(this int d, int charsAmount)
    {
        var str = d.ToString();
        if (str.Length >= charsAmount) return str;
        return new string('0', charsAmount - str.Length) + str;
    }
}