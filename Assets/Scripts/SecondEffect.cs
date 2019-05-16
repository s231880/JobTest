using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondEffect : Visual
{
    [SerializeField]
    private float m_fSampleModifer = 10.0f;
    private int m_iTextureSize = 256;

    [SerializeField]
    private Texture2D m_Texture;
    private float[] m_Samples;

    private Material m_AudioMaterial;

    public override void Init()
    {
        m_Texture = new Texture2D(m_iTextureSize, 1, TextureFormat.RFloat, false);

        //Gather surface to render the texture on
        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();

        if (null != renderer)
        {
            m_AudioMaterial = renderer.sharedMaterial;
            m_AudioMaterial.mainTexture = m_Texture;
        }

        m_Samples = new float[m_iTextureSize];

        DisableMeshRender(renderer);
    }

    public override void OnSpectrumUpdate(float[] samples)
    {
        int iNumSamples = samples.Length;
        int iSamplesPerPixel = iNumSamples / m_iTextureSize;
        Color white = Color.white;

        //Populate texture with data
        for (int i = 0; i < m_iTextureSize; i++)
        {

            int iIndex = i * iSamplesPerPixel;
            float value = samples[iIndex] * m_fSampleModifer;
            m_Samples[i] = Mathf.Lerp(m_Samples[i], value, Mathf.Sin(Time.deltaTime) * 10f);
            m_Texture.SetPixel(0, i, white * (m_Samples[i] * iIndex));
        }

        //Save changes on texture
        m_Texture.Apply();
    }

    public override void ChangeMaterialColor(Color32 newColor_)
    {
        m_AudioMaterial.SetColor("_Color", newColor_);
    }
}
