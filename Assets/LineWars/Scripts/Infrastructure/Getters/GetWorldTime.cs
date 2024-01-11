using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace LineWars.Model
{
    public class GetWorldTime : MonoBehaviour, 
        IGetter<DateTime>
    {
        private bool isLoaded;
        private DateTime loadedTime;
        private readonly string apiUrl = "https://worldtimeapi.org/api/timezone/Europe/Moscow";
        
        public void Start()
        {
            StartCoroutine(LoadTimeCoroutine());
        }

        public bool CanGet()
        {
            return isLoaded;
        }

        public DateTime Get()
        {
            if (!isLoaded)
                throw new InvalidOperationException("Can't get time yet!");

            return loadedTime;
        }

        IEnumerator LoadTimeCoroutine()
        {
            using (var request = UnityWebRequest.Get(apiUrl))
            {
                yield return request.SendWebRequest();

                if(request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Couldn't load time: {request.error}. Loading \"Special Sliva's Day\"");
                    loadedTime = new DateTime(2003, 7, 19);
                    isLoaded = true;
                    yield break;
                }

                var requestResult = request.downloadHandler.text;
                var response = JsonUtility.FromJson<WorldTimeAPIResponse>(requestResult);
                loadedTime = ParseDateTime(response.datetime);
                isLoaded = true;
            }
        }

        DateTime ParseDateTime(string datetime)
        {
            string date = Regex.Match(datetime, @"^\d{4}-\d{2}-\d{2}").Value;
            string time = Regex.Match(datetime, @"\d{2}:\d{2}:\d{2}").Value;

            return DateTime.Parse(string.Format("{0} {1}", date, time));
        }

        private class WorldTimeAPIResponse
        {
            public string datetime;
        }
    }
}
