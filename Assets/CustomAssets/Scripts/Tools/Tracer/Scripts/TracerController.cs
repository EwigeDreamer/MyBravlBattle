using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools;
using System;
using MyTools.Extensions.Components;
using MyTools.Factory;
using MyTools.Singleton;

namespace MyTools.Tracer
{
    public class TracerController : MonoSingleton<TracerController>
    {
        IFactory<TracerUnit> m_Factory;
        protected override void Awake()
        {
            base.Awake();
            m_Factory = GetComponent<IFactory<TracerUnit>>();
        }

        public TracerUnit StartTracing(Transform point, float startSpeed, int layerMask)
        {
            if (m_Factory == null) return null;
            var tracer = m_Factory.GetObject();
            tracer.StartTracing(point, startSpeed, layerMask);
            return tracer;
        }
    }
}
