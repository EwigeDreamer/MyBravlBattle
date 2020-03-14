using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DischargeImpactEffect : ImpactEffect
{
    float m_Discharge;
    public DischargeImpactEffect(float discharge)
    {
        m_Discharge = discharge;
    }
    public override void Execute(GameObject go, ProjectileInfo proj, PointInfo info)
    {
        //go.GetComponent<CharEnergy>()?.SetDischarge(m_Discharge);
    }
}

