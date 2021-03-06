﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Buckshot : Projectile
{
    [SerializeField] Projectile bulletPrefab;
    [SerializeField] ProjectileKind subKind;
    [SerializeField] int bulletCount = 10;
    [SerializeField] float scatterAngle = 10f;
    protected override void RpcGo() => SpawnBuckshot();

    protected override void RpcStop() { }

    void SpawnBuckshot()
    {
        if (NetworkServer.active)
        {
            var pos = transform.position;
            var dir = transform.forward;
            for (int i = 0; i < this.bulletCount; ++i)
            {
                var randomDir = dir + Random.insideUnitSphere * Mathf.Tan(this.scatterAngle * Mathf.Deg2Rad);
                var proj = Instantiate(this.bulletPrefab, pos, Quaternion.LookRotation(randomDir));
                ProjectileController.I.RegisterAlienProjectile(proj, subKind, Info.weapon, pos, randomDir);
            }
        }
        Finish();
    }
}
