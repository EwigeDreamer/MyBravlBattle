using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyTools.Helpers;
using MyTools;

namespace MyTools.Tween
{
    using Internal;
    public enum TweenStyle
    {
        Once,
        Loop,
        PingPong
    }
    public enum UpdateType
    {
        Update,
        LateUpdate,
        FixedUpdate
    }
    public enum SideType
    {
        From,
        To
    }
    public struct TweenInfo
    {
        public float duration;
        public TweenStyle style;
        public UpdateType updateType;
        public bool unscaledDeltaTime;
    }

    public abstract class MyTween : MonoValidate
    {
        public static SideType StartSideByDirection(bool direction)
        { return direction ? SideType.From : SideType.To; }

        [SerializeField] float m_Duration = 1f;
        [SerializeField] TweenStyle m_Style = TweenStyle.Once;
        [SerializeField] UpdateType m_UpdateType = UpdateType.Update;
        [SerializeField] bool m_UnscaledDeltaTime = false;
        bool m_Direction = true;
        public bool Direction => m_Direction;
        float m_Factor = 0f;

        public void SetDuration(float duration)
        { m_Duration = Mathf.Abs(duration); }

        public event Action<SideType> OnFinish = delegate { };
        public event Action OnLoop = delegate { };
        public event Action<SideType> OnPinPong = delegate { };
        public event Action<SideType> OnPlay = delegate { };
        public event Action OnStop = delegate { };
        public event Action<SideType> OnReset = delegate { };
        Action m_OnFinishCallback = delegate { };

        void SetCallback(Action callback)
        {
            DoCallBack();
            m_OnFinishCallback += callback;
        }
        void DoCallBack()
        {
            m_OnFinishCallback();
            m_OnFinishCallback = delegate { };
        }

        public void SetTweenInfo(TweenInfo info)
        {
            m_Duration = info.duration;
            m_Style = info.style;
            m_UpdateType = info.updateType;
            m_UnscaledDeltaTime = info.unscaledDeltaTime;
        }
        public void PlayTween(SideType start = SideType.From, Action onFinish = null)
        {
            var dir = start == SideType.From;
            m_Direction = dir;
            m_Factor = dir ? 0f : -1f;
            enabled = true;
            OnPlay(start);
            SetCallback(onFinish);
        }
        public void PlayTween(float position, SideType start = SideType.From, Action onFinish = null)
        {
            var dir = start == SideType.From;
            m_Direction = dir;
            m_Factor = Mathf.Clamp01(position) * (dir ? 1f : -1f);
            enabled = true;
            OnPlay(start);
            SetCallback(onFinish);
        }
        public void PauseTween() { enabled = false; }
        public void ResumeTween() { enabled = true; }
        public void StopTween()
        {
            var dir = m_Direction;
            var factor = dir ? 1f : 0f;
            m_Factor = factor;
            UpdateTween(factor * (dir ? 1f : -1f));
            enabled = false;
            OnStop();
            DoCallBack();
        }
        public void ResetTween(SideType side = SideType.From)
        {
            var dir = side == SideType.From;
            m_Direction = dir;
            m_Factor = dir ? 0f : -1f;
            enabled = false;
            UpdateTween(m_Factor * (dir ? 1f : -1f));
            OnReset(side);
            DoCallBack();
        }
        public void ResetTween(float position, SideType side = SideType.From)
        {
            var dir = side == SideType.From;
            m_Direction = dir;
            m_Factor = Mathf.Clamp01(position) * (dir ? 1f : -1f);
            enabled = false;
            UpdateTween(m_Factor * (dir ? 1f : -1f));
            OnReset(side);
            DoCallBack();
        }

        private void OnEnable()
        { Subscribe(); }
        private void OnDisable()
        { Unsubscribe(); }

        void Subscribe()
        {
            Unsubscribe();
            switch (m_UpdateType)
            {
                case UpdateType.Update: MyTweenController.OnUpdate += ManualUpdate; break;
                case UpdateType.LateUpdate: MyTweenController.OnLateUpdate += ManualUpdate; break;
                case UpdateType.FixedUpdate: MyTweenController.OnFixedUpdate += ManualUpdate; break;
                default: MyTweenController.OnUpdate += ManualUpdate; break;
            }
        }
        void Unsubscribe()
        {
            MyTweenController.OnUpdate -= ManualUpdate;
            MyTweenController.OnLateUpdate -= ManualUpdate;
            MyTweenController.OnFixedUpdate -= ManualUpdate;
        }

        void ManualUpdate(float deltaTime, float unscaledDeltaTime)
        { DoTween(m_UnscaledDeltaTime ? unscaledDeltaTime : deltaTime); }

        void DoTween(float deltaTime)
        {
            bool dir = m_Direction;
            float factor = m_Factor;
            float roof = dir ? 1f : 0f;
            float floor = dir ? 0f : -1f;
            factor += deltaTime / m_Duration;
            if (factor > roof)
            {
                switch (m_Style)
                {
                    case TweenStyle.Once:
                        factor = roof;
                        UpdateTween(factor);
                        OnFinish(dir ? SideType.To : SideType.From);
                        DoCallBack();
                        StopTween();
                        break;
                    case TweenStyle.Loop:
                        factor = floor;
                        UpdateTween(factor * (dir ? 1f : -1f));
                        OnLoop();
                        break;
                    case TweenStyle.PingPong:
                        factor = -roof;
                        dir = !dir;
                        m_Direction = dir;
                        UpdateTween(factor * (dir ? 1f : -1f));
                        OnPinPong(dir ? SideType.From : SideType.To);
                        break;
                }
            }
            else
                UpdateTween(factor * (dir ? 1f : -1f));
            m_Factor = factor;
        }

        protected virtual void UpdateTween(float factor) { }
    }
}
