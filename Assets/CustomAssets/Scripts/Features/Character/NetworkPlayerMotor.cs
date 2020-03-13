using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MyTools.Extensions.GameObjects;
using MyTools.Extensions.Vectors;
using MyTools.Helpers;

public class NetworkPlayerMotor : NetworkBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed = 10f;

    public bool WithAim { get; set; } = false;

    public Vector3 NormalizedVelocity => rb.velocity / speed;

    private void OnValidate()
    {
        gameObject.ValidateGetComponent(ref this.rb);
    }

    [Command]
    public void CmdMove(Vector2 dir) => RpcMove(dir);

    [ClientRpc]
    void RpcMove(Vector2 dir)
    {
        var dir3d = dir.ToV3_x0y();
        this.rb.velocity = dir3d * Mathf.Max(speed, this.rb.velocity.magnitude);
        if (!WithAim)
            this.rb.rotation = Quaternion.Slerp(this.rb.rotation, Quaternion.LookRotation(dir3d, Vector3.up), TimeManager.DeltaTime * 10f);
    }
}
