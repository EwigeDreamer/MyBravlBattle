using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MyTools.ValueInfo;
using System;

public class NetworkPlayerHealth : NetworkBehaviour
{
    public event Action OnDead = delegate { };
    public event Action<int> OnDamage = delegate { };
    public event Action<int> OnHeal = delegate { };
    public event Action OnReset = delegate { };


    [SerializeField] IntInfo hp = new IntInfo { Min = 0, Max = 100, Value = 100 };

    public IntInfo Hp => hp;


    [Command] public void CmdSetDamage(int damage)
    {
        if (this.hp.IsMin) return;
        var newHp = this.hp;
        newHp.Value -= damage;
        RpcSetNewHpValue(newHp);
    }

    [Command] public void CmdSetHeal(int heal)
    {
        if (this.hp.IsMax) return;
        var newHp = this.hp;
        newHp.Value += heal;
        RpcSetNewHpValue(newHp);
    }

    [ClientRpc]
    void RpcSetNewHpValue(IntInfo hp)
    {
        int diff = hp.value - this.hp.value;
        if (diff == 0) return;
        this.hp = hp;
        if (diff > 0) OnHeal(diff);
        if (diff < 0) OnDamage(-diff);
        if (hp.IsZero) OnDead();
    }

    [Command] public void CmdResetHealth() => RpcResetHealth();
    [ClientRpc] public void RpcResetHealth()
    {
        if (hp.IsMax) return;
        hp.Value = hp.Max;
        OnReset();
    }

    public void Refresh()
    {

    }
}
