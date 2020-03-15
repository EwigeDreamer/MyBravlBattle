using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageImpactEffect : ImpactEffect
{
    int damage;
    public DamageImpactEffect(int damage)
    {
        this.damage = damage;
    }
    public override void Execute(GameObject go, ProjectileInfo proj, PointInfo info)
    {
        var health = go.GetComponent<NetworkPlayerHealth>();
        if (health == null) return;
        health.SetDamage(this.damage, proj.weapon.owner);
    }
}

