using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ImpactEffect
{
    public abstract void Execute(GameObject go, ProjectileInfo projectile, PointInfo info);
}

