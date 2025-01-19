using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Music References")]
    [SerializeField] Playlist[] playlists;

    [Header("Audio References")]
    [SerializeField] AudioMixer audioMixer;
    [Space(5)]
    [SerializeField] AudioMixerGroup musicMixerGroup;
    [SerializeField] AudioMixerGroup soundMixerGroup;

    Queue<AudioSource> soundsGo = new Queue<AudioSource>();

    [Header("System References")]
    [SerializeField, Tooltip("Number of GameObject create on start for the sound")] int startingAudioObjectsCount = 30;

    [Header("Output")]
    [SerializeField] RSE_PlayClipAt rsePlayClipAt;

    private void OnEnable()
    {
        rsePlayClipAt.action += PlayClipAt;
    }
    private void OnDisable()
    {
        rsePlayClipAt.action -= PlayClipAt;
    }

    private void Start()
    {
        // Set Audio Source for Musics
        foreach (var playlist in playlists)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            playlist.audioSource = audioSource;
            audioSource.volume = playlist.volumMultiplier;
            audioSource.outputAudioMixerGroup = musicMixerGroup;

            StartCoroutine(SetAudioSourceClip(playlist, playlist.maxLoop));
        }

        // Create Audio Object
        for (int i = 0; i < startingAudioObjectsCount; i++)
        {
            soundsGo.Enqueue(CreateSoundsGO());
        }
    }

    IEnumerator SetAudioSourceClip(Playlist playlist, int maxLoop)
    {
        while (maxLoop != 0)
        {
            playlist.audioSource.clip = playlist.clips[playlist.currentClipIndex];
            playlist.audioSource.Play();

            yield return new WaitForSeconds(playlist.clips[playlist.currentClipIndex].length);

            playlist.currentClipIndex = (playlist.currentClipIndex + 1) % playlist.clips.Length;

            if (maxLoop > 0) maxLoop--;
        }
    }

    /// <summary>
    /// Require the clip and the power of the sound
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="soundPower"></param>
    /// <param name="position of the sound"></param>
    void PlayClipAt(AudioClip clip, Vector3 position, float volumMultiplier = 1)
    {
        AudioSource tmpAudioSource;
        if (soundsGo.Count > 0)
        {
            tmpAudioSource = soundsGo.Dequeue();
        }
        else
        {
            tmpAudioSource = CreateSoundsGO();
        }

        tmpAudioSource.transform.position = position;

        // Set the volume
        volumMultiplier = Mathf.Clamp(volumMultiplier, 0, 1);
        tmpAudioSource.volume = volumMultiplier;

        // Set the clip
        tmpAudioSource.clip = clip;
        tmpAudioSource.Play();
        StartCoroutine(ReturnAudioSourceToQueue(tmpAudioSource));
    }

    IEnumerator ReturnAudioSourceToQueue(AudioSource current)
    {
        yield return new WaitUntil(() => !current.isPlaying);
        soundsGo.Enqueue(current);
    }

    AudioSource CreateSoundsGO()
    {
        AudioSource tmpAudioSource = new GameObject("AudioSource").AddComponent<AudioSource>();
        tmpAudioSource.transform.SetParent(transform);
        tmpAudioSource.outputAudioMixerGroup = soundMixerGroup;

        return tmpAudioSource;
    }

    [System.Serializable]
    public class Playlist
    {
        [Range(0, 1)] public float volumMultiplier = 1;
        [Tooltip("If value equals \"-1\", infinite loop")] public int maxLoop = -1;
        [Space(10)]
        public AudioClip[] clips;

        [HideInInspector] public AudioSource audioSource;
        [HideInInspector] public int currentClipIndex = 0;

        public Playlist(AudioClip[] clips, float volumMultiplier, int maxLoop)
        {
            this.clips = clips;
            this.volumMultiplier = volumMultiplier;
            this.maxLoop = maxLoop;
        }
    }
}