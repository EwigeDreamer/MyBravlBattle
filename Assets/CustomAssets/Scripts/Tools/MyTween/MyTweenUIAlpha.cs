using UnityEngine;
using UnityEngine.Events;

namespace MyTools.Tween
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MyTweenUIAlpha : MyTween
    {
#pragma warning disable 649
        [SerializeField] CanvasGroup m_CanvGroub;
        [SerializeField] float m_From;
        [SerializeField] float m_To;
        [SerializeField] AnimationCurve m_Curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
#pragma warning restore 649

        protected override void OnValidate()
        {
            base.OnValidate();
            ValidateGetComponent(ref m_CanvGroub, true);
        }
        void Awake()
        {
            ValidateGetComponent(ref m_CanvGroub);
        }

        protected override void UpdateTween(float factor)
        {
            m_CanvGroub.alpha = Mathf.Lerp(m_From, m_To, m_Curve.Evaluate(factor));
        }
    }
}
