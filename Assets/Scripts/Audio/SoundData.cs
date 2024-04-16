using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound")]
public class SoundData : ScriptableObject
{
    [field: SerializeField] public AudioClip[] Clip { get; private set; }
    [field: SerializeField] public bool Loop { get; private set; }
    [field: SerializeField] public bool DontSmoothOut { get; private set; }
    [field: SerializeField] public bool PlayOnAwake { get; private set; }
    [field: SerializeField] public float Volume { get; private set; } = 1.0f;

    public void Play(AudioSource source)
    {
        source.clip = Clip[Random.Range(0, Clip.Length)];
        source.volume = Volume;

        source.loop = Loop;
        source.playOnAwake = PlayOnAwake;
        source.Play();
    }
}