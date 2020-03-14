    using UnityEngine;
    using System;
    using MyTools.Pooling;
    using MyTools.Helpers;
using Object = UnityEngine.Object;

public class Weapon : IDisposable
{
    Transform hand;
    Transform point;
    GameObject model;
    WeaponInfo info;
    public Transform Hand => hand;
    public Transform Point => point;
    public WeaponInfo Info => info;

    public event Action<WeaponInfo, Vector3, Vector3> OnShoot = delegate { };


    public Weapon(Transform hand, Transform point, WeaponInfo info)
    {
        this.hand = hand;
        this.point = point;
        this.info = info;
        this.model = WeaponController.I.GetWeaponModel(info.kind);
        this.model.transform.SetParent(hand);
        this.model.transform.localPosition = Vector3.zero;
        this.model.transform.localRotation = Quaternion.identity;
        if (info.isServer) WeaponController.I.Subscribe(this);
    }

    public void Dispose()
    {
        WeaponController.I.Unsubscribe(this);
        Object.Destroy(this.model);
        this.model = null;
    }

    public void Shoot(Vector3 dir)
    {
        var info = this.info;
        OnShoot(info, this.point.position, dir);
    }
}

