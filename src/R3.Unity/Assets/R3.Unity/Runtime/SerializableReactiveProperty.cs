﻿using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace R3
{
    [Serializable]
    public class SerializableReactiveProperty<T> : ReactiveProperty<T>, ISerializationCallbackReceiver
    {
        [SerializeField]
        T value;

        public SerializableReactiveProperty()
            : base(default!)
        {
        }

        public SerializableReactiveProperty(T value)
            : base(value)
        {
        }

        protected override void OnValueChanged(T value)
        {
            this.value = value;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            GetValueRef() = this.value; // force set
        }
    }

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(SerializableReactiveProperty<>))]
    internal class SerializableReactivePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var p = property.FindPropertyRelative("value");
            if (p.propertyType == SerializedPropertyType.Quaternion)
            {
                label.text += "(EulerAngles)";
                EditorGUI.PropertyField(position, p, label, true);
            }
            else
            {
                EditorGUI.PropertyField(position, p, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var p = property.FindPropertyRelative("value");
            if (p.propertyType == SerializedPropertyType.Quaternion)
            {
                // Quaternion is Vector3(EulerAngles)
                return EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector3, label);
            }
            else
            {
                return EditorGUI.GetPropertyHeight(p);
            }
        }
    }

#endif
}
