using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTools.Tween
{
    public class MyTweenPositionCurveAxis : MyTween
    {
#pragma warning disable 649
        [SerializeField] bool m_Local = true;
        [SerializeField] AnimationCurve m_X = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f);
        [SerializeField] AnimationCurve m_Y = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f);
        [SerializeField] AnimationCurve m_Z = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f);
#pragma warning restore 649

        protected override void UpdateTween(float factor)
        {
            var pos = new Vector3
            {
                x = m_X.Evaluate(factor),
                y = m_Y.Evaluate(factor),
                z = m_Z.Evaluate(factor)
            };

            if (m_Local)
                TR.localPosition = pos;
            else
                TR.position = pos;
        }
    }
}