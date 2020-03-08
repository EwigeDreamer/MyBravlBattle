using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MyTools.Helpers;

public class WheelSliderTouchpad : MonoBehaviour, IPointerDownHandler, IEndDragHandler, IDragHandler
{
    //public event System.Action<Vector2> OnUpdateValue = delegate { };
    public event System.Action<Vector2> OnUpdateDelta = delegate { };

    [SerializeField] float m_LerpFactor = 5f;
    [SerializeField] float m_Sensitivity = 1f;

    //[Header("Debug")]
    //public 
        //Vector2 m_Value = default;
    //public 
        bool m_Dragging = false;
    //public 
        Vector2 m_PrevPos = default;
    //public 
        Vector2 m_Delta = default;
    //public 
        int m_PointerId = -1;

    public Vector2 Delta
    {
        get => m_Delta;
        private set
        {
            m_Delta = value;
            OnUpdateDelta(value);
        }
    }
    //public Vector2 Value
    //{
    //    get => m_Value;
    //    private set
    //    {
    //        m_Value = value;
    //        OnUpdateValue(value);
    //    }
    //}

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (m_Dragging) return;
        m_PointerId = eventData.pointerId;
        m_PrevPos = eventData.position;
        m_Dragging = true;
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId != m_PointerId) return;
        Delta = eventData.delta * m_Sensitivity;
        m_PrevPos = eventData.position;
        //Value += Delta;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerId != m_PointerId) return;
        Delta = (eventData.position - m_PrevPos) * m_Sensitivity;
        m_Dragging = false;
    }

    private void Update()
    {
        if (m_Dragging) return;
        Delta = Vector2.Lerp(Delta, Vector2.zero, TimeManager.UnscaledDeltaTime * m_LerpFactor);
        //Value += Delta;
    }
}
