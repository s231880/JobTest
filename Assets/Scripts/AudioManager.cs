using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    //Current playing song
    public static Transform currentSong_;

    //Media player volume
    public static float playerVolume = 1;


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

    public void PlaySong()
    {
        m_AudioSources[currentSong_].GetComponent<AudioSource>().volume = playerVolume;
        m_AudioSources[currentSong_].GetComponent<AudioSource>().Play();
    }

    public void PauseSong()
    {
        m_AudioSources[currentSong_].GetComponent<AudioSource>().Pause();
    }

    public void StopSong()
    {
        m_AudioSources[currentSong_].GetComponent<AudioSource>().Stop();
    }


    public void UpdateVolume(float newVolume_)
    {
        playerVolume = newVolume_;
        m_AudioSources[currentSong_].GetComponent<AudioSource>().volume = playerVolume;
    }


    public void ChangeSong(Transform newSong_)
    {
        StopSong();
        currentSong_ = newSong_;
        PlaySong();
    }
}
