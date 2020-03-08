using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools;
using System;
using MyTools.Extensions.Transforms;
using MyTools.Pooling;
using MyTools.Helpers;

namespace MyTools.Tracer
{
    public class TracerUnit : ImprovedBehaviour, IPooledComponent
    {
#pragma warning disable 649
        [Header("HitDecal")]
        [SerializeField] Transform m_DecalTr;

        [Header("DrawLine")]
        [SerializeField] LineRenderer m_Line;
        [SerializeField] int m_DrawSteps = 9;
        [SerializeField] float m_DrawTimeStep = 0.25f;

        [Header("RayCast")]
        [SerializeField] LayerMask m_LayerMask;
        [SerializeField] int m_CastSteps = 3;

        [Header("StartInfo")]
        [SerializeField] Transform m_StartPoint = null;
        [SerializeField] float m_StartVelocity = 10f;

        [Header("Renderers")]
        [SerializeField] Renderer[] m_Renderers;

        [ContextMenu("Get Renderers")]
        void GetRenderers() { m_Renderers = GetComponentsInChildren<Renderer>(); }

        int m_ColHash;

        public event Action<TracerUnit> OnProcess = delegate { };
        RaycastHit m_HitInfo = default;
        public RaycastHit HitInfo { get { return m_HitInfo; } }

        Vector3 m_GravStep;
        List<Vector3> m_Points = new List<Vector3>(100);
        List<Vector3> m_Buffer = new List<Vector3>(10);

        bool m_IsHitting = false;
        public bool IsHitting { get { return m_IsHitting; } }
#pragma warning restore 649

        public void SetColor(Color col)
        {
            var hash = m_ColHash;
            var rs = m_Renderers;
            var count = rs.Length;
            for (int i = 0; i < count; ++i)
                rs[i].material.SetColor(hash, col);
        }




        Action m_Deactive;
        event Action IPooledComponent.Deactive
        {
            add { m_Deactive += value; }
            remove { m_Deactive -= value; }
        }
        void IPooledComponent.OnActivation() { }
        void IPooledComponent.OnDeactivation() { OnProcess = delegate { }; }

        void Awake()
        {
            m_GravStep = Physics.gravity * m_DrawTimeStep;
            if (m_DrawSteps < m_CastSteps) m_DrawSteps = m_CastSteps;
            m_ColHash = Shader.PropertyToID("_Color");
        }


        public TracerUnit StartTracing(Transform startPoint, float startVelocity, int layerMask)
        {
            m_StartPoint = startPoint;
            m_StartVelocity = startVelocity;
            m_LayerMask = layerMask;
            enabled = true;
            return this;
        }
        public void StopTracing()
        {
            if (m_Deactive != null) m_Deactive(); 
            else Destroy(GO);
        }

        void Update()
        {
            var startPoint = m_StartPoint;
            if (startPoint == null) { StopTracing(); return; }
            var decal = m_DecalTr;

            var points = m_Points;
            var buffer = m_Buffer;
            points.Clear();
            buffer.Clear();

            var vel = startPoint.forward * m_StartVelocity;
            var drawSteps = m_DrawSteps;
            var castSteps = m_CastSteps;
            var gravStep = m_GravStep;
            var drawTimeStep = m_DrawTimeStep;
            int mask = m_LayerMask;
            Vector3 velStep;

            var cast1 = startPoint.position;
            var cast2 = cast1;

            points.Add(cast1);

            bool isHitting = false;

            for (int i = 1; i < drawSteps; ++i)
            {
                vel += gravStep;
                velStep = vel * drawTimeStep;
                cast2 += velStep;
                buffer.Add(cast2);

                if (i % castSteps == 0 || i == drawSteps - 1)
                {
                    if (Physics.Linecast(cast1, cast2, out var hit, mask))
                    {
                        cast2 = hit.point;
                        points.Add(cast2);
                        decal?.SetPositionAndRotation(hit.point + hit.normal * 0.1f, Quaternion.LookRotation(-hit.normal));
                        isHitting = true;
                        m_HitInfo = hit;
                        m_IsHitting = true;
                        i = drawSteps;
                    }
                    else
                    {
                        cast1 = cast2;
                        points.AddRange(buffer);
                        buffer.Clear();
                        m_HitInfo = default;
                        m_IsHitting = false;
                    }
                }
            }

            var line = m_Line;
            var count = points.Count;
            line.positionCount = count;
            for (int i = 0; i < count; ++i)
                line.SetPosition(i, points[i]);

            decal?.SetActive(isHitting);

            OnProcess(this);
        }
    }
}