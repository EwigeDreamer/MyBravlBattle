using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyTools.Extensions.Vectors;

public class PlayerStatusBar : MonoBehaviour
{
    [SerializeField] RectTransform hpBarRtr;
    [SerializeField] Image hpBarImg;
    public void SetActive(bool state) => gameObject.SetActive(state);
    public void SetHpValue(float value) => this.hpBarRtr.localScale = this.hpBarRtr.localScale.SetX(value);
    public void SetHpColor(Color col) => this.hpBarImg.color = col;
}
