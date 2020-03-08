using UnityEngine;
using UnityEngine.Events;
//using MyTools.Tween.Base;

namespace MyTools.Tween
{
    public class MyTweenRotation : MyTween
    {
#pragma warning disable 649
        [SerializeField] bool m_Local = true;
        [SerializeField] Quaternion m_From;
        [SerializeField] Quaternion m_To;
        [SerializeField] AnimationCurve m_Curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
#pragma warning restore 649

        protected override void UpdateTween(float factor)
        {
            if (m_Local)
                TR.localRotation = Quaternion.SlerpUnclamped(m_From, m_To, m_Curve.Evaluate(factor));
            else
                TR.rotation = Quaternion.SlerpUnclamped(m_From, m_To, m_Curve.Evaluate(factor));
        }

        [ContextMenu("Set From")]
        void SetFrom() { m_From = m_Local ? transform.localRotation : transform.rotation; }
        [ContextMenu("Set To")]
        void SetTo() { m_To = m_Local ? transform.localRotation : transform.rotation; }
        [ContextMenu("Restore From")]
        void RestoreFrom() { if (m_Local) transform.localRotation = m_From; else transform.rotation = m_From; }
        [ContextMenu("Restore To")]
        void RestoreTo() { if (m_Local) transform.localRotation = m_To; else transform.rotation = m_To; }
    }
}