using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MyTools.Extensions.GameObjects;

public class NetworkPlayerView : NetworkBehaviour
{
    [SerializeField] Renderer[] renderers;
    [SerializeField] Animator animator;

    private void OnValidate()
    {
        gameObject.ValidateGetComponentInChildren(ref this.animator);
    }

    [ContextMenu("Get renderers")]
    void GetRenderers() => this.renderers = gameObject.GetComponentsInChildren<Renderer>();

    [Command]
    public void CmdSetVisible(bool state) => RpcSetVisible(state);

    [ClientRpc]
    void RpcSetVisible(bool state)
    {
        foreach (var r in this.renderers) r.enabled = state;
    }
}
