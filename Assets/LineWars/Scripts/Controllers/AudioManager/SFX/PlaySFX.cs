using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Controllers
{
    public class PlaySFX : MonoBehaviour
    {
        [SerializeField] private SFXData sfx;

        public void Play()
        {
            SfxManager.Instance.Play(sfx);
        }
    }
}
