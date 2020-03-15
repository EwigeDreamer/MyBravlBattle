using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;
using OneLine;
using MyTools.Pooling;
using System.Collections.ObjectModel;

public class ProjectileVisualFXController : MonoValidate
{
    [SerializeField] ProjectileController projectileCtrl;

    [SerializeField, OneLine(Header = LineHeader.Short)]
    ProjectileEffectInfo[] infoList;

    Dictionary<ProjectileKind, ProjectileEffectInfo> dict = new Dictionary<ProjectileKind, ProjectileEffectInfo>();



    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref this.projectileCtrl);
    }
    void Awake()
    {
        this.dict = new Dictionary<ProjectileKind, ProjectileEffectInfo>();
        foreach (var item in this.infoList) this.dict[item.kind] = item;
        projectileCtrl.OnShoot += OnShootEvent;
        projectileCtrl.OnHit += OnHitEvent;
    }

    private void OnShootEvent(ProjectileInfo proj, PointInfo point)
    {
        var kind = proj.kind;
        if (!this.dict.TryGetValue(kind, out var info)) return;
        Debug.LogWarning("SHOOT EFFECT!");
    }
    private void OnHitEvent(GameObject obj, ProjectileInfo proj, PointInfo hit)
    {
        var kind = proj.kind;
        if (!this.dict.TryGetValue(kind, out var info)) return;
        Debug.LogWarning("IMPACT EFFECT!");
    }


    [System.Serializable]
    public class ProjectileEffectInfo
    {
        public ProjectileKind kind;
        [PoolKey] public string shoot;
        [PoolKey] public string impact;
    }
}

