using System;
using DataStructures;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace LineWars
{
    [Flags]
    public enum PauseType
    {
        AudioPause = 1,
        TimeScalePause = 2,
        CursorActivity = 4,
        EventSystemLock = 8,
        All = AudioPause | TimeScalePause | CursorActivity | EventSystemLock
    }
    
    public class PauseInstaller: Singleton<PauseInstaller>
    {
        [SerializeField] private BoolEventChannel raisedPauseSetChannel;
        [SerializeField] private PauseType pauseType;

        [SerializeField] private bool logPause;

        private static bool audioPauseOnAd;
        private static float timeScaleOnAd;
        private static bool cursorVisibleOnAd;
        private static CursorLockMode cursorLockModeOnAd;
        private static bool start;
        private static bool awaitingClosure;
        private EventSystem eventSystem;
        public static bool Paused { get; private set; }

        private void Start()
        {
            if (start) return;
            start = true;
            audioPauseOnAd = AudioListener.pause;
            timeScaleOnAd = Time.timeScale;
            cursorVisibleOnAd = Cursor.visible;
            cursorLockModeOnAd = Cursor.lockState;
        }

        public static void Pause(bool pause)
        {
            if (Instance != null)
                Instance._Pause(pause);
        }
        
        public static void Pause(PauseType pauseType, bool pause)
        {
            if (Instance != null)
                Instance._Pause(pauseType, pause);
        }
        
        private void _Pause(bool pause)
        {
            _Pause(pauseType, pause);
        }
        private void _Pause(PauseType pauseType, bool pause)
        {
            Paused = pause;
            
            if (logPause)
                Debug.Log("Pause game: " + pause);

            if (pauseType.HasFlag(PauseType.EventSystemLock))
            {
                if (pause)
                {
                    if (awaitingClosure)
                        return;
                    awaitingClosure = true;

                    if (!eventSystem)
                        eventSystem = FindObjectOfType<EventSystem>();
                    if (eventSystem)
                        eventSystem.enabled = false;
                }
                else
                {
                    awaitingClosure = false;

                    if (!eventSystem)
                        eventSystem = FindObjectOfType<EventSystem>();
                    if (eventSystem)
                        eventSystem.enabled = true;
                }
            }
            
            if (pauseType.HasFlag(PauseType.AudioPause))
            {
                if (pause)
                {                   
                    audioPauseOnAd = AudioListener.pause;
                    AudioListener.pause = true;
                    raisedPauseSetChannel.RaiseEvent(true);
                }
                else
                {
                    AudioListener.pause = audioPauseOnAd;
                    raisedPauseSetChannel.RaiseEvent(false);
                }
            }

            if (pauseType.HasFlag(PauseType.TimeScalePause))
            {
                if (pause)
                {
                    timeScaleOnAd = Time.timeScale;
                    Time.timeScale = 0;
                }
                else Time.timeScale = timeScaleOnAd;
            }

            if (pauseType.HasFlag(PauseType.CursorActivity))
            {
                if (pause)
                {
                    cursorVisibleOnAd = Cursor.visible;
                    cursorLockModeOnAd = Cursor.lockState;

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.visible = cursorVisibleOnAd;
                    Cursor.lockState = cursorLockModeOnAd;
                }
            }
        }
    }
}