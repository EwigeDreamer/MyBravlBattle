using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Menu;
using MyTools.Tween;
using MyTools.Extensions.Common;
using UnityEngine.UI;
using System;
using MyTools.Helpers;
using TMPro;

public class ChooseWeaponUI : UIBase
{
    [SerializeField] Button pistolBtn;
    [SerializeField] Button rifleBtn;
    [SerializeField] Button shotgunBtn;

    [SerializeField] Image pistolBg;
    [SerializeField] Image rifleBg;
    [SerializeField] Image shotgunBg;

    [SerializeField] Sprite selectedBg;
    [SerializeField] Sprite unselectedBg;

    public event Action OnPistolPressed = delegate { };
    public event Action OnRiflePressed = delegate { };
    public event Action OnShotgunPressed = delegate { };

    void Awake()
    {
        pistolBtn.onClick.AddListener(() => OnPistolPressed());
        rifleBtn.onClick.AddListener(() => OnRiflePressed());
        shotgunBtn.onClick.AddListener(() => OnShotgunPressed());
    }

    public void SetSelected(WeaponKind kind)
    {
        pistolBg.sprite = kind == WeaponKind.Pistol ? selectedBg : unselectedBg;
        rifleBg.sprite = kind == WeaponKind.Rifle ? selectedBg : unselectedBg;
        shotgunBg.sprite = kind == WeaponKind.Shotgun ? selectedBg : unselectedBg;
    }
}
