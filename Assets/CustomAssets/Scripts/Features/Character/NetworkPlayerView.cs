using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MyTools.Extensions.GameObjects;
using DG.Tweening;
using MyTools.Extensions.Vectors;

public class NetworkPlayerView : NetworkBehaviour
{
    [SerializeField] Renderer[] renderers;
    [SerializeField] Animator animator;
    [SerializeField] NetworkPlayerMotor motor;

    Tween torsoTween = null;
    bool isVisible = true;

    int forwardHash = Animator.StringToHash("forward");
    int rightHash = Animator.StringToHash("right");
    int torsoLayerIndex;

    public bool IsVisible => IsVisible;

    private void OnValidate()
    {
        gameObject.ValidateGetComponentInChildren(ref this.animator);
        gameObject.ValidateGetComponent(ref this.motor);
    }

    private void Awake()
    {
        torsoLayerIndex = animator.GetLayerIndex("Torso");
    }

    [ContextMenu("Get renderers")]
    void GetRenderers() => this.renderers = gameObject.GetComponentsInChildren<Renderer>();

    [Command]
    public void CmdSetVisible(bool state) => RpcSetVisible(state);

    [ClientRpc]
    void RpcSetVisible(bool state)
    {
        foreach (var r in this.renderers) r.enabled = state;
        this.isVisible = state;
    }

    [Command]
    public void CmdSetAim(bool state, bool forced) => RpcSetAim(state, forced);
    [ClientRpc]
    void RpcSetAim(bool state, bool forced)
    {
        torsoTween?.Kill();
        if (forced)
            animator.SetLayerWeight(torsoLayerIndex, state ? 1f : 0f);
        else
        {
            torsoTween = DOVirtual.Float(
                animator.GetLayerWeight(torsoLayerIndex),
                state ? 1f : 0f,
                0.25f,
                value => animator.SetLayerWeight(torsoLayerIndex, value));
        }
    }

    private void FixedUpdate()
    {
        SetGlobalMove(motor.NormalizedVelocity.ToV2_xz());
    }

    void SetGlobalMove(Vector2 globalDir)
    {
        var localDir = transform.InverseTransformDirection(globalDir.ToV3_x0y());
        SetLocalMove(localDir.ToV2_xz());
    }
    void SetLocalMove(Vector2 localDir)
    {
        animator.SetFloat(rightHash, localDir.x);
        animator.SetFloat(forwardHash, localDir.y);
    }

    public void Refresh()
    {

    }
}
