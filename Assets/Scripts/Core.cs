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
    private static bool isThePlayerPlaying = true;
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

            //Setting the current song to the first child
            AudioManager.currentSong_ = m_AudioSource.GetChild(0);
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
    public void NextSong()
    {
        if (currentSong != 2)
        {
            ++currentSong;
            AudioManager.Instance.ChangeSong(m_AudioSource.GetChild(currentSong));
            NextEffect(m_AudioSource.GetChild(currentSong));
        }

        else
        {
            currentSong = 0;
            AudioManager.Instance.ChangeSong(m_AudioSource.GetChild(currentSong));
            NextEffect(m_AudioSource.GetChild(currentSong));
        }
    }

    /// <summary>
    ///Method called by the UI previous button
    /// </summary>
    public void PreviousSong()
    {

        if (currentSong != 0)
        {
            --currentSong;
            AudioManager.Instance.ChangeSong(m_AudioSource.GetChild(currentSong));
            PreviousEffect(m_AudioSource.GetChild(currentSong));
        }

        else
        {
            currentSong = 2;
            AudioManager.Instance.ChangeSong(m_AudioSource.GetChild(currentSong));
            PreviousEffect(m_AudioSource.GetChild(currentSong));
        }
    }

    /// <summary>
    ///  Method called by the UI play button, to pause and play the song
    /// </summary>
    public void PausePlaySong()
    {
        if (isThePlayerPlaying)
        {
            AudioManager.Instance.PauseSong();
            isThePlayerPlaying = false;
        }
        else
        {
            AudioManager.Instance.PlaySong();
            isThePlayerPlaying = true;
        }
            
    }

    /// <summary>
    ///  Method to change the Volume through the UI slider
    /// </summary>
    public void ChangeVolume()
    {
        AudioManager.Instance.UpdateVolume(volumeSlider.value);
    }

    public void NextEffect(Transform newSong_)
    {
        if(currentEffect != 2)
        {
            currentEffect++;
            VisualManager.Instance.ChangeEffect(m_Visuals.GetChild(currentEffect), newSong_);
        }
        else
        {
            currentEffect = 0;
            VisualManager.Instance.ChangeEffect(m_Visuals.GetChild(currentEffect), newSong_);
        }

        UpdateSelectedToggle(currentEffect);

        if (isTheEffectBeenChanged)
            ResetToggle(currentEffect);
    }

    public void PreviousEffect(Transform newSong_)
    {
        if (currentEffect != 0)
        {
            currentEffect--;
            VisualManager.Instance.ChangeEffect(m_Visuals.GetChild(currentEffect), newSong_);
        }
        else
        {
            currentEffect = 2;
            VisualManager.Instance.ChangeEffect(m_Visuals.GetChild(currentEffect), newSong_);
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

    public void UpdateTextureColor()
    {
        textureColor.r = redSlider.value;
        textureColor.g = greenSlider.value;
        textureColor.b = blueSlider.value;

        VisualManager.Instance.changeMediaPlayerColor(textureColor);
    }

}
