﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;

public class CharacterControlMediator : MonoValidate
{
    [SerializeField] UserControlScript userControl;
    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref userControl);
    }

    private void Awake()
    {
        userControl.OnMove += dir => PlayerController.I.LocalPlayer.Motor.CmdMove(dir);
    }
}
