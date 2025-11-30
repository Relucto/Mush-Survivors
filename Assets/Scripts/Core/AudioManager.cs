using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Cache")]
    [SerializeField] AudioChannel musicChannel;
    [SerializeField] AudioChannel SFXChannel;
    [SerializeField] AudioSource soundFXObject;

    [Header("Pool Settings")]
    [SerializeField] int startSize = 15, maxSize = 50;

    AudioSource musicAudioSource; // Plays music on this object only
    List <AudioSource> activeAudio = new List<AudioSource>();
    Pool audioPool;
    Vector3 defaultPosition;

    //Create singleton & Check Dependencies
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
                Destroy(gameObject);
        }

        if (musicChannel == null)
        {
            Debug.LogError("musicChannel is null for " + name);
        }
        if (SFXChannel == null)
        {
            Debug.LogError("SFXChannel is null for " + name);
        }
        if (soundFXObject == null)
        {
            Debug.LogError("soundFXObject is null for " + name);
        }

        musicAudioSource = GetComponent<AudioSource>();
        audioPool = new Pool(soundFXObject.gameObject, startSize, maxSize, transform);

        defaultPosition = transform.position;
    }

    //Subscribe to audio channels
    void OnEnable()
    {
        musicChannel.SoundRequested += PlayMusic;
        SFXChannel.SoundRequested += PlaySound;
        musicChannel.StopRequested += StopMusic;
    }

    //Unsubscribe from audio channels
    void OnDisable()
    {
        musicChannel.SoundRequested -= PlayMusic;
        SFXChannel.SoundRequested -= PlaySound;
        musicChannel.StopRequested -= StopMusic;
    }

    //This function will play the specified music clip with this object's AudioSource
    //Does not instantiate a SFX prefab to allow for further music customization later on
    void PlayMusic(AudioClip clipName, float volume, float pitchVariance, Transform spawnTransform, Vector3? position)
    {
        if (musicAudioSource.isPlaying)
            return;

        musicAudioSource.clip = clipName;
        musicAudioSource.volume = volume;
        musicAudioSource.Play();
    }

    void StopMusic()
    {
        StartCoroutine(FadeOut());

        IEnumerator FadeOut()
        {
            while (musicAudioSource.volume > 0)
            {
                musicAudioSource.volume -= Time.deltaTime;
                yield return null;
            }

            musicAudioSource.Stop();
        }   
    }

    void PlaySound(AudioClip clipName, float volume, float pitchVariance, Transform spawnTransform, Vector3? position)
    {
        // Get from pool
        GameObject poolObject = audioPool.Get();

        // ===== Set transform and position =====
        // Setting transform only will set it to the transform's position as well.
        // Adding both transform and position will parent it, but set it to the given world position
        if (spawnTransform != null)
        {
            poolObject.transform.position = spawnTransform.position;
            poolObject.transform.parent = spawnTransform;
        }
        if (position.HasValue)
        {
            poolObject.transform.position = position.Value;
        }       
        
        // ===== Audio Settings =====
        AudioSource audio = poolObject.GetComponent<AudioSource>();

        audio.clip = clipName;
        audio.volume = volume;

        audio.pitch = pitchVariance == 0 ? 1 : 1 + Random.Range(-pitchVariance, pitchVariance);

        // Play and add to list of active audio
        audio.Play();
        activeAudio.Add(audio);

        if (activeAudio.Count > maxSize)
        {
            Debug.LogWarning($"Greater than {maxSize} active audio objects");
        }
    }

    void Update()
    {
        // Return audio when finished playing
        for (int i = activeAudio.Count - 1; i >= 0; i--)
        {
            if (activeAudio[i].isPlaying == false)
            {
                activeAudio[i].transform.position = defaultPosition;
                activeAudio[i].transform.parent = transform;

                audioPool.Return(activeAudio[i].gameObject);
                activeAudio.RemoveAt(i);
            }
        }
    }
}

namespace AudioSystem
{
    [System.Serializable]
    public class AudioPair
    {
        public AudioClip clip;
        [Range(0, 1)] public float volume = 1;
        [Range(-3, 3)] public float pitchVariance = 0;
    }

    public static class AudioHelper
    {
        public static AudioPair GetRandom(AudioPair[] audios)
        {
            return audios[Random.Range(0, audios.Length)];
        }
    }
}