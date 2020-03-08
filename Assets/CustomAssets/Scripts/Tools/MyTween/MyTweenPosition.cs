using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTools.Tween
{
    public class MyTweenPosition : MyTween
    {
#pragma warning disable 649
        [SerializeField] bool m_Local = true;
        [SerializeField] Vector3 m_From;
        [SerializeField] Vector3 m_To;
        [SerializeField] AnimationCurve m_Curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
#pragma warning restore 649

        protected override void UpdateTween(float factor)
        {
            if (m_Local)
                TR.localPosition = Vector3.LerpUnclamped(m_From, m_To, m_Curve.Evaluate(factor));
            else
                TR.position = Vector3.LerpUnclamped(m_From, m_To, m_Curve.Evaluate(factor));
        }

        [ContextMenu("Set From")]
        void SetFrom() { m_From = m_Local ? transform.localPosition : transform.position; }
        [ContextMenu("Set To")]
        void SetTo() { m_To = m_Local ? transform.localPosition : transform.position; }
        [ContextMenu("Restore From")]
        void RestoreFrom() { if (m_Local) transform.localPosition = m_From; else transform.position = m_From; }
        [ContextMenu("Restore To")]
        void RestoreTo() { if (m_Local) transform.localPosition = m_To; else transform.position = m_To; }
    }
}