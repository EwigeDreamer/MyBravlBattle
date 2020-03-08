using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Factory;

namespace MyTools.Tracer.Helpers
{
    public class TracerUnitFactory : MonoBehaviour, IFactory<TracerUnit>
    {
#pragma warning disable 649
        [SerializeField] TracerUnit m_UnitPrefab;
#pragma warning restore 649

        public TracerUnit GetObject()
        {
            var prefab = m_UnitPrefab;
            if (prefab == null) return null;
            return Instantiate(prefab);
        }

        public bool TryGetObject(out TracerUnit obj)
        {
            obj = GetObject();
            return obj != null;
        }
    }
}
