using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMainCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        var cam = MainCamera.Camera.transform;
        transform.rotation = Quaternion.LookRotation(cam.forward, cam.up);
    }
}
