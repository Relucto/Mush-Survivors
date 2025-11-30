using UnityEngine;
using System;

[CreateAssetMenu(fileName = "AudioChannel", menuName = "Scriptable Objects/AudioChannel")]
public class AudioChannel : ScriptableObject
{
    public event Action<AudioClip, float, float, Transform, Vector3?> SoundRequested;
    public event Action StopRequested;

    float defaultPitch = 0;
    Transform defaultTransform = null;
    Vector3? defaultPosition = null;

    public void Play(AudioClip clipName, float volume)
    {
        Play(clipName, volume, defaultPitch, defaultTransform, defaultPosition);
    }

    public void Play(AudioClip clipName, float volume, float pitchVariance)
    {
        Play(clipName, volume, pitchVariance, defaultTransform, defaultPosition);
    }

    public void Play(AudioClip clipName, float volume, Transform spawnTransform)
    {
        Play(clipName, volume, defaultPitch, spawnTransform, defaultPosition);
    }

    public void Play(AudioClip clipName, float volume, Vector3? position)
    {
        Play(clipName, volume, defaultPitch, defaultTransform, position);
    }

    public void Play(AudioClip clipName, float volume, float pitchVariance, Transform defaultTransform)
    {
        Play(clipName, volume, pitchVariance, defaultTransform, defaultPosition);
    }

    public void Play(AudioClip clipName, float volume, float pitchVariance, Vector3? position)
    {
        Play(clipName, volume, pitchVariance, defaultTransform, position);
    }

    public void Play(AudioClip clipName, float volume, Transform spawnTransform, Vector3? position)
    {
        Play(clipName, volume, defaultPitch, spawnTransform, position);
    }

    public void Play(AudioClip clipName, float volume, float pitchVariance, Transform spawnTransform, Vector3? position)
    {
        if (clipName == null)
        {
            Debug.LogError("Missing clip name or transform from request for " + name);
            return;
        }
        if (SoundRequested == null)
        {
            Debug.LogError("No Play subscribers for " + name);
            return;
        }

        SoundRequested.Invoke(clipName, volume, pitchVariance, spawnTransform, position);
    }

    public void Stop()
    {
        if (StopRequested == null)
        {
            Debug.LogError("No Stop subscribers for " + name);
            return;
        }

        StopRequested.Invoke();
    }
}