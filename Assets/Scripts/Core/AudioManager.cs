using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;

        private List<AudioSource> audioSources;

        private List<AudioSource> pausedAudioSource;

        public float MusicVolume { get; set; }

        public float SoundVolume { get; set; }

        public float MasterVolume { get; set; }

        public bool SoundEnabled { get; private set; }

        public static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("AudioManager").AddComponent<AudioManager>();
                }

                return _instance;
            }
        }

        public void Awake()
        {
            audioSources = new List<AudioSource>();
            pausedAudioSource = new List<AudioSource>();
        }

        public void PlaySound(AudioClip audioClip)
        {
            PlaySound(audioClip, SoundVolume, false);
        }

        public void PlaySound(AudioClip audioClip, float volume, bool loop)
        {
            if (SoundEnabled)
            {
                var audioSource = GetOrAddAudioSource(audioClip);
                audioSource.volume = volume;
                audioSource.loop = loop;
                audioSource.Play();
            }
        }

        public void PlayMusic(AudioClip audioClip)
        {
            PlayMusic(audioClip, MusicVolume);
        }

        public void PlayMusic(AudioClip audioClip, float volume)
        {
            PlaySound(audioClip, volume, true);
        }

        public bool IsMusicPlaying(AudioClip audioClip)
        {
            var audioSource = GetAudioSourceFromCache(audioClip);
            return audioSource != null ? audioSource.isPlaying : false;
        }

        public void SetVolume(AudioClip audioClip, float volume)
        {
            var audioSource = GetAudioSourceFromCache(audioClip);
            if (audioSource != null)
            {
                audioSource.volume = volume;
            }
        }

        public void StopSound(AudioClip audioClip)
        {
            foreach (var item in audioSources)
            {
                if (item.clip.name == audioClip.name)
                {
                    item.Stop();
                }
            }
        }
        
        public void StopAllSounds()
        {
            foreach (var item in audioSources)
            {
                item.Stop();
            }
        }

        public void ToggleSoundOnOff(bool enabled)
        {
            if (enabled)
            {
                EnableSound();
                
            }
            else
            {
                DisableSound();
            }
        }

        public void ToggleSoundOnOff()
        {
            ToggleSoundOnOff(!SoundEnabled);
        }

        public void DisableSound()
        {
            StopAllSounds();
            pausedAudioSource.Clear();
            SoundEnabled = false;
            
            // fix for NGUI Play Sound
            AudioListener.volume = 0;
        }
        
        public void EnableSound()
        {
            // start playing music again, music is a sound that has loop set to true
            foreach (var item in audioSources)
            {
                if (item.loop) item.Play();
            }
            SoundEnabled = true;

            // fix for NGUI Play Sound
            AudioListener.volume = 1;
        }

        public void PauseAllSounds()
        {
            pausedAudioSource.Clear();

            foreach (var item in audioSources)
            {
                if (item.isPlaying)
                {
                    item.Pause();
                    pausedAudioSource.Add(item);
                }
            }
        }

        public void ResumeAllSounds()
        {
            foreach (var item in pausedAudioSource)
            {
                item.Play();
            }
            pausedAudioSource.Clear();
        }

        private AudioSource AddAudioSource(AudioClip audioClip)
        {
            GameObject gameObject = new GameObject(audioClip.name);
            gameObject.transform.parent = this.transform;
            gameObject.AddComponent<AudioSource>();
            gameObject.audio.clip = audioClip;
            gameObject.audio.volume = MasterVolume;
            audioSources.Add(gameObject.audio);
            return gameObject.audio;
        }

        private AudioSource GetAudioSourceFromCache(AudioClip audioClip)
        {
            AudioSource cachedAudioSource = null;
            foreach (var audioSource in audioSources)
            {
                if (audioSource.clip.name == audioClip.name)
                {
                    cachedAudioSource = audioSource;
                }
            }
            return cachedAudioSource;
        }

        private AudioSource GetOrAddAudioSource(AudioClip audioClip)
        {
            var audioSource = GetAudioSourceFromCache(audioClip);
            if (audioSource == null)
            {
                audioSource = AddAudioSource(audioClip);
            }

            return audioSource;
        }

    }
}
