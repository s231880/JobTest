using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Audio : MonoBehaviour
{
    public event Action<float[]> OnAudioSpectrumUpdate;
    private const int NUM_AUDIO_SAMPLES = 512;

    [SerializeField]
    private FFTWindow m_FFTWindow = FFTWindow.Hamming;
    private AudioSource m_Audio;
    private float[] m_AudioSamples;

    /// <summary>
    /// Provide an AudioSource to allow runtime processing of the Spectrum Data
    /// </summary>
    /// <param name="source"></param>
	public virtual void Init(AudioSource source)
    {
        m_Audio = source;
        m_AudioSamples = new float[NUM_AUDIO_SAMPLES];
    }

    public virtual void DeInit() { }

    public virtual void ManualUpdate()
    {
        if(null != m_Audio)
        {
            m_Audio.GetSpectrumData(m_AudioSamples, 0, m_FFTWindow);
            if (null != OnAudioSpectrumUpdate)
            {
                OnAudioSpectrumUpdate.Invoke(m_AudioSamples);
            }
        }
    }
}