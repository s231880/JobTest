using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Core : MonoBehaviour
{
    //Public variables
    public Transform m_AudioSource;
    public Transform m_Visuals;
    public Slider volumeSlider;
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;
    public ToggleGroup effectSelector;

    private Color textureColor;

    List<Toggle> listOfToggle;

    //Variables that keep track of what track and effect aare currently running
    private static int currentSong = 0;
    private static int currentEffect = 0;

    //keep track if the audioSource is running or not
    //private static bool isThePlayerPlaying = true;
    private static bool isTheEffectBeenChanged = false;
	
    // Use this for initialization
	void Start ()
    {
        Application.runInBackground = true;
        AudioManager.Instance.Init();
        List<Transform> audioTransforms = new List<Transform>();
        List<Transform> visualTransforms = new List<Transform>();
        listOfToggle = new List<Toggle>();


        if (null != m_AudioSource)
        {
            //Setup Audio Sources to play
            foreach(Transform child in m_AudioSource)
            {
                audioTransforms.Add(child);
                AudioManager.Instance.AddAudioSource(child);
            }
        }

        VisualManager.Instance.Init();

        if(null != m_Visuals)
        {
            foreach (Transform child in m_Visuals)
            {
                visualTransforms.Add(child);
                VisualManager.Instance.CreateVisual(child);
            }

            VisualManager.currentEffect_ = m_Visuals.GetChild(0);
        }
        //Assign each visual to an audio
        for(int i = 0; i < visualTransforms.Count; i++)
        {
            VisualManager.Instance.AssignAudioToVisual(visualTransforms[i], audioTransforms[0]);
        }

        if (null != effectSelector)
        {
            foreach (Toggle child in effectSelector.GetComponentsInChildren<Toggle>())
            {
                listOfToggle.Add(child);
            }
        }

        textureColor = new Color32(0,0,0,0);
    }

    /// <summary>
    ///Method called by the UI next button
    /// </summary>
    public void pressedNextButton()
    {
        NextSong();
        NextEffect();
    }

    /// <summary>
    ///Method called by the UI previous button
    /// </summary>
    public void pressedPreviousButton()
    {
        PreviousSong();
        PreviousEffect();
    }

    /// <summary>
    /// Method to notify the audio manager to play the next song
    /// </summary>
    public void NextSong()
    {
        int newSongToPlayIndex;

        if (currentSong != 2)
        {
            newSongToPlayIndex = currentSong + 1;
            AudioManager.Instance.ChangeSong(m_AudioSource.GetChild(currentSong), m_AudioSource.GetChild(newSongToPlayIndex));
        }

        else
        {
            newSongToPlayIndex = 0;
            AudioManager.Instance.ChangeSong(m_AudioSource.GetChild(currentSong), m_AudioSource.GetChild(newSongToPlayIndex));
        }

        currentSong = newSongToPlayIndex;
    }

    /// <summary>
    /// Method to notify the audio manager to play the previous song
    /// </summary>
    public void PreviousSong()
    {
        int newSongToPlayIndex;

        if (currentSong != 0)
        {
            newSongToPlayIndex = currentSong - 1;
            AudioManager.Instance.ChangeSong(m_AudioSource.GetChild(currentSong), m_AudioSource.GetChild(newSongToPlayIndex));
        }

        else
        {
            newSongToPlayIndex = 2;
            AudioManager.Instance.ChangeSong(m_AudioSource.GetChild(currentSong), m_AudioSource.GetChild(newSongToPlayIndex));
        }

        currentSong = newSongToPlayIndex;
    }

    /// <summary>
    ///  Method called by the UI play button, notify the AudioMangare to play/pause the song
    /// </summary>
    public void PressedPlayButton()
    {
        AudioManager.Instance.PlayPauseTheSong(m_AudioSource.GetChild(currentSong));
    }

    /// <summary>
    ///  Method called by the UI volume slider
    /// </summary>
    public void SliderVolumeChange()
    {
        AudioManager.Instance.UpdateVolume(volumeSlider.value, m_AudioSource.GetChild(currentSong));
    }

    public void NextEffect()
    {
        if(currentEffect != 2)
        {
            currentEffect++;
            VisualManager.Instance.ChangeEffect(m_Visuals.GetChild(currentEffect), m_AudioSource.GetChild(currentSong));
        }
        else
        {
            currentEffect = 0;
            VisualManager.Instance.ChangeEffect(m_Visuals.GetChild(currentEffect), m_AudioSource.GetChild(currentSong));
        }

        UpdateSelectedToggle(currentEffect);

        if (isTheEffectBeenChanged)
            ResetToggle(currentEffect);
    }

    public void PreviousEffect()
    {
        if (currentEffect != 0)
        {
            currentEffect--;
            VisualManager.Instance.ChangeEffect(m_Visuals.GetChild(currentEffect), m_AudioSource.GetChild(currentSong));
        }
        else
        {
            currentEffect = 2;
            VisualManager.Instance.ChangeEffect(m_Visuals.GetChild(currentEffect), m_AudioSource.GetChild(currentSong));
        }

        UpdateSelectedToggle(currentEffect);

        if (isTheEffectBeenChanged)
            ResetToggle(currentEffect);
    }

    /// <summary>
    /// This method allows the user to change the visual effect through the Toggle Group
    /// without changing the playing song
    /// </summary>
    public void ChangeEffectThroughUI()
    {
        string effectName_ = "";

        foreach (Toggle toggle_ in listOfToggle)
        {
            if (toggle_.isOn)
                effectName_ = toggle_.name;
        }

        //Changing effect without changing song
        if (effectName_ == "Toggle1")
            VisualManager.Instance.ChangeEffect(m_Visuals.GetChild(0), m_AudioSource.GetChild(currentSong));

        else if (effectName_ == "Toggle2")
            VisualManager.Instance.ChangeEffect(m_Visuals.GetChild(1), m_AudioSource.GetChild(currentSong));

        else
            VisualManager.Instance.ChangeEffect(m_Visuals.GetChild(2), m_AudioSource.GetChild(currentSong));

        isTheEffectBeenChanged = true;

    }

    public void ResetToggle(int index_)
    {
        listOfToggle[index_].isOn = true;
        isTheEffectBeenChanged = false;
    }

    public void UpdateSelectedToggle(int index_)
    {
        listOfToggle[index_].isOn = true;
    }

    /// <summary>
    /// Method call when the user change the RGB sliders values to update the mediaPlayer effect color
    /// </summary>
    public void UpdateTextureColor()
    {
        textureColor.r = redSlider.value;
        textureColor.g = greenSlider.value;
        textureColor.b = blueSlider.value;

        VisualManager.Instance.changeMediaPlayerColor(textureColor);
    }

}
