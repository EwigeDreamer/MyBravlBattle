using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MyTools.Extensions.Vectors;
using System;
using MyTools.Extensions.GameObjects;

public class NetworkPlayerCombat : NetworkBehaviour
{
    [SerializeField] NetworkPlayerView view;
    [SerializeField] Transform weaponPoint;
    [SerializeField] LayerMask hitMask;
    [SerializeField] AudioSource audio;

    public event Action<WeaponKind> OnSetWeapon = delegate { };

    [SyncVar] WeaponKind currentKind = WeaponKind.Unknown;

    Weapon weapon = null;

    private void OnValidate()
    {
        gameObject.ValidateGetComponent(ref this.view);
    }

    private void Awake()
    {
        this.view.OnChangeVisible += state => weapon?.Model.SetVisible(state);
    }

    public void Refresh()
    {
        if (this.currentKind == WeaponKind.Unknown) return;
        CmdSetWeapon(currentKind);
        weapon?.Model.SetVisible(this.view.IsVisible);
    }

    [Command] public void CmdSetWeapon(WeaponKind kind) => RpcSetWeapon(kind);
    [ClientRpc] void RpcSetWeapon(WeaponKind kind)
    {
        if (this.weapon != null && this.weapon.Info.kind == kind) return;
        this.weapon?.Dispose();
        WeaponInfo info = new WeaponInfo
        {
            owner = gameObject,
            kind = kind,
            mask = this.hitMask,
            audio = this.audio,
            isServer = this.isServer
        };
        this.weapon = new Weapon(weaponPoint, weaponPoint, info);
        OnSetWeapon(kind);
    }

    [Command] public void CmdShoot(Vector2 dir)
    {
        this.weapon?.Shoot(dir.ToV3_x0y());
    }
}
