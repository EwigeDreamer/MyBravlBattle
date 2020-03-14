using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseImpactEffect : ImpactEffect
{
    float m_Impulse;
    public ImpulseImpactEffect(float impulse)
    {
        m_Impulse = impulse;
    }
    public override void Execute(GameObject go, ProjectileInfo proj, PointInfo info)
    { go.GetComponent<Rigidbody>()?.AddForceAtPosition(info.direction * m_Impulse, info.point, ForceMode.Impulse); }
}

