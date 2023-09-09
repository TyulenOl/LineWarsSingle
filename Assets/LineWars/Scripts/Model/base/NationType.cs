using System;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public enum NationType
    {
        Default,
        Russia,
        Deutschland,
        GreatBritain
    }

    public static class NationHelper
    {
        private static Dictionary<NationType, string> nationNames = new()
        {
            {NationType.Default, "По-умолчанию"},
            {NationType.Russia, "Россия"},
            {NationType.Deutschland, "Германия"},
            {NationType.GreatBritain, "Британия"}
        };
        
        public static string GetNationName(NationType type)
        {
            if (nationNames.TryGetValue(type, out var name))
                return name;
            return "Неизвестная страна";
        }
    }
}