using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Pooling;
using System;
using MyTools.Helpers;

public class ParticlesPoint : MonoValidate, IPooledComponent
{
    [SerializeField] float m_LifeTime = 5f;
    [SerializeField] ParticleSystem[] m_Particlesystems;

    [ContextMenu("Get particles")]
    void GetParticles() { m_Particlesystems = GetComponentsInChildren<ParticleSystem>(); }

    public event Action OnPlay = delegate { };
    public event Action OnStop = delegate { };

    event Action Deactive = null;
    event Action IPooledComponent.Deactive
    { add { Deactive += value; } remove { Deactive -= value; } }

    void IPooledComponent.OnActivation() { }

    void IPooledComponent.OnDeactivation()
    {
        if (m_Coroutine != null) StopCoroutine(m_Coroutine);
        m_Coroutine = null;
    }

    public void Begin()
    {
        Play();
        m_Coroutine = StartCoroutine(Wait(m_LifeTime, Remove));
    }

    void Play()
    {
        var particles = m_Particlesystems;
        var count = particles.Length;
        for (int i = 0; i < count; ++i)
            particles[i].Play();
        OnPlay();
    }

    void Stop()
    {
        var particles = m_Particlesystems;
        var count = particles.Length;
        for (int i = 0; i < count; ++i)
            particles[i].Stop();
        OnStop();
    }

    void Remove()
    {
        if (Deactive != null) Deactive();
        else Destroy(GO);
    }

    Coroutine m_Coroutine = null;
    IEnumerator Wait(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
