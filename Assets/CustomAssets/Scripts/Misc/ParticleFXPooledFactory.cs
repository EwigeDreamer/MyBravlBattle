using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Factory;
using MyTools.Pooling;

public class ParticleFXPooledFactory : MonoBehaviour, IVisualEffectPointFactory
{
    public ParticlesFX GetObject(string info)
    {
        ObjectPool.I.TrySpawn(info, out var obj);
        return obj?.GetComponent<ParticlesFX>();
    }

    public bool TryGetObject(string info, out ParticlesFX obj)
    {
        obj = GetObject(info);
        return obj != null;
    }
}
