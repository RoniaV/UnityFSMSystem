//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using UnityFSM;

//[CustomPropertyDrawer(typeof(Transition))]
//public class TransitionEditor : PropertyDrawer
//{
//    int parameterIndex;
//    float heightMultiplier;

//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        EditorGUI.BeginProperty(position, label, property);

//        int indent = EditorGUI.indentLevel;
//        EditorGUI.indentLevel = 0;
//        heightMultiplier = 0;

//        //Defino el Rect de cada objeto de la GUI
//        Rect nameRect = new Rect(position.x, position.y + (EditorGUIUtility.singleLineHeight * heightMultiplier), position.width, EditorGUIUtility.singleLineHeight);
//        heightMultiplier++;
//        Rect stateRect = new Rect(position.x, position.y + (EditorGUIUtility.singleLineHeight * heightMultiplier), position.width, EditorGUIUtility.singleLineHeight);
//        heightMultiplier++;
//        Rect parameterPopupRect = new Rect(position.x, position.y + (EditorGUIUtility.singleLineHeight * heightMultiplier), position.width / 2, EditorGUIUtility.singleLineHeight);
//        Rect buttonRect = new Rect(position.x + position.width / 2, position.y + (EditorGUIUtility.singleLineHeight * heightMultiplier), position.width / 2, EditorGUIUtility.singleLineHeight);
//        heightMultiplier++;
//        Rect conditionsRect = new Rect(position.x, position.y + (EditorGUIUtility.singleLineHeight * heightMultiplier), position.width, EditorGUIUtility.singleLineHeight);
//        heightMultiplier++;

//        //Recojo las propiedades y las pongo en la GUI
//        SerializedProperty state = property.FindPropertyRelative("destination");
//        SerializedProperty name = property.FindPropertyRelative("name");

//        EditorGUI.LabelField(nameRect, name.stringValue);
//        state.objectReferenceValue = EditorGUI.ObjectField(stateRect, state.objectReferenceValue, typeof(State), true);

//        //Creo un string array y lo pueblo con los nombres de los parametros
//        SerializedProperty fSM = property.FindPropertyRelative("fSM");
//        UFSM fSMObj = (UFSM)fSM.objectReferenceValue;
//        string[] parametersName = new string[0];

//        parametersName = new string[fSMObj.parameters.Count];

//        for (int i = 0; i < parametersName.Length; i++)
//        {
//            parametersName[i] = fSMObj.parameters[i].name;
//        }

//        parameterIndex = EditorGUI.Popup(parameterPopupRect, parameterIndex, parametersName);

//        if (GUI.Button(buttonRect, "Add Condition"))
//        {
//            SerializedObject transition = property.serializedObject;
//            //Transition instance = (Transition)transition.targetObject;
//        }

//        EditorGUI.PropertyField(conditionsRect, property.FindPropertyRelative("conditions"));

//        EditorGUI.indentLevel = indent;

//        EditorGUI.EndProperty();
//    }

//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        return (20 - EditorGUIUtility.singleLineHeight) + (EditorGUIUtility.singleLineHeight * heightMultiplier);
//    }
//}
