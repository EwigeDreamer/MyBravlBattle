using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Singleton;

public class ChooseWeaponScript : MonoSingleton<ChooseWeaponScript>
{
    [SerializeField] ChooseWeaponUI chooseUI;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref this.chooseUI);
    }
    protected override void Awake()
    {
        base.Awake();
        this.chooseUI.OnPistolPressed += () => PlayerController.I.LocalPlayer.Combat.CmdSetWeapon(WeaponKind.Pistol);
        this.chooseUI.OnRiflePressed += () => PlayerController.I.LocalPlayer.Combat.CmdSetWeapon(WeaponKind.Rifle);
        this.chooseUI.OnShotgunPressed += () => PlayerController.I.LocalPlayer.Combat.CmdSetWeapon(WeaponKind.Shotgun);

        //PlayerController.I.OnChangeWeapon -= OnWeapoChangeEvent;
        //PlayerController.I.OnChangeWeapon += OnWeapoChangeEvent;
    }

    //protected override void OnDestroy()
    //{
        //base.OnDestroy();
        //PlayerController.I.OnChangeWeapon -= OnWeapoChangeEvent;
    //}
    //private void Start()
    //{
    //    var kind = PlayerController.I.LocalPlayer.Combat.CurrentWeapon;
    //    this.chooseUI.SetSelected(kind);
    //}
    //void OnWeapoChangeEvent(WeaponKind kind)
    //{
    //    this.chooseUI.SetSelected(kind);
    //}
}
