using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MyTools.Extensions.GameObjects;
using DG.Tweening;
using MyTools.Extensions.Vectors;

public class NetworkPlayerStatusBar : NetworkBehaviour
{
    [SerializeField] PlayerStatusBar bar;
    [SerializeField] NetworkPlayerHealth health;
    [SerializeField] NetworkPlayerView view;

    [SerializeField] Color myColor = Color.green;
    [SerializeField] Color enemyColor = Color.red;

    private void OnValidate()
    {
        gameObject.ValidateGetComponent(ref this.health);
        gameObject.ValidateGetComponent(ref this.view);
        gameObject.ValidateGetComponentInChildren(ref this.bar);
    }

    private void Awake()
    {
        this.view.OnChangeVisible += state => this.bar.SetActive(state);
        this.health.OnDamage += _ => RefreshHp();
        this.health.OnHeal += _ => RefreshHp();
        this.health.OnReset += () => RefreshHp();
    }

    private void Start()
    {
        this.bar.SetHpColor(isLocalPlayer ? myColor : enemyColor);
        RefreshHp();
    }

    void RefreshHp()
    {
        var hp = this.health.Hp;
        this.bar.SetHpValue(hp.Normalize);
    }

    public void Refresh()
    {
        RefreshHp();
        this.bar.SetActive(this.view.IsVisible);
    }
}
