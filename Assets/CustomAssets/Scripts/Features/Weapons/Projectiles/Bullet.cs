using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;

public class Bullet : Projectile
{
    [SerializeField] float speed = 100f;
    [SerializeField] float maxDist = 100f;
    bool isActive = false;
    Vector3 lastPos = default;
    Vector3 firstPos = default;

    protected override void RpcGo()
    {
        this.lastPos = this.firstPos = transform.position;
        this.isActive = true;
    }

    protected override void RpcStop()
    {
        isActive = false;
    }


    private void Update()
    {
        if (!isActive) return;
        var mask = Info.weapon.mask;
        var dt = TimeManager.DeltaTime;
        var lastPos = this.lastPos;
        var pos = transform.position;
        var forward = transform.forward;
        var nextPos = pos + forward * this.speed * dt;
        var overlapDist = (nextPos - lastPos).magnitude;
        if (Physics.Raycast(lastPos, forward, out var hit, overlapDist, mask))
        {
            Hit(hit.collider, new PointInfo { point = hit.point, direction = forward, normal = hit.normal });
            transform.position = hit.point;
            this.isActive = false;
            Finish();
            return;
        }
        this.lastPos = pos;
        transform.position = nextPos;
        if ((nextPos - this.firstPos).sqrMagnitude > this.maxDist * this.maxDist)
        {
            this.isActive = false;
            Finish();
            return;
        }
    }
}

