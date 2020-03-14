using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyTools.Helpers;
using MyTools.Singleton;

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
        Subscribe(proj);
        proj.Init(wInfo, pos, dir);
        OnShoot(proj.Info, new PointInfo { point = pos, direction = dir, normal = dir });
    }

    void Subscribe(Projectile proj)
    {
        proj.OnHit += OnHitEvent;
        proj.OnFinish += Unsubscribe;
    }

    private void Unsubscribe(Projectile proj)
    {
        proj.OnHit -= OnHitEvent;
        proj.OnFinish -= Unsubscribe;
    }

    private void OnHitEvent(GameObject obj, ProjectileInfo info, PointInfo hit) => OnHit(obj, info, hit);
}


