using ClumsyWizard.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVolumeLerpData
{
    public float startVolume;
    public float targetVolume;
    public float elapsedTime;
    public AudioSource audioSource;

    public AudioVolumeLerpData(float startVolume, float targetVolume, AudioSource audioSource)
    {
        this.startVolume = startVolume;
        this.targetVolume = targetVolume;
        this.audioSource = audioSource;
        elapsedTime = 0.0f;
    }
}

public class AudioManager : MonoBehaviour, ISceneLoadEvent
{
    [SerializeField] private ClumsyDictionary<string, SoundData> audioData;
    private Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();
    private List<AudioVolumeLerpData> audiosToLerp = new List<AudioVolumeLerpData>();
    private float audioStopDuration = 1.0f;

    private void Awake()
    {
        foreach (string audioID in audioData.Keys)
        {
            if (audioData[audioID] == null)
                continue;

            AudioSource source = gameObject.AddComponent<AudioSource>();
            audioSources.Add(audioID, source);
            if (audioData[audioID].PlayOnAwake)
                Play(audioID);
        }
    }

    private void Update()
    {
        if (audiosToLerp.Count == 0)
            return;

        for (int i = audiosToLerp.Count - 1; i >= 0; i--)
        {
            AudioVolumeLerpData lerpData = audiosToLerp[i];

            if(lerpData.elapsedTime > audioStopDuration)
            {
                lerpData.audioSource.Stop();
                audiosToLerp.RemoveAt(i);
            }
            else
            {
                lerpData.elapsedTime += Time.deltaTime;
                lerpData.audioSource.volume = Mathf.Lerp(lerpData.startVolume, lerpData.targetVolume, lerpData.elapsedTime / audioStopDuration);
            }
        }
    }

    public void Play(string id)
    {
        if (!audioData.ContainsKey(id) || audioData[id] == null || audioSources[id].isPlaying)
            return;

        audioData[id].Play(audioSources[id]);
    }
    public void Stop(string id)
    {
        audiosToLerp.Add(new AudioVolumeLerpData(audioData[id].Volume, 0.0f, audioSources[id]));
    }

    public void OnSceneLoadTriggered(Action onComplete)
    {
        foreach (string audioID in audioData.Keys)
        {
            if (!audioData[audioID].DontSmoothOut)
                Stop(audioID);
        }

        onComplete?.Invoke();
    }
}