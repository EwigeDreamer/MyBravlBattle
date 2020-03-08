using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using MyTools.Tween.Base;

namespace MyTools.Tween
{
    public class MyTweenScaleCurveAxis : MyTween
    {
#pragma warning disable 649
        [SerializeField] AnimationCurve m_X = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f);
        [SerializeField] AnimationCurve m_Y = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f);
        [SerializeField] AnimationCurve m_Z = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f);
#pragma warning restore 649

        protected override void UpdateTween(float factor)
        {
            var scale = new Vector3
            {
                x = m_X.Evaluate(factor),
                y = m_Y.Evaluate(factor),
                z = m_Z.Evaluate(factor)
            };
            TR.localScale = scale;
        }
    }
}
