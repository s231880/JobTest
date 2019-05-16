using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Visual : MonoBehaviour
{
    public virtual void Init() { }
    public virtual void DeInit() { }
    public virtual void OnSpectrumUpdate(float[] samples) { }

    public virtual void ChangeMaterialColor(Color32 newColor_) { }

    public void DisableMeshRender(MeshRenderer render_)
    {
        render_.enabled = false;
    }

    public void EnableMeshRender(MeshRenderer render_)
    {
        render_.enabled = true;
    }
}
