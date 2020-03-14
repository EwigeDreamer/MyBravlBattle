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
        ValidateFind(ref this.gameUI);
    }

    private void Awake()
    {
        this.gameUI.OnMenuPressed += () => PopupManager.OpenPopup<GameMenuPopup>();
        this.gameUI.CombatSensor.OnClick += () => OnShoot();
        this.gameUI.CombatSensor.OnRelease += () => OnDirectionalShoot(this.gameUI.CombatSensor.LastDir);
    }

    private void FixedUpdate()
    {
        var move = this.gameUI.MovementJoystick;
        var aim = this.gameUI.CombatJoystick;
        if (!move.Horizontal.IsVerySmall() || !move.Vertical.IsVerySmall()) OnMove(move.Direction);
        if (!aim.Horizontal.IsVerySmall() || !aim.Vertical.IsVerySmall()) OnDirectionalAim(aim.Direction);
    }
}
