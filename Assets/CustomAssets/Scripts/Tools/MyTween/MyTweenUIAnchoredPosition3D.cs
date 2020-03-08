using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTools.Tween
{
    [RequireComponent(typeof(RectTransform))]
    public class MyTweenUIAnchoredPosition3D : MyTween
    {
#pragma warning disable 649
        [SerializeField] RectTransform m_Rt;
        [SerializeField] Vector3 m_From;
        [SerializeField] Vector3 m_To;
        [SerializeField] AnimationCurve m_Curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
#pragma warning restore 649

        protected override void OnValidate()
        {
            base.OnValidate();
            ValidateGetComponent(ref m_Rt, true);
        }
        void Awake()
        {
            ValidateGetComponent(ref m_Rt);
        }

        protected override void UpdateTween(float factor)
        {
            m_Rt.anchoredPosition3D = Vector3.LerpUnclamped(m_From, m_To, m_Curve.Evaluate(factor));
        }

        [ContextMenu("Set From")]
        void SetFrom() { m_From = ((RectTransform)transform).anchoredPosition3D; }
        [ContextMenu("Set To")]
        void SetTo() { m_To = ((RectTransform)transform).anchoredPosition3D; }
        [ContextMenu("Restore From")]
        void RestoreFrom() { ((RectTransform)transform).anchoredPosition3D = m_From; }
        [ContextMenu("Restore To")]
        void RestoreTo() { ((RectTransform)transform).anchoredPosition3D = m_To; }
    }
}
