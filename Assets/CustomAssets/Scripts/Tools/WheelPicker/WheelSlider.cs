using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyTools.Helpers;
using MyTools.Extensions.RichText;
using MyTools.Extensions.Vectors;

[RequireComponent(typeof(WheelSliderTouchpad))]
public class WheelSlider : MonoValidate
{
    public struct DeltaInfo
    {
        public float pixels;
        public float normalized;
    }
    public event System.Action<DeltaInfo> OnUpdateDelta = delegate { };
    public enum Orientation { Horizontal, Vertical }

#pragma warning disable 649
    [SerializeField] WheelSliderTouchpad m_Touchpad;
    [SerializeField] Orientation m_Orient;
#pragma warning restore 649

    RectTransform m_RTr;

    public Orientation Orient => m_Orient;

    //Vector2 m_Size;
    //public Vector2 Size => m_Size;
    public Vector2 Size => m_RTr.sizeDelta;

    //[SerializeField] RectTransform m_WheelImage;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateGetComponent(ref m_Touchpad);
    }

    void Awake()
    {
        ValidateGetComponent(ref m_Touchpad);
        m_Touchpad.OnUpdateDelta += UpdateDelta;
        m_RTr = TR as RectTransform;
        //ExpandToParent();
        //m_Size = RectTransformUtility.CalculateRelativeRectTransformBounds(TR).size;
        //m_Size = RectTransformUtility.CalculateRelativeRectTransformBounds(TR).size;
    }

    private void UpdateDelta(Vector2 deltaV2)
    {
        bool isHorizontal = m_Orient == Orientation.Horizontal;
        var d = isHorizontal ? deltaV2.x : deltaV2.y;
        var sizeAxis = isHorizontal ? Size.x : Size.y;
        UpdateDeltaAxis(new DeltaInfo { pixels = d, normalized = d / sizeAxis });

        //m_WheelImage.anchoredPosition += Vector3.Project(deltaV2.ToV3_xy0(), isHorizontal ? Vector3.right : Vector3.up).ToV2_xy();
    }

    void UpdateDeltaAxis(DeltaInfo delta)
    {
        //Debug.Log(delta.delta.ToString("F4").RichText().Bold().InGreen().ToString() + " " + delta.deltaNormalized.ToString("F2").RichText().Bold().InRed().ToString());
        OnUpdateDelta(delta);
    }
}
