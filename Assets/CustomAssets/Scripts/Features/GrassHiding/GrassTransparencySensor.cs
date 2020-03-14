using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrassTransparencySensor : MonoBehaviour
{
    [SerializeField] Renderer grass;
    [SerializeField] Color visibilityColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField] Color invisibilityColor = new Color(1f, 1f, 1f, 0.25f);
    [SerializeField] string shaderColorName = "_Color";

    Tween colorTween = null;
    float time = 1f;

    int colorHash;

    private void Awake()
    {
        this.colorHash = Shader.PropertyToID(this.shaderColorName);
    }

    public void SetVisibility(bool state)
    {
        this.colorTween?.Kill();
        this.colorTween = DOVirtual.Float(this.time, state ? 1f : 0f, 0.1f, t =>
        {
            this.time = t;
            var color = Color.Lerp(this.invisibilityColor, this.visibilityColor, t);
            this.grass.material.SetColor(this.colorHash, color);
        }).OnComplete(() => this.colorTween = null);
    }
}
