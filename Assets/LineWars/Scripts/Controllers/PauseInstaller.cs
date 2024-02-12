using System;
using DataStructures;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Utilities.Runtime;

namespace LineWars
{
    public class PauseInstaller: Singleton<PauseInstaller>
    {
        [Flags]
        public enum PauseType
        {
            AudioPause = 1,
            TimeScalePause = 2,
            CursorActivity = 4,
            EventSystemLock = 8
        }

        public enum PauseMethod
        {
            RememberPreviousState,
            CustomState
        };
        
        public enum CursorVisible
        {
            [InspectorName("Show Cursor")] Show,
            [InspectorName("Hide Cursor")] Hide
        };

        [Serializable]
        public class ClosingADValues
        {
            [Tooltip("Значение временной шкалы при закрытии")]
            public float timeScale = 1;

            [Tooltip("Значение аудио паузы при закрытии")]
            public bool audioPause;

            [Tooltip("Показать или скрыть курсор при закрытии")]
            public CursorVisible cursorVisible;

            [Tooltip("Выберите мод блокировки курсора при закрытии")]
            public CursorLockMode cursorLockMode;
        }

        [Serializable]
        public class CustomEvents
        {
            public UnityEvent OpenAd;
            public UnityEvent CloseAd;
        }


        [Tooltip(
            "Данный скрипт будет ставить звук или верменную шкалу на паузу взависимости от выбранной настройки Pause Type." +
             "\n •  Audio Pause - Ставить звук на паузу." +
             "\n •  Time Scale Pause - Останавливать время." +
             "\n •  Cursor Activity - Скрывать курсор." +
             "\n •  All - Ставить на паузу и звук и время." +
             "\n •  Nothing To Control - Не контролировать никакие параметры (подпишите свои методы в  Custom Events)."
        )]
        public PauseType pauseType;
        
        [Tooltip(
             "RememberPreviousState - Ставить паузу при открытии. После закрытия звук, временная шкала, курсор - придут в изначальное значение." +
             "\n CustomState - Укажите свои значения, которые будут выставляться при открытии и закрытии"
        )]
        public PauseMethod pauseMethod;

        [Tooltip("Установите значения при закрытии рекламы")]
        [ConditionallyVisible(nameof(pauseMethod))]
        public ClosingADValues closingADValues;

        [SerializeField, 
         Tooltip("Установить значения в методе Awake (то есть при старте сцены)." +
                 "\nЭто позволит не прописывать события вроде аудио пауза = false или timeScale = 1 в ваших скриптах в методах Awake или Start, что позволит убрать путаницу.")] 
        private bool awakeSetValues;
        [SerializeField, ConditionallyVisible(nameof(awakeSetValues)), Tooltip("Установите значения, которые применятся в методе Awake.")]
        private ClosingADValues awakeValues;

        [Tooltip("Ивенты для выполнения собственных методов. Вызываются при открытии или закрытии любой рекламы.")]
        public CustomEvents customEvents;

        [SerializeField]
        private bool logPause;

        private static bool audioPauseOnAd;
        private static float timeScaleOnAd;
        private static bool cursorVisibleOnAd;
        private static CursorLockMode cursorLockModeOnAd;
        private static bool start;
        private static bool awaitingClosure;
        private EventSystem eventSystem;

        protected override void Awake()
        {
            base.Awake();
            if (awakeSetValues)
            {
                audioPauseOnAd = awakeValues.audioPause;
                timeScaleOnAd = awakeValues.timeScale;
                cursorVisibleOnAd = awakeValues.cursorVisible == CursorVisible.Show ? true : false;
                cursorLockModeOnAd = awakeValues.cursorLockMode;
                start = true;

                ClosingADValues closingValuesOrig = closingADValues;
                closingADValues = awakeValues;
                Pause(false);
                closingADValues = closingValuesOrig;
            }
        }

        private void Start()
        {
            if (!start)
            {
                start = true;
                audioPauseOnAd = AudioListener.pause;
                timeScaleOnAd = Time.timeScale;
                cursorVisibleOnAd = Cursor.visible;
                cursorLockModeOnAd = Cursor.lockState;
            }
        }

        public static void Pause(bool pause)
        {
            Instance?._Pause(pause);
        }
        
        private void _Pause(bool pause)
        {
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
                if (pauseMethod == PauseMethod.CustomState)
                {
                    if (pause) AudioListener.pause = true;
                    else AudioListener.pause = closingADValues.audioPause;
                }
                else
                {
                    if (pause)
                    {
                        audioPauseOnAd = AudioListener.pause;
                        AudioListener.pause = true;
                    }
                    else AudioListener.pause = audioPauseOnAd;
                }
            }

            if (pauseType.HasFlag(PauseType.TimeScalePause))
            {
                if (pauseMethod == PauseMethod.CustomState)
                {
                    if (pause) Time.timeScale = 0;
                    else Time.timeScale = closingADValues.timeScale;
                }
                else
                {
                    if (pause)
                    {
                        timeScaleOnAd = Time.timeScale;
                        Time.timeScale = 0;
                    }
                    else Time.timeScale = timeScaleOnAd;
                }
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
                    if (pauseMethod == PauseMethod.CustomState)
                    {
                        if (closingADValues.cursorVisible == CursorVisible.Hide)
                            Cursor.visible = false;
                        else Cursor.visible = true;

                        Cursor.lockState = closingADValues.cursorLockMode;
                    }
                    else
                    {
                        Cursor.visible = cursorVisibleOnAd;
                        Cursor.lockState = cursorLockModeOnAd;
                    }
                }
            }

            if (pause) 
                customEvents.OpenAd.Invoke();
            else 
                customEvents.CloseAd.Invoke();
        }
    }
}