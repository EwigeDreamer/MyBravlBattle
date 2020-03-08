using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyTools.Helpers;
using MyTools.Extensions.RichText;
using MyTools.Extensions.Vectors;

[RequireComponent(typeof(WheelSlider))]
public class WheelSliderVisual : MonoValidate
{
#pragma warning disable 649
    [SerializeField] WheelSlider m_Slider;
    [SerializeField] RectTransform m_Mask;
    [SerializeField] RectTransform m_WheelImage;
#pragma warning restore 649


    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateGetComponent(ref m_Slider);
    }
    void Awake()
    {
        ValidateGetComponent(ref m_Slider);
        m_Slider.OnUpdateDelta += UpdateDelta;
    }

    private void Start()
    {
        PrepareGraphics();
    }

    private void UpdateDelta(WheelSlider.DeltaInfo delta)
    {
        //m_WheelImage.anchoredPosition = Vector2.zero + new Vector2(0f, delta.pixels);
        UpdatePosition(delta.pixels);
    }

    void ExpandToParent(Transform tr)
    {
        var rtr = tr as RectTransform;
        if (rtr == null) return;
        rtr.localScale = Vector3.one;
        rtr.anchorMin = Vector2.zero;
        rtr.anchorMax = Vector2.one;
        rtr.offsetMin = Vector2.zero;
        rtr.offsetMax = Vector2.zero;
    }

    void PrepareGraphics()
    {
        var slider = m_Slider;
        var containerTr = (new GameObject("WheelSliderVisualContainer")).transform;
        containerTr.SetParent(m_Mask);
        var containerRTr = containerTr.gameObject.AddComponent<RectTransform>();
        ExpandToParent(containerRTr);
        containerRTr.anchorMin = new Vector2(0.5f, 0.5f);
        containerRTr.anchorMax = new Vector2(0.5f, 0.5f);
        containerRTr.sizeDelta = slider.Size;
        var refer = m_WheelImage;
        RectTransform obj1 = Instantiate(refer, containerRTr);
        ExpandToParent(obj1);
        RectTransform obj2 = Instantiate(refer, containerRTr);
        ExpandToParent(obj2);
        if (IsHorizontal)
        {
            obj1.anchoredPosition = new Vector2(-slider.Size.x / 2f, 0f);
            obj2.anchoredPosition = new Vector2(slider.Size.x / 2f, 0f);
        }
        else
        {
            obj1.anchoredPosition = new Vector2(0f, slider.Size.y / 2f);
            obj2.anchoredPosition = new Vector2(0f, -slider.Size.y / 2f);
        }
        Destroy(refer.gameObject);
        m_WheelImage = containerRTr;
    }

    void UpdatePosition(float delta)
    {
        var rtr = m_WheelImage;
        var pos = rtr.anchoredPosition;
        var halfSize = m_Slider.Size / 2f;
        if (IsHorizontal)
        {
            pos.x += delta;
            pos.x = Loop(-halfSize.x, halfSize.x, pos.x);
        }
        else
        {
            pos.y += delta;
            pos.y = Loop(-halfSize.y, halfSize.y, pos.y);
        }
        rtr.anchoredPosition = pos;
    }

    bool IsHorizontal => m_Slider.Orient == WheelSlider.Orientation.Horizontal;

    float Loop(float min, float max, float value)
    {
        var range = max - min;
        if (value > max)
        {
            value = min + (value - max) % range;
        }
        else if (value < min)
        {
            value = max - (min - value) % range;
        }
        return value;
    }
}
