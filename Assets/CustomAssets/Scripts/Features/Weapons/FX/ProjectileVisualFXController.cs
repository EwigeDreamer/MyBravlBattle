using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;
using OneLine;
using MyTools.Pooling;
using System.Collections.ObjectModel;
using MyTools.Singleton;
using UnityEngine.Networking;


[System.Serializable]
public class ProjectileVisualFXMessage : MessageBase
{
    public ProjectileEventType eventType;
    public ProjectileKind kind;
    public PointInfo point;
}

public class ProjectileVisualFXController : MonoSingleton<ProjectileVisualFXController>
{
    const short msgType = MsgType.Highest + 4;

    [SerializeField] CustomNetworkManager manager;
    [SerializeField] ProjectileController projectileCtrl;

    [SerializeField, OneLine(Header = LineHeader.Short)]
    ProjectileEffectInfo[] infoList;
    IVisualEffectPointFactory factory;

    Dictionary<ProjectileKind, ProjectileEffectInfo> dict = new Dictionary<ProjectileKind, ProjectileEffectInfo>();

    ProjectileVisualFXMessage msg = new ProjectileVisualFXMessage();

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref this.projectileCtrl);
        ValidateFind(ref this.manager);
    }
    protected override void Awake()
    {
        base.Awake();
        ValidateGetComponent(ref this.factory);
        this.dict = new Dictionary<ProjectileKind, ProjectileEffectInfo>();
        foreach (var item in this.infoList) this.dict[item.kind] = item;
        this.manager.OnClientStarted += client => client.RegisterHandler(msgType, ReceiveFxEvent);
        this.projectileCtrl.OnShoot += (proj, point) => BroadcastFxEvent(ProjectileEventType.Shoot, proj.kind, point);
        this.projectileCtrl.OnHit += (_, proj, point) => BroadcastFxEvent(ProjectileEventType.Hit, proj.kind, point);
    }

    void OnShoot(ProjectileKind kind, PointInfo point)
    {
        if (!this.dict.TryGetValue(kind, out var info)) return;
        Debug.LogWarning($"SHOOT EFFECT! kind: {kind}, point: {point.point}");

        var effect = factory.GetObject(info.shoot);
        effect.TR.position = point.point;
        effect.TR.rotation = Quaternion.LookRotation(point.direction);
    }
    void OnHit(ProjectileKind kind, PointInfo point)
    {
        if (!this.dict.TryGetValue(kind, out var info)) return;
        Debug.LogWarning($"IMPACT EFFECT! kind: {kind}, point: {point.point}");

        var effect = factory.GetObject(info.impact);
        effect.TR.position = point.point;
        effect.TR.rotation = Quaternion.LookRotation(point.normal);
    }

    public void BroadcastFxEvent(ProjectileEventType eventType, ProjectileKind kind, PointInfo point)
    {
        if (!NetworkServer.active) return;
        this.msg.eventType = eventType;
        this.msg.kind = kind;
        this.msg.point = point;
        foreach (var conn in NetworkServer.connections)
            NetworkServer.SendToClient(conn.connectionId, msgType, this.msg);
    }
    public void ReceiveFxEvent(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<ProjectileVisualFXMessage>();
        if (msg == null) return;
        if (msg.eventType == ProjectileEventType.Shoot) OnShoot(msg.kind, msg.point);
        if (msg.eventType == ProjectileEventType.Hit) OnHit(msg.kind, msg.point);
    }


    [System.Serializable]
    public class ProjectileEffectInfo
    {
        public ProjectileKind kind;
        [PoolKey] public string shoot;
        [PoolKey] public string impact;
    }
}

