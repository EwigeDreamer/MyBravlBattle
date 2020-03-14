using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Factory;
using MyTools.Pooling;
using System;
using MyTools.Extensions.Rects;
#if UNITY_EDITOR
using UnityEditor;
#endif

    public class ProjectilePooledFactory : MonoBehaviour, IProjectileFactory
    {
        [System.Serializable]
        public struct ProjectileKindKeyPair
        {
            public ProjectileKind kind;
            [PoolKey] public string key;
        }

#pragma warning disable 649
        [SerializeField] ProjectileKindKeyPair[] m_ProjectilePoolKeys;
        Dictionary<ProjectileKind, string> m_ProjectilePoolKeyDict;
#pragma warning restore 649

        public Projectile GetObject(ProjectileKind info)
        {
            if (!m_ProjectilePoolKeyDict.TryGetValue(info, out var key))
            {
                MyLogger.ObjectErrorFormat<ProjectilePooledFactory>("don't contain \"{0}\" kind!", info);
                return null;
            }
            if (!ObjectPool.I.TrySpawn(key, out var obj))
            {
                MyLogger.ObjectErrorFormat<ProjectilePooledFactory>("\"{0}\" kan't be spawned!", key);
                return null;
            }
            return obj.GetComponent<Projectile>();
        }

        public bool TryGetObject(ProjectileKind info, out Projectile obj)
        {
            obj = GetObject(info);
            return obj != null;
        }

        private void Awake()
        {
            var keys = m_ProjectilePoolKeys;
            var count = keys.Length;
            if (count < 1) return;
            var dict = new Dictionary<ProjectileKind, string>(count);
            for (int i = 0; i < count; ++i)
                dict[keys[i].kind] = keys[i].key;
            m_ProjectilePoolKeyDict = dict;
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(ProjectilePooledFactory))]
    public class ProjectilePooledFactoryEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((ProjectilePooledFactory)target), typeof(ProjectilePooledFactory), false);
            EditorGUI.EndDisabledGroup();
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_ProjectilePoolKeys"), true);
            EditorGUI.EndDisabledGroup();
            serializedObject.ApplyModifiedProperties();
        }
    }
[CustomPropertyDrawer(typeof(ProjectilePooledFactory.ProjectileKindKeyPair))]
public class ProjectileKindKeyPairDrawer : PropertyDrawer
{
    float LineHeight { get { return EditorGUIUtility.singleLineHeight; } }
    float LineSpacing { get { return EditorGUIUtility.standardVerticalSpacing; } }
    float LabelWidth { get { return EditorGUIUtility.labelWidth; } }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var tmpW = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 50f;
        var rect = EditorGUI.IndentedRect(position);
        var tmpInd = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        GUI.Box(rect, "");
        rect.GetColumnsNonAlloc(LineSpacing, out var rect1, out var rect2);
        EditorGUI.PropertyField(rect1, property.FindPropertyRelative("kind"));
        EditorGUI.PropertyField(rect2, property.FindPropertyRelative("key"));

        EditorGUIUtility.labelWidth = tmpW;
        EditorGUI.indentLevel = tmpInd;
    }
}
#endif
