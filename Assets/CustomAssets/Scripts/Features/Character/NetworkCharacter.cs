using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkCharacter : NetworkBehaviour
{
    private void Awake()
    {
        transform.SetParent(CharacterController.I.TR);
    }
}
