using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameMenuPopup : PopupBase
{
    [SerializeField] GameObject serverWindow;
    [SerializeField] GameObject clientWindow;
    [SerializeField] TMP_Text ipLabel;
    [SerializeField] Button closeRoomBtn;
    [SerializeField] Button disconnectBtn;
    [SerializeField] Button copyIpAddressBtn;
    [SerializeField] Button[] returnBtns;
    [SerializeField] RectTransform popoverTr;
    [SerializeField] CanvasGroup popoverCg;

    TextEditor textEditor = null;
    Sequence sequence = null;

    protected override void OnInit()
    {
        base.OnInit();
        var isServer = CustomNetworkManager.I.IsServer;
        serverWindow.SetActive(isServer);
        clientWindow.SetActive(!isServer);
        ipLabel.text = CustomNetworkManager.I.IpAddress;
        closeRoomBtn.onClick.AddListener(() => { CustomNetworkManager.I.StopHost(); Hide(null); });
        disconnectBtn.onClick.AddListener(() => { CustomNetworkManager.I.StopClient(); Hide(null); });
        copyIpAddressBtn.onClick.AddListener(() => { CopyToClipboard(); AnimatePopover(); });
        foreach (var btn in returnBtns) btn.onClick.AddListener(() => Hide(null));
        popoverTr.gameObject.SetActive(false);
    }

    protected override void OnRemove()
    {
        base.OnRemove();
        textEditor = null;
    }

    void CopyToClipboard()
    {
        textEditor = new TextEditor();
        textEditor.text = CustomNetworkManager.I.IpAddress;
        textEditor.SelectAll();
        textEditor.Copy();
    }

    void AnimatePopover()
    {
        if (sequence != null) return;
        popoverTr.gameObject.SetActive(true);
        popoverTr.anchoredPosition = new Vector2(0f, 150f);
        popoverCg.alpha = 0f;
        sequence = DOTween.Sequence()
            .Append(popoverTr.DOAnchorPos(new Vector2(0f, 200f), 0.5f).SetEase(Ease.OutSine))
            .Join(popoverCg.DOFade(1f, 0.25f).SetEase(Ease.InOutSine))
            .AppendInterval(1f)
            .Append(popoverCg.DOFade(0f, 0.25f).SetEase(Ease.InOutSine))
            .AppendCallback(() =>
            {
                popoverTr.gameObject.SetActive(false);
                sequence = null;
            });
    }
}
