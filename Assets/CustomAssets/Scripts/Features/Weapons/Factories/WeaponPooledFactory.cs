using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Factory;
using MyTools.Pooling;
using MyTools.Extensions.Rects;
#if UNITY_EDITOR
using UnityEditor;
#endif

    public class WeaponPooledFactory : MonoBehaviour, IWeaponFactory
    {
        [System.Serializable]
        public struct WeaponKindKeyPair
        {
            public WeaponKind kind;
            [PoolKey] public string key;
        }

#pragma warning disable 649
        [SerializeField] WeaponKindKeyPair[] m_WeaponPoolKeys;
        Dictionary<WeaponKind, string> m_WeaponPoolKeyDict;
#pragma warning restore 649

        public Weapon GetObject(WeaponKind info)
        {
            if (!m_WeaponPoolKeyDict.TryGetValue(info, out var key)) return null;
            if (!ObjectPool.I.TrySpawn(key, out var obj)) return null;
            return obj.GetComponent<Weapon>();
        }

        public bool TryGetObject(WeaponKind info, out Weapon obj)
        {
            obj = GetObject(info);
            return obj != null;
        }

        private void Awake()
        {
            var keys = m_WeaponPoolKeys;
            var count = keys.Length;
            if (count < 1) return;
            var dict = new Dictionary<WeaponKind, string>(count);
            for (int i = 0; i < count; ++i)
                dict.Add(keys[i].kind, keys[i].key);
            m_WeaponPoolKeyDict = dict;
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(WeaponPooledFactory))]
    public class WeaponPooledFactoryEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_WeaponPoolKeys"), true);
            EditorGUI.EndDisabledGroup();
            serializedObject.ApplyModifiedProperties();
        }
    }
[CustomPropertyDrawer(typeof(WeaponPooledFactory.WeaponKindKeyPair))]
public class WeaponKindKeyPairDrawer : PropertyDrawer
{
    float LineHeight => EditorGUIUtility.singleLineHeight;
    float LineSpacing => EditorGUIUtility.standardVerticalSpacing;
    float LabelWidth => EditorGUIUtility.labelWidth;
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
