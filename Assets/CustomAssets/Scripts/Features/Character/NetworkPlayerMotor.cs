using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MyTools.Extensions.GameObjects;
using MyTools.Extensions.Vectors;

public class NetworkPlayerMotor : NetworkBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed = 10f;

    public Vector3 NormalizedVelocity => rb.velocity / speed;

    private void OnValidate()
    {
        gameObject.ValidateGetComponent(ref this.rb);
    }

    [Command]
    public void CmdMove(Vector2 dir)
    {
        var dir3d = dir.ToV3_x0y();
        this.rb.velocity = dir3d * Mathf.Max(speed, this.rb.velocity.magnitude);
    }

    [Command]
    public void CmdTeleport(Vector3 point)
    {
        Debug.LogWarning($"TELEPORT 111! {point}", gameObject);
        transform.position = point;
    }
}
