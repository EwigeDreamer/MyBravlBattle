using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CombatJoystickSensor : MonoBehaviour, 
    IPointerDownHandler, 
    IPointerClickHandler, 
    IPointerUpHandler
{
    public event Action OnPress = delegate { };
    public event Action OnClick = delegate { };
    public event Action OnRelease = delegate { };

    int pointer = -1;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (pointer != -1) return;
        pointer = eventData.pointerId;
        OnPress();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (pointer != eventData.pointerId) return;
        pointer = -1;
        OnClick();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (pointer != eventData.pointerId) return;
        pointer = -1;
        OnRelease();
    }
}
