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
    [SerializeField] Collider collider;

    bool withAim = false;

    public Vector3 NormalizedVelocity => rb.velocity / speed;
    public Vector3 Position => transform.position;
    public Rigidbody Rb => this.rb;

    public void SetEnabledCollider(bool State) => this.collider.enabled = State;
    public void SetRigidbodyKinematic(bool state) => this.rb.isKinematic = state;

    private void OnValidate()
    {
        gameObject.ValidateGetComponent(ref this.rb);
        gameObject.ValidateGetComponent(ref this.collider);
    }

    public void Move(Vector2 dir)
    {
        var dir3d = dir.ToV3_x0y();
        this.rb.velocity = dir3d * Mathf.Max(speed, this.rb.velocity.magnitude);
        if (!this.withAim)
            this.rb.rotation = Quaternion.Slerp(this.rb.rotation, Quaternion.LookRotation(dir3d, Vector3.up), TimeManager.DeltaTime * 10f);
    }

    public void SetAimRotation(Vector2 dir)
    {
        this.withAim = true;
        var dir3d = dir.ToV3_x0y();
        this.rb.rotation = Quaternion.LookRotation(dir3d, Vector3.up);
    }
    public void SetAimRotation(bool state)
    {
        this.withAim = state;
    }

    public void Refresh()
    {

    }
}
