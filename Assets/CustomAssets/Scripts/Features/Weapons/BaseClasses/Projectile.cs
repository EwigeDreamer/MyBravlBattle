using UnityEngine;
using System;
using MyTools.Pooling;
using MyTools.Helpers;
using UnityEngine.Networking;

public abstract class Projectile : NetworkBehaviour
{
    public event Action<GameObject, ProjectileInfo, PointInfo> OnHit = delegate { };
    public event Action<Projectile> OnFinish = delegate { };

    [SerializeField] new AudioSource audio;
    ProjectileInfo info;
    public ProjectileInfo Info => info;
    public AudioSource Audio => audio;


    public void Init(WeaponInfo weapon, Vector3 position, Vector3 direction)
    {
        info.weapon = weapon;
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(direction);
        RpcGo();
    }

    protected void Hit(Collider col, PointInfo hit)
    {
        GameObject obj;
        Rigidbody rb = col.attachedRigidbody;
        if (rb != null)
            obj = rb.gameObject;
        else
            obj = col.gameObject;
        OnHit(obj, info, hit);
    }

    protected void Finish()
    {
        OnFinish(this);
    }

    [ClientRpc]
    protected abstract void RpcGo();
    [ClientRpc]
    protected abstract void RpcStop();
}

