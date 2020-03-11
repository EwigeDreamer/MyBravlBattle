using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Menu;
using MyTools.Tween;
using MyTools.Extensions.Common;
using UnityEngine.UI;
using System;
using MyTools.Helpers;

public class GameUI : UIBase
{
    public event Action OnMenuPressed = delegate { };

    [SerializeField] Button menuBtn;
    [SerializeField] Joystick movement;
    [SerializeField] Joystick combat;
    [SerializeField] CombatJoystickSensor combatSensor;

    public Joystick MovementJoystick => movement;
    public Joystick CombatJoystick => combat;
    public CombatJoystickSensor CombatSensor => combatSensor;

    void Awake()
    {
        menuBtn.onClick.AddListener(() => OnMenuPressed());
    }
}