using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class JoinPopup : PopupBase
{
    [SerializeField] TMP_InputField field;
    [SerializeField] Button joinBtn;
    [SerializeField] Button cancelBtn;
    [SerializeField] Button[] returnBtns;

    protected override void OnInit()
    {
        base.OnInit();
        field.text = CustomNetworkManager.I.networkAddress;
        field.onValueChanged.AddListener(ip => CustomNetworkManager.I.networkAddress = ip);
        joinBtn.onClick.AddListener(() => { CustomNetworkManager.I.StartClient(); Hide(null); });
        cancelBtn.onClick.AddListener(() => Hide(null));
        foreach (var btn in returnBtns) btn.onClick.AddListener(() => Hide(null));
    }
}
