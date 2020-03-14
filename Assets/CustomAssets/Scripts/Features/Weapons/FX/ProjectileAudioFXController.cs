using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Extensions.Rects;
using MyTools.Helpers;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ProjectileAudioFXController : MonoValidate
{
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
    [SerializeField] ProjectileController m_ProjectileCtrl;
    [SerializeField] ProjectileAudioClipsInfo[] m_ProjectileClips;
    Dictionary<ProjectileKind, ProjectileAudioClips> m_ClipsDictionary;
    IAudioPointFactory m_Factory;
#pragma warning restore 649

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref m_ProjectileCtrl);
    }

    void Awake()
    {
        if (!ValidateGetComponent(ref m_ProjectileCtrl))
        {
            MyLogger.NotFoundObjectError<ProjectileAudioFXController, ProjectileController>();
            return;
        }
        if (!ValidateGetComponent(ref m_Factory))
        {
            MyLogger.NotFoundObjectError<ProjectileAudioFXController, IAudioPointFactory>();
            return;
        }
        Init();
    }

    void Init()
    {
        var clips = m_ProjectileClips;
        var count = clips.Length;
        var dict = new Dictionary<ProjectileKind, ProjectileAudioClips>(count);
        for (int i = 0; i < count; ++i)
            dict[clips[i].kind] = clips[i].clips;
        m_ClipsDictionary = dict;
        m_ProjectileCtrl.OnShoot += OnShootEvent;
        m_ProjectileCtrl.OnHit += OnHitEvent;
    }


    private void OnShootEvent(ProjectileInfo proj, PointInfo point)
    {
        if (!m_ClipsDictionary.TryGetValue(proj.kind, out var clips))
        {
            MyLogger.ObjectErrorFormat<ProjectileAudioFXController>("don't contain \"{0}\" kind!", proj.kind);
            return;
        }
        if (clips.shoot != null)
            proj.weapon.audio.PlayOneShot(clips.shoot);
        if (clips.flight != null)
        {
            proj.instance.Audio.clip = clips.flight;
            proj.instance.Audio.Play();
        }
    }
    private void OnHitEvent(GameObject obj, ProjectileInfo proj, PointInfo hit)
    {
        if (m_Factory == null) return;
        if (!m_ClipsDictionary.TryGetValue(proj.kind, out var clips))
        {
            MyLogger.ObjectErrorFormat<ProjectileAudioFXController>("don't contain \"{0}\" kind!", proj.kind);
            return;
        }
        proj.instance.Audio.Stop();
        proj.instance.Audio.clip = null;
        var sound = m_Factory.GetObject();
        sound.PlayOneShoot(hit.point, clips.hit);
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(ProjectileAudioFXController))]
    public class ProjectilePooledFactoryEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((ProjectileAudioFXController)target), typeof(ProjectileAudioFXController), false);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_ProjectileCtrl"));
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_ProjectileClips"), true);
            EditorGUI.EndDisabledGroup();
            serializedObject.ApplyModifiedProperties();
        }
    }
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
}
#endif

