using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MyTools.Helpers;
using MyTools.Extensions.Vectors;
using MyTools.Extensions.GameObjects;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] NetworkPlayerView view;
    [SerializeField] NetworkPlayerMotor motor;
    [SerializeField] NetworkPlayerCombat combat;
    [SerializeField] new NetworkPlayerCamera camera;
    [SerializeField] NetworkPlayerGrassXRay grassHider;
    [SerializeField] NetworkPlayerStatusBar statusBar;
    [SerializeField] NetworkPlayerHealth health;


    public NetworkPlayerView View => this.view;
    public NetworkPlayerMotor Motor => this.motor;
    public NetworkPlayerCombat Combat => this.combat;
    public NetworkPlayerCamera Camera => this.camera;
    public NetworkPlayerHealth Health => this.health;

    private void OnValidate()
    {
        gameObject.ValidateGetComponent(ref this.view);
        gameObject.ValidateGetComponent(ref this.motor);
        gameObject.ValidateGetComponent(ref this.combat);
        gameObject.ValidateGetComponent(ref this.camera);
        gameObject.ValidateGetComponent(ref this.grassHider);
        gameObject.ValidateGetComponent(ref this.health);
    }

    private void Awake()
    {
        transform.SetParent(CustomNetworkManager.I.transform);
        this.camera.SetActiveCamera(false);
        this.health.OnDead += () =>
        {
            if (!isLocalPlayer) return;
            this.view.CmdDead();
            if (CharacterControlMediator.I != null) CharacterControlMediator.I.SetActive(false);
        };
    }

    private void Start()
    {
        var connToClient = connectionToClient;
        var connToServer = connectionToServer;
        var clientAddress = connToClient != null ? $" [client: {connToClient.address}]" : "";
        var serverAddress = connToServer != null ? $" [server: {connToServer.address}]" : "";
        name = $"{typeof(NetworkPlayer)}{clientAddress}{serverAddress}";
        PlayerController.I.Register(this);
        if (CharacterControlMediator.I != null) CharacterControlMediator.I.SetActive(true);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        this.camera.SetActiveCamera(true);
        PlayerController.I.RegisterLocal(this);
        this.combat.CmdSetWeapon(WeaponKind.Pistol);
    }

    public override void OnNetworkDestroy()
    {
        base.OnNetworkDestroy();
        PlayerController.I.UnregisterLocal(this);
        PlayerController.I.Unregister(this);
    }

    private void OnDestroy()
    {
        PlayerController.I.UnregisterLocal(this);
        PlayerController.I.Unregister(this);
    }


    [Command]
    public void CmdRefresh() => RpcRefresh();
    [ClientRpc]
    void RpcRefresh()
    {
        view.Refresh();
        motor.Refresh();
        combat.Refresh();
        camera.Refresh();
        statusBar.Refresh();
    }
}
