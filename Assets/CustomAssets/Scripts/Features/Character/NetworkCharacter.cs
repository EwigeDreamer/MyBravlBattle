using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MyTools.Extensions.Vectors;
using Cinemachine;

public class NetworkCharacter : NetworkBehaviour
{
    [SerializeField] CinemachineVirtualCamera camera;
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed = 5f;
    private void Awake()
    {
        transform.SetParent(CharacterController.I.TR);
        SetActiveCamera(false);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        SetActiveCamera(true);
    }

    public void Move(Vector2 dir)
    {
        var dir3d = dir.ToV3_x0y();
        rb.velocity = dir3d * Mathf.Max(speed, rb.velocity.magnitude);
    }

    void SetActiveCamera(bool state)
    {
        camera.enabled = state;
    }
}
