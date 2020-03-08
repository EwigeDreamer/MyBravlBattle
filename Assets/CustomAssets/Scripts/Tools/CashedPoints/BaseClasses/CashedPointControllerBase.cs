using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MyTools.Helpers.CashedPointUnderHood
{
    public class CashedPointControllerBase<TInfo> : ImprovedBehaviour, IEnumerable<PointInfo<TInfo>>
    {
        [SerializeField]
        [HideInInspector]
        private int m_Count = 0;
        public int Count { get { return m_Count; } }

        [SerializeField]
        [HideInInspector]
        private Vector3[] m_CashedPositions = null;

        [SerializeField]
        [HideInInspector]
        private Quaternion[] m_CashedRotations = null;

        [SerializeField]
        [HideInInspector]
        private TInfo[] m_CashedInforms = null;


        protected void CashPoints()
        {
            if (m_Count > 0) return;
            List<CashedPointBase<TInfo>> points = new List<CashedPointBase<TInfo>>(TR.childCount);
            GetComponentsInChildren(true, points);

            int count = m_Count = points.Count;
            m_CashedPositions = new Vector3[count];
            m_CashedRotations = new Quaternion[count];
            m_CashedInforms = new TInfo[count];

            PointInfo<TInfo> pointInfo;

            for (int i = 0; i < count; ++i)
            {
                pointInfo = CollectInfo(points[i]);
                m_CashedPositions[i] = pointInfo.position;
                m_CashedRotations[i] = pointInfo.rotation;
                m_CashedInforms[i] = pointInfo.info;
            }
            for (int i = count - 1; i > -1; --i)
                if (points[i] != null)
                    DestroyImmediate(points[i].GO);
        }
        
        protected void UncashPoints()
        {
            if (m_Count < 1) return;
            int count = m_Count;
            PointInfo<TInfo> pointInfo;
            for (int i = 0; i < count; ++i)
            {
                GameObject obj = new GameObject(string.Format("Point ({0})", i));
                obj.transform.parent = TR;
                pointInfo.position = m_CashedPositions[i];
                pointInfo.rotation = m_CashedRotations[i];
                pointInfo.info = m_CashedInforms[i];
                RestoreInfo(obj, pointInfo);
                RegisterObject(obj);
            }
            m_CashedPositions = null;
            m_CashedRotations = null;
            m_CashedInforms = null;
            m_Count = 0;
        }

        protected virtual PointInfo<TInfo> CollectInfo(CashedPointBase<TInfo> cashedPoint) { return default; }
        protected virtual void RestoreInfo(GameObject obj, PointInfo<TInfo> pointInfo) { }
        protected virtual void RegisterObject(GameObject obj) { }

        public PointInfo<TInfo>[] GetPoints()
        {
            int count = m_Count;
            PointInfo<TInfo>[] points = new PointInfo<TInfo>[count];
            for (int i = 0; i < count; ++i)
            {
                points[i].position = m_CashedPositions[i];
                points[i].rotation = m_CashedRotations[i];
                points[i].info = m_CashedInforms[i];
            }
            return points;
        }

        public IEnumerator<PointInfo<TInfo>> GetEnumerator()
        {
            return new CashedPointsEnumerator<TInfo>(m_CashedPositions, m_CashedRotations, m_CashedInforms, m_Count);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CashedPointsEnumerator<TInfo>(m_CashedPositions, m_CashedRotations, m_CashedInforms, m_Count);
        }
    }

    public class CashedPointsEnumerator<TInfo> : IEnumerator<PointInfo<TInfo>>
    {
        int m_Count = 0;
        Vector3[] m_Poss;
        Quaternion[] m_Rots;
        TInfo[] m_Infs;
        int m_Index = -1;

        public CashedPointsEnumerator(Vector3[] poss, Quaternion[] rots, TInfo[] infs, int count)
        {
            if (poss == null || rots == null || infs == null) return;
            if (count != poss.Length || count != rots.Length || count != infs.Length) return;
            m_Poss = poss;
            m_Rots = rots;
            m_Infs = infs;
            m_Count = count;
        }

        public PointInfo<TInfo> Current
        {
            get
            {
                if (m_Index < 0 || m_Index > m_Count - 1)
                    throw new InvalidOperationException();
                PointInfo<TInfo> pointInfo;
                pointInfo.position = m_Poss[m_Index];
                pointInfo.rotation = m_Rots[m_Index];
                pointInfo.info = m_Infs[m_Index];
                return pointInfo;
            }
        }

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (++m_Index < m_Count)
                return true;
            return false;
        }

        public void Reset()
        {
            m_Index = -1;
        }

        public void Dispose() { }
    }
}
