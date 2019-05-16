using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualManager : MonoBehaviour {

    public static VisualManager Instance;
    
    //Current playing effect
    public static Transform currentEffect_;

    private Dictionary<Transform, Visual> m_DictOfVisuals = new Dictionary<Transform, Visual>();

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

    public void CreateVisual(Transform obj)
    {
        if (false == m_DictOfVisuals.ContainsKey(obj))
        {
            Visual visual = obj.gameObject.GetComponent<Visual>();
            if (null != visual)
            {
                visual.Init();
                m_DictOfVisuals.Add(obj, visual);
            }
        }
    }

    public void RemoveVisual(Transform obj)
    {
        if(true == m_DictOfVisuals.ContainsKey(obj))
        {
            m_DictOfVisuals[obj].DeInit();
            m_DictOfVisuals.Remove(obj);
        }
    }

    public void AssignAudioToVisual(Transform visual, Transform Audio)
    {
        Audio audio = AudioManager.Instance.GetAudio(Audio);
        
        if (true == m_DictOfVisuals.ContainsKey(visual))
        {
            Visual effect = m_DictOfVisuals[visual];
            if (null != audio)
            {
                audio.OnAudioSpectrumUpdate += effect.OnSpectrumUpdate;
            }
        }
    }

    /// <summary>
    /// Change the effect when changing the song
    /// </summary>
    public void ChangeEffect(Transform newEffect_, Transform newSong_)
    {
        //Disable the previous effect meshrender
        m_DictOfVisuals[currentEffect_].DisableMeshRender(m_DictOfVisuals[currentEffect_].GetComponentInChildren<MeshRenderer>());
        //Setting the new effect
        currentEffect_ = newEffect_;
        //Activating the new effect meshrender
        m_DictOfVisuals[currentEffect_].EnableMeshRender(m_DictOfVisuals[currentEffect_].GetComponentInChildren<MeshRenderer>());
        //Activating the effect
        AssignAudioToVisual(currentEffect_, newSong_);
    }

     /// <summary>
     /// Change the texture color
     /// </summary>
    public void changeMediaPlayerColor(Color32 newColor_)
    {
        m_DictOfVisuals[currentEffect_].ChangeMaterialColor(newColor_);
    }
}
