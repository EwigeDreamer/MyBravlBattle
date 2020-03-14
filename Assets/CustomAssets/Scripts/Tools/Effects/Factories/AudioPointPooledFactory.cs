using System.Collections;
using System.Collections.Generic;
using MyTools.Factory;
using UnityEngine;
using MyTools.Pooling;

public class AudioPointPooledFactory : MonoBehaviour, IAudioPointFactory
{
#pragma warning disable 649
    [SerializeField] [PoolKey] string m_AudioEffectPointKey;

#pragma warning restore 649

    public AudioPoint GetObject()
    {
        ObjectPool.I.TrySpawn(m_AudioEffectPointKey, out var obj);
        return obj?.GetComponent<AudioPoint>();
    }

    public bool TryGetObject(out AudioPoint obj)
    {
        obj = GetObject();
        return obj != null;
    }
}
