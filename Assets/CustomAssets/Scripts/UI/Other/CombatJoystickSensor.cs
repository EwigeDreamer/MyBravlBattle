using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CombatJoystickSensor : MonoBehaviour, 
    IPointerDownHandler,
    IPointerUpHandler,
    IDragHandler
{
    public event Action OnPress = delegate { };
    public event Action OnClick = delegate { };
    public event Action OnRelease = delegate { };

    [SerializeField] Joystick combat;

    int pointer = -1;
    bool isDragging = false;
    Vector2 lastDir;

    public Vector2 LastDir => this.lastDir;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (this.pointer != -1) return;
        this.pointer = eventData.pointerId;
        this.isDragging = false;
        OnPress();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (this.pointer != eventData.pointerId) return;
        this.pointer = -1;
        if (this.isDragging) OnRelease();
        else OnClick();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId != this.pointer) return;
        this.lastDir = this.combat.Direction;
        this.isDragging = true;
    }
}
