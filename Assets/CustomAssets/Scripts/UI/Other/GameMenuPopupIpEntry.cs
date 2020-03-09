using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMenuPopupIpEntry : MonoBehaviour
{
    [SerializeField] TMP_Text ipLabel;
    [SerializeField] Button copyBtn;

    public TMP_Text IpLabel => ipLabel;
    public Button CopyBtn => copyBtn;
    public Transform Parent => transform.parent;
    public void SetActive(bool state) => gameObject.SetActive(state);
}
