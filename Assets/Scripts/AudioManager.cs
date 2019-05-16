using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    //Media player volume
    public static float playerVolume = 1;
    //Keep track if the mediaPlayer is currently active
    private static bool isThePlayerPlaying = true;

    private Dictionary<Transform, Audio> m_AudioSources = new Dictionary<Transform, Audio>();

    private void Awake()
    {
        if(null == Instance)
        {
            Instance = this;
        }
        else
        {
            GameObject.Destroy(this);
        }
    }

	public void Init() { }

    public void AddAudioSource(Transform source)
    {
        if(false == m_AudioSources.ContainsKey(source))
        {
            AudioSource audioSource = source.GetComponent<AudioSource>();
            if (null != audioSource)
            {
                //Create new instance
                Audio audio = source.gameObject.AddComponent<Audio>();
                audio.Init(audioSource);

                m_AudioSources.Add(source, audio);
            }
        }
    }

    public void RemoveAudioSource(Transform source)
    {
        if(true == m_AudioSources.ContainsKey(source))
        {
            m_AudioSources.Remove(source);
        }
    }

    public Audio GetAudio(Transform source)
    {
        if(true == m_AudioSources.ContainsKey(source))
        {
            return m_AudioSources[source];
        }

        return null;
    }

    private void Update()
    {
        foreach(KeyValuePair<Transform, Audio> audio in m_AudioSources)
        {
            audio.Value.ManualUpdate();
        }
    }

    public void PlaySong(Transform songToPlay_)
    {
        m_AudioSources[songToPlay_].GetComponent<AudioSource>().volume = playerVolume;
        m_AudioSources[songToPlay_].GetComponent<AudioSource>().Play();
    }

    public void PauseSong(Transform songToPause_)
    {
        m_AudioSources[songToPause_].GetComponent<AudioSource>().Pause();
    }

    public void PlayPauseTheSong(Transform currentSong_)
    {
        if (isThePlayerPlaying)
        {
            PauseSong(currentSong_);
            isThePlayerPlaying = false;
        }

        else
        {
            PlaySong(currentSong_);
            isThePlayerPlaying = true;
        }
    }

    public void StopSong(Transform songToStop_)
    {
        m_AudioSources[songToStop_].GetComponent<AudioSource>().Stop();
    }

    public void UpdateVolume(float newVolume_, Transform currentSong_)
    {
        playerVolume = newVolume_;
        m_AudioSources[currentSong_].GetComponent<AudioSource>().volume = playerVolume;
    }


    public void ChangeSong(Transform oldSong_, Transform newSong_)
    {
        StopSong(oldSong_);
        PlaySong(newSong_);
    }
}
