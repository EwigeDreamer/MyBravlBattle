    using UnityEngine;
    using System;
    using MyTools.Pooling;
    using MyTools.Helpers;
using Object = UnityEngine.Object;

public class Weapon : IDisposable
{
    Transform hand;
    Transform point;
    WeaponModel model;
    WeaponInfo info;
    public Transform Hand => this.hand;
    public Transform Point => this.point;
    public WeaponInfo Info => this.info;
    public WeaponModel Model => this.model;

    public event Action<WeaponInfo, Vector3, Vector3> OnShoot = delegate { };


    public Weapon(Transform hand, Transform point, WeaponInfo info)
    {
        this.hand = hand;
        this.point = point;
        this.info = info;
        this.model = WeaponController.I.GetWeaponModel(info.kind);
        this.model.TR.SetParent(hand);
        this.model.TR.localPosition = Vector3.zero;
        this.model.TR.localRotation = Quaternion.identity;
        if (info.isServer) WeaponController.I.Subscribe(this);
    }

    public void Dispose()
    {
        WeaponController.I.Unsubscribe(this);
        Object.Destroy(this.model.gameObject);
        this.model = null;
    }

    public void Shoot(Vector3 dir)
    {
        OnShoot(this.info, this.point.position, dir);
    }
}

