using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFSM;
//using UnityEditor;
//using UnityEngine.UIElements;
//using UnityEditor.UIElements;

//[CustomPropertyDrawer(typeof(Parameter))]
//public class ParameterEditor : PropertyDrawer
//{
//    //VisualElement container;
//    //PropertyField valueField;

//    //System.Action<SerializedProperty> onTypeChange = delegate { };

//    //public override VisualElement CreatePropertyGUI(SerializedProperty property)
//    //{        
//    //    container = new VisualElement();

//    //    PropertyField nameField = new PropertyField(property.FindPropertyRelative("name"));
//    //    PropertyField typeField = new PropertyField(property.FindPropertyRelative("type"));
//    //    container.Add(nameField);
//    //    container.Add(typeField);


//    //    SerializedProperty type = property.FindPropertyRelative("type");

//    //    valueField = new PropertyField();
//    //    valueField.TrackPropertyValue(type, onTypeChange);   

//    //    return container;
//    //}

//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        EditorGUI.BeginProperty(position, label, property);

//        int indent = EditorGUI.indentLevel;
//        EditorGUI.indentLevel = 0;

//        Rect nameRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
//        Rect typeRect = new Rect(position.x, position.y + 20f, position.width / 2, EditorGUIUtility.singleLineHeight);
//        Rect secondRect = new Rect(position.x + position.width / 2, position.y + 20f, position.width / 2, EditorGUIUtility.singleLineHeight);

//        SerializedProperty name = property.FindPropertyRelative("name");
//        SerializedProperty type = property.FindPropertyRelative("type");

//        EditorGUI.LabelField(nameRect, name.stringValue);
//        EditorGUI.LabelField(typeRect, type.enumNames[type.enumValueIndex]);

//        switch ((ParameterType)type.intValue)
//        {
//            case ParameterType.Bool:
//                SerializedProperty boolValue = property.FindPropertyRelative("boolValue");
//                boolValue.boolValue = EditorGUI.Toggle(secondRect, boolValue.boolValue);
//                break;
//            case ParameterType.Int:
//                SerializedProperty intValue = property.FindPropertyRelative("intValue");
//                intValue.intValue = EditorGUI.IntField(secondRect, intValue.intValue);
//                break;
//            case ParameterType.Float:
//                SerializedProperty floatValue = property.FindPropertyRelative("floatValue");
//                floatValue.floatValue = EditorGUI.FloatField(secondRect, floatValue.floatValue);
//                break;
//            case ParameterType.Trigger:
//                SerializedProperty triggerValue = property.FindPropertyRelative("triggerValue");
//                triggerValue.boolValue = EditorGUI.Toggle(secondRect, triggerValue.boolValue);
//                break;
//        }

//        EditorGUI.indentLevel = indent;

//        EditorGUI.EndProperty();
//    }

//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        return (20 - EditorGUIUtility.singleLineHeight) + (EditorGUIUtility.singleLineHeight * 2);
//    }
//}
