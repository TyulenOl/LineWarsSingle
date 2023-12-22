using UnityEngine;

namespace LineWars.Model
{
    public static class JSONSaverUtilities
    {
        public static string PathGenerator(string objectName, int id)
        {
            return Application.persistentDataPath +
                $"/{objectName}/" +
                $"{objectName}_{id}.json";
        }
    }
}
