﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer Current { get; private set; } = null;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Debug.Log($"Init local player [{name}]");
        Current = this;
    }

    public override void OnNetworkDestroy()
    {
        base.OnNetworkDestroy();
        Debug.Log($"Destroy local player [{name}]");
        if (Current == this) Current = null;
    }

    [Command]
    public void CmdDoInServer()
    {
        Debug.LogError($"SERVER COMMAND! [{name}]");
        RpcDoInAllClients();
        RpcDoInTargetClients();
    }

    [ClientRpc]
    void RpcDoInAllClients()
    {
        Debug.LogError($"CLIENT COMMAND! [{name}]");
    }

    [ClientRpc]
    void RpcDoInTargetClients()
    {
        Debug.LogError($"TARGET CLIENT COMMAND! [{name}]");
    }
}
