using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyTools.Helpers;
using MyTools.Singleton;
using UnityEngine.Networking;
using DG.Tweening;

public enum ProjectileEventType { Shoot, Hit }

public struct ProjectileInfo
{
    public Projectile instance;
    public WeaponInfo weapon;
    public ProjectileKind kind;
}
public struct PointInfo
{
    public Vector3 point;
    public Vector3 direction;
    public Vector3 normal;
}

public class ProjectileController : MonoSingleton<ProjectileController>
{
    public event Action<ProjectileInfo, PointInfo> OnShoot = delegate { };
    public event Action<GameObject, ProjectileInfo, PointInfo> OnHit = delegate { };

    [SerializeField] WeaponController weaponCtrl;
    [SerializeField] ProjectilePrefabs projectiles;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref weaponCtrl);
    }

    protected override void Awake()
    {
        base.Awake();
        weaponCtrl.OnShoot += CreateProjectile;
    }

    private void CreateProjectile(WeaponInfo wInfo, Vector3 pos, Vector3 dir)
    {
        var pKind = WeaponStaticData.WeaponProjectileBindData[wInfo.kind];
        var proj = Instantiate(projectiles.ProjectilePrefabDict[pKind], pos, Quaternion.LookRotation(dir));
        NetworkServer.Spawn(proj.gameObject);
        Subscribe(proj);
        proj.Init(wInfo, pKind, pos, dir);
        OnShoot(proj.Info, new PointInfo { point = pos, direction = dir, normal = dir });
    }

    void Subscribe(Projectile proj)
    {
        proj.OnHit += OnHitEvent;
        proj.OnFinish += DestroyProjectile;
    }

    public void RegisterAlienProjectile(Projectile proj, ProjectileKind kind, WeaponInfo wInfo, Vector3 pos, Vector3 dir)
    {
        if (!NetworkServer.active) return;
        if (proj == null) return;
        NetworkServer.Spawn(proj.gameObject);
        Subscribe(proj);
        proj.Init(wInfo, kind, pos, dir);
        OnShoot(proj.Info, new PointInfo { point = pos, direction = dir, normal = dir });
    }

    private void DestroyProjectile(Projectile proj)
    {
        DOVirtual.DelayedCall(1f, () =>
        {
            proj.OnHit -= OnHitEvent;
            proj.OnFinish -= DestroyProjectile;
            NetworkServer.Destroy(proj.gameObject);
        });
    }

    private void OnHitEvent(GameObject obj, ProjectileInfo info, PointInfo hit) => OnHit(obj, info, hit);
}


