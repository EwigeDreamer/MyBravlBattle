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


    [SerializeField] [SyncVar] IntInfo hp = new IntInfo { Min = 0, Max = 100, Value = 100 };

    public IntInfo Hp => hp;

    [Command] public void CmdSetDamage(int damage)
    {
        if (hp.IsMin) return;
        damage = Math.Min(hp.Value, damage);
        hp -= damage;
        RpcSetDamage(damage, hp.IsZero);
    }
    [ClientRpc] void RpcSetDamage(int damage, bool dead)
    {
        OnDamage(damage);
        if (dead) OnDead();
    }

    [Command] public void CmdSetHeal(int heal)
    {
        if (hp.IsMax) return;
        heal = Math.Min(hp.Max - hp.Value, heal);
        hp += heal;
        RpcSetHeal(heal);
    }
    [ClientRpc] public void RpcSetHeal(int heal)
    {
        OnHeal(heal);
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
