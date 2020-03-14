using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;
using Cinemachine;

public class NetworkPlayerCamera : MonoValidate
{
    [SerializeField] CinemachineVirtualCameraBase camera;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateGetComponentInChildren(ref this.camera);
    }

    public void SetActiveCamera(bool state)
    {
        this.camera.enabled = state;
    }

    public void Refresh()
    {

    }
}
