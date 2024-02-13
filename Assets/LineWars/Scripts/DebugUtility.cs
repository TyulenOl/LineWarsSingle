using UnityEngine;

namespace LineWars
{
    public static class DebugUtility
    {
        public static bool Logged = true;
        
        public static void Log(object massage)
        {
            if (Logged)
                Debug.Log(massage);
        }
        
        public static void Log(object massage, Object context)
        {
            if (Logged)
                Debug.Log(massage, context);
        }
        
        public static void LogWarning(object massage)
        {
            if (Logged)
                Debug.LogWarning(massage);
        }
        
        public static void LogWarning(object massage, Object context)
        {
            if (Logged)
                Debug.LogWarning(massage, context);
        }
        
        public static bool LogError(object massage)
        {
            if (Logged)
                Debug.LogError(massage);
            return false;
        }
        
        public static bool LogError(object massage, Object context)
        {
            if (Logged)
                Debug.LogError(massage, context);
            return false;
        }
    }
}