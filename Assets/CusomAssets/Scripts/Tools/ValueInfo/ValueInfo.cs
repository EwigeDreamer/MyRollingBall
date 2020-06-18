using System;
using UnityEngine;

namespace MyTools.ValueInfo
{
    [Serializable]
    public struct IntInfo
    {
        [SerializeField] int min;
        [SerializeField] int max;
        [SerializeField] int value;

        public IntInfo(int min, int max, int value)
        {
            if (min > value) min = value;
            if (max < value) max = value;
            this.min = min;
            this.max = max;
            this.value = value;
        }

        public int Min { get => this.min; set => SetMin(value); }
        public int Max { get => this.max; set => SetMax(value); }
        public int Value { get => this.value; set => SetValue(value); }

        public float ValueToMaxRatio => this.max == 0 ? 0f : ((float)this.value) / this.max;
        public float Normalize => (float)this.value / (this.max - this.min);

        public bool IsMax => this.value == this.max;
        public bool IsMin => this.value == this.min;
        public bool IsZero => this.value == 0;
        public IntInfo ToMin() { this.value = this.min; return this; }
        public IntInfo ToMax() { this.value = this.max; return this; }
        public IntInfo ToZero() { this.value = this.min = 0; return this; }

        public IntInfo SetMin(int min)
        {
            if (min > this.max) min = this.max;
            this.min = min;
            if (this.value < this.min) this.value = this.min;
            return this;
        }
        public IntInfo SetMax(int max)
        {
            if (max < this.min) max = this.min;
            this.max = max;
            if (this.value > this.max) this.value = this.max;
            return this;
        }
        public IntInfo SetValue(int value)
        {
            if (value < this.min) value = this.min;
            else if (value > this.max) value = this.max;
            this.value = value;
            return this;
        }

        public static implicit operator int(IntInfo info) => info.value;
        public static IntInfo operator +(IntInfo a, int b) { a.Value += b; return a; }
        public static IntInfo operator +(int a, IntInfo b) { b.Value += a; return b; }
        public static IntInfo operator -(IntInfo a, int b) { a.Value -= b; return a; }
        public static int operator -(int a, IntInfo b) { return a - b.Value; }
    }


    [Serializable]
    public struct FloatInfo
    {
        [SerializeField] float min;
        [SerializeField] float max;
        [SerializeField] float value;

        public FloatInfo(float min, float max, float value)
        {
            if (min > value) min = value;
            if (max < value) max = value;
            this.min = min;
            this.max = max;
            this.value = value;
        }

        public float Min { get => this.min; set => SetMin(value); }
        public float Max { get => this.max; set => SetMax(value); }
        public float Value { get => this.value; set => SetValue(value); }

        public float ValueToMaxRatio => this.max.IsVerySmall() ? 0f : this.value / this.max;
        public float Normalize => this.value / (this.max - this.min);

        public bool IsMax => (this.value - this.max).IsVerySmall();
        public bool IsMin => (this.value - this.min).IsVerySmall();
        public bool IsZero => this.value.IsVerySmall();
        public FloatInfo ToMin() { this.value = this.min; return this; }
        public FloatInfo ToMax() { this.value = this.max; return this; }
        public FloatInfo ToZero() { this.value = this.min = 0f; return this; }

        public FloatInfo SetMin(float min)
        {
            if (min > this.max) min = this.max;
            this.min = min;
            if (this.value < this.min) this.value = this.min;
            return this;
        }
        public FloatInfo SetMax(float max)
        {
            if (max < this.min) max = this.min;
            this.max = max;
            if (this.value > this.max) this.value = this.max;
            return this;
        }
        public FloatInfo SetValue(float value)
        {
            if (value < this.min) value = this.min;
            else if (value > this.max) value = this.max;
            this.value = value;
            return this;
        }

        public static implicit operator float(FloatInfo info) => info.value;
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
                var minProp = property.FindPropertyRelative("min");
                var maxProp = property.FindPropertyRelative("max");
                var valueProp = property.FindPropertyRelative("value");

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
                var minProp = property.FindPropertyRelative("min");
                var maxProp = property.FindPropertyRelative("max");
                var valueProp = property.FindPropertyRelative("value");

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
