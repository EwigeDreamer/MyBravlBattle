using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    [SerializeField] float speed = 100f;

    protected override void RpcGo()
    {
    }

    protected override void RpcStop()
    {
    }
}

