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
    private static int currentEffect_ = 0;


    //private static int selectedEffectThroughUI_ = 0;
    private static bool hasTheEffectBeenChanged = false;


    //private static bool isThePlayerPlaying = true;
    //private static bool isTheEffectBeenChanged = false;

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
    //-------------------------- UI HANDLE FUNCTIONS ----------------------------------------
    
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
    ///  Method called by the UI play button
    /// </summary>
    public void PressedPlayButton()
    {
        AudioManager.Instance.PlayPauseTheSong(m_AudioSource.GetChild(currentSong_));
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

        VisualManager.Instance.changeMediaPlayerColor(m_Visuals.GetChild(currentEffect_), textureColor);
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
    public void ChangeEffectThroughUI()
    {
        currentEffect_ = GetEnabledToggleIndex();
        VisualManager.Instance.ChangeEffect(m_Visuals.GetChild(currentEffect_), m_Visuals.GetChild(currentEffect_), m_AudioSource.GetChild(currentSong_));

        //UpdateSelectedToggle(currentEffect_, true);
        //hasTheEffectBeenChanged = true;
        //Debug.Log(selectedEffectThroughUI_ + "ChangeEffectThroughUI" + currentEffect_);
    }

    public void ResetToggle(int index_)
    {
        listOfToggle[index_].isOn = true;
        //selectedEffectThroughUI_ = currentEffect_;
    }

    public void UpdateSelectedToggle(int index_, bool manualChange)
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

  

    public void NextEffect()
    {
        int newEffectToPlayIndex_;
 
            if (currentEffect_ != 2)
            {
                newEffectToPlayIndex_ = currentEffect_ + 1;
                VisualManager.Instance.ChangeEffect(m_Visuals.GetChild(currentEffect_), m_Visuals.GetChild(newEffectToPlayIndex_), m_AudioSource.GetChild(currentSong_));
            }
            else
            {
                newEffectToPlayIndex_ = 0;
                VisualManager.Instance.ChangeEffect(m_Visuals.GetChild(currentEffect_), m_Visuals.GetChild(newEffectToPlayIndex_), m_AudioSource.GetChild(currentSong_));
            }

        //if(currentEffect_ != selectedEffectThroughUI_)
        //    ResetToggle(currentEffect_);

        Debug.Log(hasTheEffectBeenChanged + "bool vediamo");
        currentEffect_ = newEffectToPlayIndex_;
        UpdateSelectedToggle(currentEffect_, false);

    }

    public void PreviousEffect()
    {
        int newEffectToPlayIndex_;

        if (currentEffect_ != 0)
        {
            newEffectToPlayIndex_ = currentEffect_ - 1;
            VisualManager.Instance.ChangeEffect(m_Visuals.GetChild(currentEffect_), m_Visuals.GetChild(newEffectToPlayIndex_), m_AudioSource.GetChild(currentSong_));
        }
        else
        {
            newEffectToPlayIndex_ = 2;
            VisualManager.Instance.ChangeEffect(m_Visuals.GetChild(currentEffect_), m_Visuals.GetChild(newEffectToPlayIndex_), m_AudioSource.GetChild(currentSong_));
        }

        currentEffect_ = newEffectToPlayIndex_;
        UpdateSelectedToggle(currentEffect_, false);

        //if (isTheEffectBeenChanged)
        //    ResetToggle(currentEffect_);
    }

    //---------------------FUNCTIONS TO HANDLE AUDIO AND VISUAL MANAGER--------------------------
    //-------------------------------------------------------------------------------------------
}
