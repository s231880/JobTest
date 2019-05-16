using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private static int currentSong_ = 0;
    private static int currentPlayingEffectIndex_ = 0;
    private static int newEffectToPlayIndex_ = 0;

    //If the effect has been change through the UI toggle group
    private static bool manualChangeVisualEffect_ = false;

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

    //-------------------------------------------------------------------------------------------
    //-------------------------- UI HANDLE FUNCTIONS --------------------------------------------

    /// <summary>
    ///Method called by the UI next button
    /// </summary>
    public void PressedNextButton()
    {
        //Playing the new song
        NextSong();

        //If is running the default visual effect activating the next one
        if (!manualChangeVisualEffect_)
            UpdateSelectedToggle(GetNextEffectIndex());

        //If visual effect has been updated through the UI
        else
        {
            //Setting the default effect
            UpdateSelectedToggle(currentSong_);
            manualChangeVisualEffect_ = false;
        }

    }



    /// <summary>
    ///Method called by the UI previous button
    /// </summary>
    public void PressedPreviousButton()
    {
        //Playing the new song
        PreviousSong();

        //If is running the default visual effect activating the next one
        if (!manualChangeVisualEffect_)
            UpdateSelectedToggle(GetPreviousEffectIndex());

        else
        {
            //Setting the default effect
            UpdateSelectedToggle(currentSong_);
            manualChangeVisualEffect_ = false;
        }
    }

    /// <summary>
    ///  Method called by the UI play button
    /// </summary>
    public void PressedPlayButton()
    {
        AudioManager.Instance.PlayPauseTheSong(m_AudioSource.GetChild(currentSong_));
    }

    /// <summary>
    ///  Method called by the UI play button
    /// </summary>
    public void PressedStopButton()
    {
        AudioManager.Instance.StopSong(m_AudioSource.GetChild(currentSong_));
    }
    
    /// <summary>
    ///  Method called by the UI volume slider
    /// </summary>
    public void SliderVolumeChange()
    {
        AudioManager.Instance.UpdateVolume(volumeSlider.value, m_AudioSource.GetChild(currentSong_));
    }

    /// <summary>
    /// Method call when the user change the RGB sliders values to update the mediaPlayer effect color
    /// </summary>
    public void UpdateTextureColor()
    {
        textureColor.r = redSlider.value;
        textureColor.g = greenSlider.value;
        textureColor.b = blueSlider.value;

        VisualManager.Instance.changeMediaPlayerColor(m_Visuals.GetChild(currentPlayingEffectIndex_), textureColor);
    }

    /// <summary>
    /// Function to find which one of the toggle effect is activated
    /// </summary>
    public int GetEnabledToggleIndex()
    {
        string effectName_ = "";

        foreach (Toggle toggle_ in listOfToggle)
        {
            if (toggle_.isOn)
                effectName_ = toggle_.name;
        }

        if (effectName_ == "Toggle1")
            return 0;

        else if (effectName_ == "Toggle2")
            return 1;

        else
            return 2;
    }

    /// <summary>
    /// This method allows the user to change the visual effect through the Toggle Group
    /// without changing the playing song
    /// </summary>
    public void ChangeEffect()
    {
        //MediaPlayer not updated through the UI toggles  -> running the default effect 
        if (!manualChangeVisualEffect_)
        {
            VisualManager.Instance.ChangeEffect(m_Visuals.GetChild(currentPlayingEffectIndex_), m_Visuals.GetChild(newEffectToPlayIndex_), m_AudioSource.GetChild(currentSong_));

            //Media player manual updated now, updating the effect without changing song
            if (GetEnabledToggleIndex() != currentSong_)
            {
                UpdateCurrentEffectIndex(GetEnabledToggleIndex());
                manualChangeVisualEffect_ = true;  //No more default visual effect
            }

            //Media player automatic updated, no toggle used!
            else
                UpdateCurrentEffectIndex(newEffectToPlayIndex_);
        }

        //MediaPlayer updated through the UI toggles
        else
        {
            VisualManager.Instance.ChangeEffect(m_Visuals.GetChild(currentPlayingEffectIndex_), m_Visuals.GetChild(GetEnabledToggleIndex()), m_AudioSource.GetChild(currentSong_));
            UpdateCurrentEffectIndex(GetEnabledToggleIndex());
        }
    }

    public void UpdateSelectedToggle(int index_)
    {
        listOfToggle[index_].isOn = true;
    }

    //-------------------------- UI HANDLE FUNCTIONS ----------------------------------------
    //-------------------------------------------------------------------------------------------



    //-------------------------------------------------------------------------------------------
    //---------------------FUNCTIONS TO HANDLE AUDIO AND VISUAL MANAGER--------------------------

    /// <summary>
    /// Method to notify the audio manager to play the next song
    /// </summary>
    public void NextSong()
    {
        int newSongToPlayIndex_;

        if (currentSong_ != 2)
        {
            newSongToPlayIndex_ = currentSong_ + 1;
            AudioManager.Instance.ChangeSong(m_AudioSource.GetChild(currentSong_), m_AudioSource.GetChild(newSongToPlayIndex_));
        }

        else
        {
            newSongToPlayIndex_ = 0;
            AudioManager.Instance.ChangeSong(m_AudioSource.GetChild(currentSong_), m_AudioSource.GetChild(newSongToPlayIndex_));
        }

        currentSong_ = newSongToPlayIndex_;
    }

    /// <summary>
    /// Method to notify the audio manager to play the previous song
    /// </summary>
    public void PreviousSong()
    {
        int newSongToPlayIndex_;

        if (currentSong_ != 0)
        {
            newSongToPlayIndex_ = currentSong_ - 1;
            AudioManager.Instance.ChangeSong(m_AudioSource.GetChild(currentSong_), m_AudioSource.GetChild(newSongToPlayIndex_));
        }

        else
        {
            newSongToPlayIndex_ = 2;
            AudioManager.Instance.ChangeSong(m_AudioSource.GetChild(currentSong_), m_AudioSource.GetChild(newSongToPlayIndex_));
        }

        currentSong_ = newSongToPlayIndex_;
    }

  

    public int GetNextEffectIndex()
    { 
        if (currentPlayingEffectIndex_ != 2)
            newEffectToPlayIndex_ = currentPlayingEffectIndex_ + 1;
        else
            newEffectToPlayIndex_ = 0;

        return newEffectToPlayIndex_;
    }

    public int GetPreviousEffectIndex()
    {
        if (currentPlayingEffectIndex_ != 0)
            newEffectToPlayIndex_ = currentPlayingEffectIndex_ - 1;
        else
            newEffectToPlayIndex_ = 2;

        return newEffectToPlayIndex_;
    }

    public void UpdateCurrentEffectIndex(int value)
    {
        currentPlayingEffectIndex_ = value;
    }

    //---------------------FUNCTIONS TO HANDLE AUDIO AND VISUAL MANAGER--------------------------
    //-------------------------------------------------------------------------------------------




}
