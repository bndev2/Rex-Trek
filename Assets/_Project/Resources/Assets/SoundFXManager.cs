using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace MyAssets{
public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    [SerializeField] AudioMixerGroup _audioMixerGroup;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySoundAtTransform(AudioClip audioClip, Transform spawnTransform, float volume = 1, float amountToPlay = 1)
    {
        PlaySound(audioClip, spawnTransform, volume, amountToPlay);
    }

    public void PlaySoundAtTransformRandom(List<AudioClip> audioClips, Transform spawnTransform, float volume = 1, float amountToPlay = 1)
    {
        AudioClip audioClip = audioClips[Random.Range(0, audioClips.Count)];
        PlaySound(audioClip, spawnTransform, volume, amountToPlay);
    }

    private void PlaySound(AudioClip audioClip, Transform spawnTransform, float volume = 1, float amountToPlay = 1)
    {
        // Create a new GameObject
        GameObject audioObject = new GameObject("AudioObject");

        // Add an AudioSource component to the GameObject
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        audioSource.spatialBlend = 1;

        audioSource.minDistance = 400;

        audioSource.outputAudioMixerGroup = _audioMixerGroup;

        // Set the AudioSource properties
        audioSource.clip = audioClip;
        audioSource.volume = volume;

        // Move the GameObject to the spawnTransform position
        audioObject.transform.position = spawnTransform.position;

        if (amountToPlay > 1)
        {
            audioSource.loop = true;
        }

        // Play the sound
        audioSource.Play();

        // Destroy the GameObject after the sound has finished playing
        Destroy(audioObject, audioSource.clip.length * amountToPlay);
    }
}
}