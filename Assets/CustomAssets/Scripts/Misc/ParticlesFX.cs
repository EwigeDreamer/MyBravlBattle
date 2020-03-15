using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Pooling;
using System;
using MyTools.Helpers;
using DG.Tweening;

public class ParticlesFX : MonoValidate, IPooledComponent
{
    [SerializeField] ParticleSystem particle;
    Tween tween = null;

    Action Deactive = null;
    event Action IPooledComponent.Deactive
    { add { Deactive += value; } remove { Deactive -= value; } }

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateGetComponent(ref particle);
    }

    void IPooledComponent.OnActivation() 
    {
        particle.Stop();
        particle.Play();
        tween = DOVirtual.DelayedCall(particle.main.duration, Remove);
    }
    void IPooledComponent.OnDeactivation()
    {
        particle.Stop();
        tween?.Kill();
        tween = null;
    }

    void Remove()
    {
        tween = null;
        if (Deactive != null) Deactive();
        else Destroy(GO);
    }
}
