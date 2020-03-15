using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Buckshot : Projectile
{
    [SerializeField] Projectile bulletPrefab;
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
                Debug.LogError($"AAAAAAAAAAAAAAAAAAAAAAAAAAAAA {Info.weapon.mask.value}");
                var randomDir = dir + Random.insideUnitSphere * Mathf.Tan(this.scatterAngle * Mathf.Deg2Rad);
                var proj = Instantiate(this.bulletPrefab, pos, Quaternion.LookRotation(randomDir));
                ProjectileController.I.RegisterAlienProjectile(proj, Info.kind, Info.weapon, pos, randomDir);
            }
        }
        Finish();
    }
}
