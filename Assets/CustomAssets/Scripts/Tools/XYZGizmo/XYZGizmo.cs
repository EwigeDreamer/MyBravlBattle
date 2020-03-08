using UnityEngine;

namespace MyTools
{
    public static class XYZGizmo
    {
        static GameObject m_GizmoRef;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            m_GizmoRef = (GameObject)Resources.Load("XYZGizmo/3DarrowsGizmo");
        }
        static Transform GetGizmoInst()
        {
            return m_GizmoRef != null ? Object.Instantiate(m_GizmoRef).transform : null;
        }

        public static Transform DrawXYZGizmo(Transform parent, float scale = 1f, float lifetime = -1)
        {
            Transform newGizmoTR = null;
#if UNITY_EDITOR
            newGizmoTR = GetGizmoInst();
            if (newGizmoTR)
            {
                newGizmoTR.localScale = new Vector3(scale, scale, scale);
                newGizmoTR.parent = parent;
                newGizmoTR.localPosition = Vector3.zero;
                newGizmoTR.localRotation = Quaternion.identity;
            }
#endif
            if (lifetime > 0) Object.Destroy(newGizmoTR.gameObject, lifetime);
            return newGizmoTR;
        }

        public static Transform DrawXYZGizmo(Vector3 position, Quaternion rotation, float scale = 1f, float lifetime = -1)
        {
            Transform newGizmoTR = null;
#if UNITY_EDITOR
            newGizmoTR = GetGizmoInst();
            if (newGizmoTR)
            {
                newGizmoTR.localScale = new Vector3(scale, scale, scale);
                newGizmoTR.localPosition = position;
                newGizmoTR.localRotation = rotation;
            }
#endif
            if (lifetime > 0) Object.Destroy(newGizmoTR.gameObject, lifetime);
            return newGizmoTR;
        }
    }
}