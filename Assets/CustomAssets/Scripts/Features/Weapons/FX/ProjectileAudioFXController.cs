using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Extensions.Rects;
using MyTools.Helpers;
using MyTools.Singleton;
using UnityEngine.Networking;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class ProjectileAudioFXMessage : MessageBase
{
    public ProjectileEventType eventType;
    public ProjectileKind kind;
    public Vector3 point;
}

public class ProjectileAudioFXController : MonoSingleton<ProjectileAudioFXController>
{
    const short msgType = MsgType.Highest + 5;

    [System.Serializable]
    public struct ProjectileAudioClips
    {
        public AudioClip shoot;
        public AudioClip flight;
        public AudioClip hit;
    }
    [System.Serializable]
    public struct ProjectileAudioClipsInfo
    {
        public ProjectileKind kind;
        public ProjectileAudioClips clips;
    }

#pragma warning disable 649
    [SerializeField] CustomNetworkManager manager;
    [SerializeField] ProjectileController projectileCtrl;
    [SerializeField] ProjectileAudioClipsInfo[] projectileClips;
    Dictionary<ProjectileKind, ProjectileAudioClips> clipsDictionary;
    IAudioPointFactory factory;
#pragma warning restore 649

    ProjectileAudioFXMessage msg = new ProjectileAudioFXMessage();

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
        var clips = this.projectileClips;
        var count = clips.Length;
        var dict = new Dictionary<ProjectileKind, ProjectileAudioClips>(count);
        for (int i = 0; i < count; ++i)
            dict[clips[i].kind] = clips[i].clips;
        this.clipsDictionary = dict;
        manager.OnClientStarted += client => client.RegisterHandler(msgType, ReceiveFxEvent);
        this.projectileCtrl.OnShoot += (proj, point) => BroadcastFxEvent(ProjectileEventType.Shoot, proj.kind, point.point);
        this.projectileCtrl.OnHit += (_, proj, point) => BroadcastFxEvent(ProjectileEventType.Hit, proj.kind, point.point);
    }


    void OnShoot(ProjectileKind kind, Vector3 point)
    {
        if (!this.clipsDictionary.TryGetValue(kind, out var clips))
        {
            Debug.LogError($"{typeof(ProjectileAudioFXController).Name}: don't contain \"{kind}\" kind!", gameObject);
            return;
        }
        if (clips.shoot != null)
        {
            var sound = factory.GetObject();
            sound.PlayOneShoot(point, clips.shoot, 0);
        }
    }
    void OnHit(ProjectileKind kind, Vector3 point)
    {
        if (!this.clipsDictionary.TryGetValue(kind, out var clips))
        {
            Debug.LogError($"{typeof(ProjectileAudioFXController).Name}: don't contain \"{kind}\" kind!", gameObject);
            return;
        }
        if (clips.hit != null)
        {
            var sound = factory.GetObject();
            sound.PlayOneShoot(point, clips.hit, 100);
        }
    }

    public void BroadcastFxEvent(ProjectileEventType eventType, ProjectileKind kind, Vector3 point)
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
        var msg = netMsg.ReadMessage<ProjectileAudioFXMessage>();
        if (msg == null) return;
        if (msg.eventType == ProjectileEventType.Shoot) OnShoot(msg.kind, msg.point);
        if (msg.eventType == ProjectileEventType.Hit) OnHit(msg.kind, msg.point);
    }


#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ProjectileAudioFXController.ProjectileAudioClipsInfo))]
    public class ProjectileAudioClipsInfoDrawer : PropertyDrawer
    {
        float LineHeight { get { return EditorGUIUtility.singleLineHeight; } }
        float LineSpacing { get { return EditorGUIUtility.standardVerticalSpacing; } }
        float LabelWidth { get { return EditorGUIUtility.labelWidth; } }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int lines = 3;
            return (LineHeight * lines) + (LineSpacing * (lines - 1));
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var tmpW = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 50f;
            var rect = EditorGUI.IndentedRect(position);
            var tmpInd = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            GUI.Box(rect, "");
            rect.GetColumnsNonAlloc(LineSpacing, out var rect1, out var rect2);
            rect1.GetRowsNonAlloc(LineSpacing, out var rect1_line1, out var rect1_line2, out var rect1_line3);
            rect2.GetRowsNonAlloc(LineSpacing, out var rect2_line1, out var rect2_line2, out var rect2_line3);

            EditorGUI.PropertyField(rect1_line1, property.FindPropertyRelative("kind"));
            EditorGUI.PropertyField(rect2_line1, property.FindPropertyRelative("clips").FindPropertyRelative("shoot"));
            EditorGUI.PropertyField(rect2_line2, property.FindPropertyRelative("clips").FindPropertyRelative("flight"));
            EditorGUI.PropertyField(rect2_line3, property.FindPropertyRelative("clips").FindPropertyRelative("hit"));

            EditorGUIUtility.labelWidth = tmpW;
            EditorGUI.indentLevel = tmpInd;
        }
    }
#endif
}

