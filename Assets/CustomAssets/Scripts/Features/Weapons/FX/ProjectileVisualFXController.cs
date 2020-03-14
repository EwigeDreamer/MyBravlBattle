using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;

public class ProjectileVisualFXController : MonoValidate
{
    [SerializeField] ProjectileController m_ProjectileCtrl;

    [System.Serializable]
    public class ProjectileImpactInfo
    {
        [SerializeField] ProjectileKind m_pKind;
        //[SerializeField] 
    }

    [System.Serializable]
    public class TagEffectPair
    {
        [SerializeField] string m_Tag;
        //[SerializeField] ParticlesEffectPoint
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref m_ProjectileCtrl);
    }
    void Awake()
    {
        if (!ValidateGetComponent(ref m_ProjectileCtrl))
        {
            MyLogger.NotFoundObjectError<ProjectileVisualFXController, ProjectileController>();
            return;
        }
        Init();
    }

    void Init()
    {
        m_ProjectileCtrl.OnShoot += OnShootEvent;
        m_ProjectileCtrl.OnHit += OnHitEvent;
    }

    private void OnShootEvent(ProjectileInfo proj, PointInfo point)
    {
    }
    private void OnHitEvent(GameObject obj, ProjectileInfo proj, PointInfo hit)
    {
    }
}

