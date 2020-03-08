using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyTools.Tween
{
    [RequireComponent(typeof(Graphic))]
    public class MyTweenUIColor : MyTween
    {
#pragma warning disable 649
        [SerializeField] Graphic m_Graphic;
        [SerializeField] Gradient m_Gradient;
#pragma warning restore 649

        protected override void OnValidate()
        {
            base.OnValidate();
            ValidateGetComponent(ref m_Graphic, true);
        }
        void Awake()
        {
            ValidateGetComponent(ref m_Graphic);
        }

        protected override void UpdateTween(float factor)
        {
            m_Graphic.color = m_Gradient.Evaluate(factor);
        }
    }
}
