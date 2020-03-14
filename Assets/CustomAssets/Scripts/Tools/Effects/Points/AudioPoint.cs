using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Pooling;
using System;
using MyTools.Helpers;

public class AudioPoint : MonoValidate, IPooledComponent
{
    [SerializeField] AudioSource m_Audio;

    event Action Deactive = null;
    event Action IPooledComponent.Deactive
    { add { Deactive += value; } remove { Deactive -= value; } }

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateGetComponent(ref m_Audio);
    }
    void Awake()
    {
        ValidateGetComponent(ref m_Audio);
        m_Audio?.Stop();
    }

    void IPooledComponent.OnActivation() { }

    void IPooledComponent.OnDeactivation()
    {
        m_Audio?.Stop();
        if (m_Coroutine != null)
        {
            StopCoroutine(m_Coroutine);
            m_Coroutine = null;
        }
    }

    public void PlayOneShoot(AudioClip clip)
    {
        if (clip == null) { Remove(); return; }
        var audio = m_Audio;
        if (audio == null) { Remove(); return; }
        var time = clip.length;
        audio.PlayOneShot(clip);
        m_Coroutine = StartCoroutine(Wait(time, Remove));
    }
    public void PlayOneShoot(Vector3 position, AudioClip clip)
    {
        TR.position = position;
        PlayOneShoot(clip);
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
