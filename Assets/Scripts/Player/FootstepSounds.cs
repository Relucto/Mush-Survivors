using UnityEngine;
using AudioSystem;

public class FootstepSounds : MonoBehaviour
{
    [Header("Audio")]
    public AudioChannel sfx;
    public AudioPair[] stepSounds;

    public void PlayStepSound()
    {
        int rand = Random.Range(0, stepSounds.Length);

        sfx.Play(stepSounds[rand].clip, stepSounds[rand].volume, transform);
    }
}
