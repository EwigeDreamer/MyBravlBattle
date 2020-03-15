using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;

public class KillCounter : MonoValidate
{
    [SerializeField] GameUI gameUI;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref this.gameUI);
    }

    int killCount = 0;
    void Start()
    {
        MatchController.I.OnKill -= IncrementKill;
        MatchController.I.OnKill += IncrementKill;
        this.gameUI.SetKillCount(this.killCount);
    }

    private void OnDestroy()
    {
        MatchController.I.OnKill -= IncrementKill;
    }

    void IncrementKill()
    {
        ++this.killCount;
        this.gameUI.SetKillCount(this.killCount);
    }
}
