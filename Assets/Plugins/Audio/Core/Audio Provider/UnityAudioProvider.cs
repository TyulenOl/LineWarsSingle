﻿using System.Collections;
using Plugins.Audio.Utils;
using UnityEngine;

namespace Plugins.Audio.Core
{
    public class UnityAudioProvider : BaseUnityAudioProvider
    {
        private readonly SourceAudio _sourceAudio;
        private readonly AudioSource _unitySource;
        
        public override float Volume
        {
            get => _unitySource.volume;
            set => _unitySource.volume = value;
        }

        public override bool Mute
        {
            get => _unitySource.mute;
            set => _unitySource.mute = value;
        }

        public override bool Loop
        {
            get => _unitySource.loop;
            set => _unitySource.loop = value;
        }
        
        public override float Pitch
        {
            get => _unitySource.pitch;
            set => _unitySource.pitch = value;
        }

        public override float Time 
        { 
            get => _unitySource.time;
            set => _unitySource.time = value;
        }
        
        public override bool IsPlaying => _unitySource.isPlaying;

        private Coroutine _playRoutine;
        private AudioClip _clip;
        private bool _loadClip;

        private bool _beginPlaying;
        private bool _isFocus = true;
        private float _lastTime;

        public UnityAudioProvider(SourceAudio sourceAudio)
        {
            _sourceAudio = sourceAudio;
            _unitySource = CreateAudioSource(_sourceAudio);
        }

        public override void RefreshSettings(SourceAudio.AudioSettings settings)
        {
            base.RefreshSettings(settings);

            _unitySource.SetData(settings);
        }

        public override void Play(string key, float time)
        {
            if (string.IsNullOrEmpty(key))
            {
                AudioManagement.Instance.LogError("key is empty, Source Audio PlaySound: " + _sourceAudio.name);
                return;
            }
            
            if (_playRoutine != null)
            {
                _sourceAudio.StopCoroutine(_playRoutine);
            }
            
            _playRoutine = _sourceAudio.StartCoroutine(PlayRoutine(key, time));
        }

        public override void PlayOneShot(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                AudioManagement.Instance.LogError("key is empty, Source Audio PlaySound: " + _sourceAudio.name);
                return;
            }

            _playRoutine = _sourceAudio.StartCoroutine(PlayOneShotRoutine(key));
        }

        public override void Stop()
        {
            _unitySource.Stop();
            _beginPlaying = false;
            _lastTime = 0;
            Time = 0;
        }
        
        public override void Pause()
        {
            _unitySource.Pause();
        }

        public override void UnPause()
        {
            _unitySource.UnPause();
        }

        private IEnumerator PlayRoutine(string key, float time)
        {
            _beginPlaying = false;
            _loadClip = true;
            _clip = null;
            
            yield return AudioManagement.Instance.GetClip(key, audioClip => _clip = audioClip);

            if (_clip == null)
            {
                AudioManagement.Instance.LogError("Audio Management not found clip at key: " + key + ",\n Source Audio PlaySound: " + _sourceAudio.name);

                yield break;
            }

            _unitySource.clip = _clip;
            _unitySource.Play();
            _loadClip = false;
            _beginPlaying = true;

            _unitySource.time = time;
            _lastTime = time;
        }

        private IEnumerator PlayOneShotRoutine(string key)
        {
            AudioClip _clip = null;
            
            yield return AudioManagement.Instance.GetClip(key, audioClip => _clip = audioClip);

            if (_clip == null)
            {
                AudioManagement.Instance.LogError("Audio Management not found clip at key: " + key + ",\n Source Audio PlaySound: " + _sourceAudio.name);

                yield break;
            }

            _unitySource.PlayOneShot(_clip);
        }
        
        public override void Update()
        {
            CheckFinished();
        }

        private void CheckFinished()
        {
            if (_clip == null || _loadClip)
            {
                return;
            }
            
            if (_unitySource.time <= 0 && _unitySource.isPlaying == false && _beginPlaying)
            {
                _beginPlaying = false;
                _sourceAudio.ClipFinished();
            }
        }

        public override void OnGlobalAudioUnpaused()
        {
            if (_isFocus == false && _beginPlaying && _lastTime > 0)
            {
                _unitySource.time = _lastTime;

                AudioManagement.Instance.Log(_sourceAudio.CurrentKey + " Last Time: " + _lastTime);
            }
            
            _isFocus = true;
        }

        public override void OnGlobalAudioPaused()
        {
            if (_isFocus && _beginPlaying)
            {
                _lastTime = _unitySource.time;
                AudioManagement.Instance.Log(_sourceAudio.CurrentKey + "Set Last Time: " + _lastTime);
            }
            
            _isFocus = false;
        }
    }
}