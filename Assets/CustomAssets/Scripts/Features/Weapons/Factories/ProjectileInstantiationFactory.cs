using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInstantiationFactory : MonoBehaviour, IProjectileFactory
{
#pragma warning disable 649
    [SerializeField] Projectile[] m_Projectiles;
#pragma warning restore 649

    public Projectile GetObject(ProjectileKind info)
    {
        var projs = m_Projectiles;
        int count = projs.Length;
        for (int i = 0; i < count; ++i)
            if (projs[i].Info.kind == info)
                return Instantiate(projs[i]);
        return null;
    }
    public bool TryGetObject(ProjectileKind info, out Projectile obj)
    {
        obj = GetObject(info);
        return obj != null;
    }
}
