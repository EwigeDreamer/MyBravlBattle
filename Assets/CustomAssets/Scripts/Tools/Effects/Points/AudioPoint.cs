using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Pooling;
using System;
using MyTools.Helpers;
using DG.Tweening;

public class AudioPoint : MonoValidate, IPooledComponent
{
    [SerializeField] AudioSource m_Audio;
    Tween tween = null;

    Action Deactive = null;
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
        tween?.Kill();
        tween = null;
    }

    public void PlayOneShoot(AudioClip clip, int prority = 100)
    {
        if (clip == null) { Remove(); return; }
        var audio = m_Audio;
        if (audio == null) { Remove(); return; }
        var time = clip.length;
        audio.priority = prority;
        audio.PlayOneShot(clip);
        tween = DOVirtual.DelayedCall(time, Remove);
    }
    public void PlayOneShoot(Vector3 position, AudioClip clip, int prority = 100)
    {
        TR.position = position;
        PlayOneShoot(clip, prority);
    }

    void Remove()
    {
        tween = null;
        if (Deactive != null) Deactive();
        else Destroy(GO);
    }
}
