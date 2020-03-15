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
    bool isFounded = false;

    int forwardHash = Animator.StringToHash("forward");
    int rightHash = Animator.StringToHash("right");
    int torsoLayerIndex;

    public bool IsVisible => this.isVisible || this.isFounded || isLocalPlayer;

    private void OnValidate()
    {
        gameObject.ValidateGetComponentInChildren(ref this.animator);
        gameObject.ValidateGetComponent(ref this.motor);
    }

    private void Awake()
    {
        this.torsoLayerIndex = this.animator.GetLayerIndex("Torso");
    }

    [ContextMenu("Get renderers")]
    void GetRenderers() => this.renderers = gameObject.GetComponentsInChildren<Renderer>();

    public void SetVisible(bool state)
    {
        this.isVisible = state;
        foreach (var r in this.renderers) r.enabled = IsVisible;
    }

    public void SetFounded(bool state)
    {
        this.isFounded = state;
        foreach (var r in this.renderers) r.enabled = IsVisible;
    }

    [Command] public void CmdSetAim(bool state, bool forced) => RpcSetAim(state, forced);
    [ClientRpc] void RpcSetAim(bool state, bool forced)
    {
        this.torsoTween?.Kill();
        if (forced)
            this.animator.SetLayerWeight(torsoLayerIndex, state ? 1f : 0f);
        else
        {
            this.torsoTween = DOVirtual.Float(
                this.animator.GetLayerWeight(this.torsoLayerIndex),
                state ? 1f : 0f,
                0.25f,
                value => this.animator.SetLayerWeight(torsoLayerIndex, value))
                .OnComplete(() => this.torsoTween = null);
        }
    }

    private void FixedUpdate()
    {
        SetGlobalMove(this.motor.NormalizedVelocity.ToV2_xz());
    }

    void SetGlobalMove(Vector2 globalDir)
    {
        var localDir = transform.InverseTransformDirection(globalDir.ToV3_x0y());
        SetLocalMove(localDir.ToV2_xz());
    }
    void SetLocalMove(Vector2 localDir)
    {
        this.animator.SetFloat(this.rightHash, localDir.x);
        this.animator.SetFloat(this.forwardHash, localDir.y);
    }

    public void Refresh()
    {

    }
}
