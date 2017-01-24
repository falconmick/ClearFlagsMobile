using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ClearFlagsImageEffect : MonoBehaviour
{
    static Material m_Material = null;
    protected Material material
    {
        get
        {
            if (m_Material == null)
            {
                m_Material = new Material(Shader.Find("Hidden/ClearFlagsImageEffect"));
                m_Material.hideFlags = HideFlags.DontSave;
            }
            return m_Material;
        }
    }

    protected void OnDisable()
    {
        if (m_Material)
        {
            DestroyImmediate(m_Material);
        }
    }

    private RenderTexture _pastFrame;
    private bool _isEnabled = false;

    // if your scene is using transparency, you will need to set this
    // to the lowest alpha you assign. However this will result in your background
    // camera color showing through. values from 0-1 clamped
    [SerializeField]
    [Range(0, 1)]
    private float _maxTransparency = 1.0f;

    void Start()
    {
        _pastFrame = new RenderTexture(Screen.width, Screen.height, 16);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (_isEnabled)
        {
            material.SetTexture("_PrevFrame", _pastFrame);
            material.SetFloat("_MaxTransparency", Mathf.Clamp(_maxTransparency, 0, 1));
            Graphics.Blit(src, dst, material);
        }
        else
        {
            Graphics.Blit(src, dst);
        }

        Graphics.Blit(RenderTexture.active, _pastFrame);
    }

    public void EnableEffect()
    {
        _isEnabled = true;
    }

    public void DisableEffect()
    {
        _isEnabled = false;
    }
}
