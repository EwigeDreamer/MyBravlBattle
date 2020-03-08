using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour
{
    private void Update()
    {
        if (isServer) CheckServerEvents();
        if (isClient) CheckClientEvents();
        if (isLocalPlayer) CheckLocalPlayerEvents();
    }

    void CheckServerEvents()
    {

    }
    void CheckClientEvents()
    {

    }
    void CheckLocalPlayerEvents()
    {

    }
}
