using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;
using System;

    public struct TransmitInfo
    {
        public GameObject transmitter;
        public GameObject receiver;
        public ProjectileInfo projectile;
        public PointInfo point;
    }

public class ImpactEffectTransmitter : MonoValidate
{
    [SerializeField] ProjectileController m_ProjectileCtrl;
    public event Action<TransmitInfo> OnTransmit = delegate { };

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref m_ProjectileCtrl);
    }
    void Awake()
    {
        if (!ValidateGetComponent(ref m_ProjectileCtrl))
        {
            MyLogger.NotFoundObjectError<ImpactEffectTransmitter, ProjectileController>();
            return;
        }
        Init();
    }

    void Init()
    {
        m_ProjectileCtrl.OnHit += TransmitEffects;
    }

    private void TransmitEffects(GameObject obj, ProjectileInfo proj, PointInfo hit)
    {
        var receiver = obj.GetComponent<ImpactEffectReceiver>();
        if (receiver == null)
        {
            MyLogger.ObjectLogFormat<ImpactEffectTransmitter>("Object {0} has no {1}!", obj, typeof(ImpactEffectReceiver));
            return;
        }
        if (!WeaponStaticData.ProjectileEffectBindData.TryGetValue(proj.kind, out var effects))
        {
            MyLogger.ObjectErrorFormat<ImpactEffectTransmitter>("Weapon's BindData don't contain effects for \"{0}\" kind!", proj.kind);
            return;
        }
        int count = effects.Count;
        for (int i = 0; i < count; ++i)
            receiver.ApplyEffect(effects[i], proj, hit);
        OnTransmit(new TransmitInfo
        {
            transmitter = proj.weapon.owner,
            receiver = obj,
            projectile = proj,
            point = hit
        });
    }
}