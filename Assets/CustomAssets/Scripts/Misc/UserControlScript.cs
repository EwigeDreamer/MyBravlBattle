using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;
using System;
using MyTools.ValueInfo;

public class UserControlScript : MonoValidate
{
    public event Action OnShoot = delegate { };
    public event Action<Vector2> OnDirectionalAim = delegate { };
    public event Action<Vector2> OnDirectionalShoot = delegate { };
    public event Action<Vector2> OnMove = delegate { };

    [SerializeField] GameUI gameUI;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref gameUI);
    }

    private void Awake()
    {
        gameUI.OnMenuPressed += () => PopupManager.OpenPopup<GameMenuPopup>();
        gameUI.CombatSensor.OnClick += () => OnShoot();
        gameUI.CombatSensor.OnRelease += () => OnDirectionalShoot(gameUI.CombatJoystick.Direction);
    }

    private void FixedUpdate()
    {
        var move = gameUI.MovementJoystick;
        var aim = gameUI.CombatJoystick;
        if (!move.Horizontal.IsVerySmall() || !move.Vertical.IsVerySmall()) OnMove(move.Direction);
        if (!aim.Horizontal.IsVerySmall() || !aim.Vertical.IsVerySmall()) OnDirectionalAim(aim.Direction);
    }
}
