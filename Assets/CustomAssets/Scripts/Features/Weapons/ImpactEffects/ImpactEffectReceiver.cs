using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;

public class ImpactEffectReceiver : ImprovedBehaviour
{
    public void ApplyEffect(ImpactEffect effect, ProjectileInfo projectile, PointInfo info)
    { effect.Execute(GO, projectile, info); }
}