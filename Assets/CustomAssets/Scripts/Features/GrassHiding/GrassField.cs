using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassField : MonoBehaviour
{
    [SerializeField] Renderer grass;
    [SerializeField] Color visibilityColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField] Color invisibilityColor = new Color(1f, 1f, 1f, 0.25f);
    [SerializeField] string shaderColorName = "_Color";

    int colorHash;

    private void Awake()
    {
        this.colorHash = Shader.PropertyToID(this.shaderColorName);
    }

    public void SetVisibility(bool state)
    {
        this.grass.material.SetColor(this.colorHash, state ? this.visibilityColor : this.invisibilityColor);
    }
}
