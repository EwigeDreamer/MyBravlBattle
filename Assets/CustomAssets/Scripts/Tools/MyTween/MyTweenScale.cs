using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using MyTools.Tween.Base;

namespace MyTools.Tween
{
    public class MyTweenScale : MyTween
    {
#pragma warning disable 649
        [SerializeField] Vector3 m_From;
        [SerializeField] Vector3 m_To;
        [SerializeField] AnimationCurve m_Curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
#pragma warning restore 649

        protected override void UpdateTween(float factor)
        {
            TR.localScale = Vector3.LerpUnclamped(m_From, m_To, m_Curve.Evaluate(factor));
        }

        [ContextMenu("Set From")]
        void SetFrom() { m_From = transform.localScale; }
        [ContextMenu("Set To")]
        void SetTo() { m_To = transform.localScale; }
        [ContextMenu("Restore From")]
        void RestoreFrom() { transform.localScale = m_From; }
        [ContextMenu("Restore To")]
        void RestoreTo() { transform.localScale = m_To; }
    }
}
