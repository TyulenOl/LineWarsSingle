using System;
using AYellowpaper.SerializedCollections;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    [CreateAssetMenu]
    public class PromoCodeContainer: ScriptableObject
    {
        public SerializedDictionary<string, Prize> promoCodes;
    }
}