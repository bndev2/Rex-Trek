using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyAssets
{
    public class SoundFXPlayer : MonoBehaviour
    {
        [SerializeField] private bool _isLoopedPitchRandom;


        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSource _loopingAudioSource;

        [SerializeField] private List<AudioClip> _clips;
        [SerializeField] private float _volume = 1;
        [SerializeField] private float _amountToPlay = 1;

        private AudioClip _currentLoopingTrack;
        private bool _isLooping;
        private float _loopInterval;
        private float _loopTimer = 0;

        public void PlaySound(int index)
        {
            SoundFXManager.instance.PlaySoundAtTransform(_clips[index], transform, _volume, _amountToPlay);
        }

        public void PlaySoundLocally(int index, float pitch = 1)
        {
            if (_audioSource == null)
            {
                Debug.LogError("No audio source attached!");
                return;
            }

            _audioSource.pitch = pitch;
            _audioSource.PlayOneShot(_clips[index], _volume);
        }

        public void PlayLocallyLooped(int index, float loopInterval = 1)
        {
            if (_loopingAudioSource == null)
            {
                Debug.LogError("No looping audio source attached!");
                return;
            }

            _loopInterval = loopInterval;
            _currentLoopingTrack = _clips[index];
            _loopingAudioSource.clip = _currentLoopingTrack;
            _loopingAudioSource.loop = true;
            _loopingAudioSource.Play();
            _isLooping = true;
        }

        public void StopLocallyLooped()
        {
            if (_loopingAudioSource == null)
            {
                Debug.LogError("No looping audio source attached!");
                return;
            }

            _loopingAudioSource.Stop();
            _loopingAudioSource.loop = false;
            _currentLoopingTrack = null;
            _isLooping = false;
            _loopTimer = 0;
        }

        private void Update()
        {
            if (_isLooping)
            {
                _loopTimer += Time.deltaTime;

                if (_loopTimer >= _loopInterval)
                {
                    if (_isLoopedPitchRandom)
                    {
                        _loopingAudioSource.pitch = Random.Range(.95f, 1);
                    }
                    _loopTimer = 0;
                    _loopingAudioSource.Play();
                }
            }
        }

        private void Start()
        {

        }
    }
}