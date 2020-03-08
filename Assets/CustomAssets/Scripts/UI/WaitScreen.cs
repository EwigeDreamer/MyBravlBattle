using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Menu;
using MyTools.Tween;
using MyTools.Extensions.Common;
using UnityEngine.UI;
using System;
using MyTools.Helpers;
using DG.Tweening;
using MyTools.Singleton;
using MyTools.Extensions.Colors;


namespace SpaceTramp
{
    public class WaitScreen : MonoSingleton<WaitScreen>
    {
        [SerializeField] Canvas canvas;
        [SerializeField] float duration = 0.5f;

        [Header("Background")]
        [SerializeField] Image background;
        [SerializeField] Color backgroundColorOff;
        [SerializeField] Color backgroundColorOn;

        [Header("Spinner")]
        [SerializeField] RectTransform spinner;
        [SerializeField] RectTransform spinnerContainer;
        [SerializeField] RectTransform spinnerPointOff;
        [SerializeField] RectTransform spinnerPointOn;
        [SerializeField] float spinnerSpeed = 1f;

        protected override void OnValidate()
        {
            base.OnValidate();
            ValidateGetComponent(ref canvas);
        }

        protected override void Awake()
        {
            base.Awake();
            if (ValidateGetComponent(ref this.canvas)) this.canvas.enabled = false;
        }

        public Coroutine Show(bool forced = false)
        {
            return CorouWaiter.Start(GetRoutine());
            IEnumerator GetRoutine()
            {
                this.canvas.enabled = true;
                this.spinner.localRotation = Quaternion.identity;
                this.spinner.DOLocalRotate(new Vector3(0f, 0f, -360f), 1f / this.spinnerSpeed, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
                if (forced)
                {
                    this.background.color = this.backgroundColorOn;
                    this.spinnerContainer.anchoredPosition = this.spinnerPointOn.anchoredPosition;
                }
                else
                {
                    this.background.color = this.backgroundColorOff;
                    this.spinnerContainer.anchoredPosition = this.spinnerPointOff.anchoredPosition;
                    var sequence = DOTween.Sequence()
                        .Append(this.background.DOColor(this.backgroundColorOn, this.duration).SetEase(Ease.InOutSine))
                        .Join(this.spinnerContainer.DOAnchorPos(this.spinnerPointOn.anchoredPosition, this.duration).SetEase(Ease.OutSine));
                    yield return sequence.WaitForCompletion(true);
                }
                yield break;
            }
        }

        public Coroutine Hide(bool forced = false)
        {
            return CorouWaiter.Start(GetRoutine());
            IEnumerator GetRoutine()
            {
                if (!forced)
                {
                    this.background.color = this.backgroundColorOn;
                    this.spinnerContainer.anchoredPosition = this.spinnerPointOn.anchoredPosition;
                    var sequence = DOTween.Sequence()
                        .Append(this.background.DOColor(this.backgroundColorOff, this.duration).SetEase(Ease.InOutSine))
                        .Join(this.spinnerContainer.DOAnchorPos(this.spinnerPointOff.anchoredPosition, this.duration).SetEase(Ease.InSine));
                    yield return sequence.WaitForCompletion(true);
                }
                this.spinner.DOKill();
                this.canvas.enabled = false;
                yield break;
            }
        }
    }
}