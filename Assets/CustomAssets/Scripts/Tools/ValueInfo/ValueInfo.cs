using System;
using UnityEngine;

namespace MyTools.ValueInfo
{
    [Serializable]
    public struct IntInfo
    {
        [SerializeField] int m_Min;
        [SerializeField] int m_Max;
        [SerializeField] int m_Value;

        public IntInfo(int value)
        {
            if (value != 0)
            {
                bool positive = value > 0;
                m_Min = positive ? 0 : value;
                m_Max = positive ? value : 0;
            }
            else
            {
                m_Min = 0;
                m_Max = 1;
            }
            m_Value = value;
        }

        public int Min { get => m_Min; set => SetMin(value); }
        public int Max { get => m_Max; set => SetMax(value); }
        public int Value { get => m_Value; set => SetValue(value); }

        public float ValueToMaxRatio => m_Max == 0 ? 0f : ((float)m_Value) / m_Max;
        public float Normalize => (float)m_Value / (m_Max - m_Min);

        public bool IsMax => m_Value == m_Max;
        public bool IsMin => m_Value == m_Min;
        public bool IsZero => m_Value == 0;
        public void ToMin() => m_Value = m_Min;
        public void ToMax() => m_Value = m_Max;
        public void ToZero() => m_Value = m_Min = 0;

        void SetMin(int min)
        {
            if (min > m_Max) min = m_Max;
            m_Min = min;
            if (m_Value < m_Min) m_Value = m_Min;
        }
        void SetMax(int max)
        {
            if (max < m_Min) max = m_Min;
            m_Max = max;
            if (m_Value > m_Max) m_Value = m_Max;
        }
        void SetValue(int value)
        {
            if (value < m_Min) value = m_Min;
            else if (value > m_Max) value = m_Max;
            m_Value = value;
        }

        public static implicit operator int(IntInfo info) => info.m_Value;
        public static IntInfo operator +(IntInfo a, int b) { a.Value += b; return a; }
        public static IntInfo operator +(int a, IntInfo b) { b.Value += a; return b; }
        public static IntInfo operator -(IntInfo a, int b) { a.Value -= b; return a; }
        public static int operator -(int a, IntInfo b) { return a - b.Value; }
    }


    [Serializable]
    public struct FloatInfo
    {
        [SerializeField] float m_Min;
        [SerializeField] float m_Max;
        [SerializeField] float m_Value;

        public FloatInfo(float value)
        {
            bool positive = value > 0;
            m_Min = positive ? 0 : value;
            m_Max = positive ? value : 0;
            m_Value = value;
        }

        public float Min { get => m_Min; set => SetMin(value); }
        public float Max { get => m_Max; set => SetMax(value); }
        public float Value { get => m_Value; set => SetValue(value); }

        public float ValueToMaxRatio => m_Max.IsVerySmall() ? 0f : m_Value / m_Max;
        public float Normalize => m_Value / (m_Max - m_Min);

        public bool IsMax => (m_Value - m_Max).IsVerySmall();
        public bool IsMin => (m_Value - m_Min).IsVerySmall();
        public bool IsZero => m_Value.IsVerySmall();
        public void ToMin() => m_Value = m_Min;
        public void ToMax() => m_Value = m_Max;
        public void ToZero() => m_Value = m_Min = 0f;

        void SetMin(float min)
        {
            if (min > m_Max) min = m_Max;
            m_Min = min;
            if (m_Value < m_Min) m_Value = m_Min;
        }
        void SetMax(float max)
        {
            if (max < m_Min) max = m_Min;
            m_Max = max;
            if (m_Value > m_Max) m_Value = m_Max;
        }
        void SetValue(float value)
        {
            if (value < m_Min) value = m_Min;
            else if (value > m_Max) value = m_Max;
            m_Value = value;
        }

        public static implicit operator float(FloatInfo info) => info.m_Value;
        public static FloatInfo operator +(FloatInfo a, float b) { a.Value += b; return a; }
        public static FloatInfo operator +(float a, FloatInfo b) { b.Value += a; return b; }
        public static FloatInfo operator -(FloatInfo a, float b) { a.Value -= b; return a; }
        public static float operator -(float a, FloatInfo b) { return a - b.Value; }
    }

    public static class ExtensionMethods
    {
        public static bool IsVerySmall(this float value) => value < 1e-5f && value > -1e-5f;
    }

#if UNITY_EDITOR
    namespace Editor
    {
        using UnityEditor;
        using MyTools.Extensions.Rects;
        using MyTools.Extensions.Editor;

        [CustomPropertyDrawer(typeof(IntInfo))]
        public class IntInfoDrawer : PropertyDrawer
        {
            float LineHeight => EditorGUIUtility.singleLineHeight;
            float LineSpacing => EditorGUIUtility.standardVerticalSpacing;
            float LabelWidth => EditorGUIUtility.labelWidth;

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                int lines = 2;
                return (LineHeight * lines) + (LineSpacing * (lines - 1));
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                int indentLevel = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                position.GetCustomLabelFieldPair(LabelWidth, out var l, out var f);
                GUI.Box(f, "");
                l.GetRowsNonAlloc(LineSpacing, out var l1, out var l2);
                EditorGUI.LabelField(l1, label);
                f.GetRowsNonAlloc(LineSpacing, out var f1, out var f2);
                f1.GetColumnsNonAlloc(LineSpacing, out var f11, out var f12, out var f13);
                var minProp = property.FindPropertyRelative("m_Min");
                var maxProp = property.FindPropertyRelative("m_Max");
                var valueProp = property.FindPropertyRelative("m_Value");

                float labWidthTmp = EditorGUIUtility.labelWidth;

                EditorGUI.ProgressBar(f2, valueProp.intValue / (float)(maxProp.intValue - minProp.intValue), label.text);

                EditorGUIUtility.labelWidth = 23f;
                EditorGUI.PropertyField(f11, minProp);
                EditorGUIUtility.labelWidth = 37f;
                //EditorGUI.BeginDisabledGroup(true);
                EditorGUI.PropertyField(f12, valueProp);
                //EditorGUI.EndDisabledGroup();
                EditorGUIUtility.labelWidth = 28f;
                EditorGUI.PropertyField(f13, maxProp);

                property.serializedObject.ApplyModifiedProperties();

                EditorGUIUtility.labelWidth = labWidthTmp;

                EditorGUI.indentLevel = indentLevel;
            }
        }

        [CustomPropertyDrawer(typeof(FloatInfo))]
        public class FloatInfoDrawer : PropertyDrawer
        {
            float LineHeight => EditorGUIUtility.singleLineHeight;
            float LineSpacing => EditorGUIUtility.standardVerticalSpacing;
            float LabelWidth => EditorGUIUtility.labelWidth;

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                int lines = 2;
                return (LineHeight * lines) + (LineSpacing * (lines - 1));
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                int indentLevel = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                position.GetCustomLabelFieldPair(LabelWidth, out var l, out var f);
                GUI.Box(f, "");
                l.GetRowsNonAlloc(LineSpacing, out var l1, out var l2);
                EditorGUI.LabelField(l1, label);
                f.GetRowsNonAlloc(LineSpacing, out var f1, out var f2);
                f1.GetColumnsNonAlloc(LineSpacing, out var f11, out var f12, out var f13);
                var minProp = property.FindPropertyRelative("m_Min");
                var maxProp = property.FindPropertyRelative("m_Max");
                var valueProp = property.FindPropertyRelative("m_Value");

                float labWidthTmp = EditorGUIUtility.labelWidth;

                EditorGUI.ProgressBar(f2, valueProp.floatValue / (maxProp.floatValue - minProp.floatValue), label.text);

                EditorGUIUtility.labelWidth = 23f;
                EditorGUI.PropertyField(f11, minProp);
                EditorGUIUtility.labelWidth = 37f;
                //EditorGUI.BeginDisabledGroup(true);
                EditorGUI.PropertyField(f12, valueProp);
                //EditorGUI.EndDisabledGroup();
                EditorGUIUtility.labelWidth = 28f;
                EditorGUI.PropertyField(f13, maxProp);

                property.serializedObject.ApplyModifiedProperties();

                EditorGUIUtility.labelWidth = labWidthTmp;

                EditorGUI.indentLevel = indentLevel;
            }
        }
    }
#endif
}
