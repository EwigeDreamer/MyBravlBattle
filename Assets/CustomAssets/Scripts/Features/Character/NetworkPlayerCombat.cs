using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MyTools.Extensions.Vectors;
using System;

public class NetworkPlayerCombat : NetworkBehaviour
{
    [SerializeField] Transform weaponPoint;
    [SerializeField] LayerMask hitMask;
    [SerializeField] AudioSource audio;

    public event Action<WeaponKind> OnSetWeapon = delegate { };

    [SyncVar] WeaponKind currentKind = WeaponKind.Unknown;

    Weapon weapon = null;

    public void Refresh()
    {
        if (this.currentKind == WeaponKind.Unknown) return;
        CmdSetWeapon(currentKind);
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
