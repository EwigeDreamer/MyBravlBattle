using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuittingPopup : PopupBase
{
    [SerializeField] Button confirmBtn;
    [SerializeField] Button declineBtn;
    [SerializeField] Button[] returnBtns;

    protected override int SortDelta => 0;

    protected override void OnInit()
    {
        base.OnInit();
        this.confirmBtn.onClick.AddListener(() => Application.Quit());
        this.declineBtn.onClick.AddListener(() => Hide(null));
        foreach (var btn in returnBtns) btn.onClick.AddListener(() => Hide(null));
    }
}
