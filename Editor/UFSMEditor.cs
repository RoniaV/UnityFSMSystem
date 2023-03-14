using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityFSM;
using System;

[CanEditMultipleObjects]
[CustomEditor(typeof(UFSM), true, isFallback = true)]
public class UFSMEditor : Editor
{
    UFSM fsm;

    ParameterType parameterType;
    string parameterName;
    bool parametersExpanded = true;

    bool anyStateExpanded = true;
    string newTransitionName;

    bool[] transitionExpanded;
    int[] parameterIndexes;

    public void OnEnable()
    {
        fsm = (UFSM)target;

        SetExpanded();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        DrawParameters();

        EditorGUILayout.Space();

        DrawTransitions(fsm.anyStateTransitions);
    }

    private void DrawParameterButtons()
    {
        GUILayout.BeginHorizontal();
        parameterType = (ParameterType)EditorGUILayout.EnumPopup(parameterType);
        parameterName = EditorGUILayout.TextField(parameterName);

        if (GUILayout.Button("Add Parameter"))
        {
            if (parameterName != null)
            {
                Undo.RecordObject(target, "Parameter Added");
                fsm.AddParameter(parameterType, parameterName);

                parameterType = 0;
                parameterName = null;

                foreach (Parameter p in fsm.parameters)
                {
                    Debug.Log(p?.name);
                }
            }
        }

        if(GUILayout.Button("Delete All Parameters"))
        {
            fsm.DeleteAllParameters();
        }

        GUILayout.EndHorizontal();
    }

    private void DrawParameters()
    {
        parametersExpanded = EditorGUILayout.Foldout(parametersExpanded, "Parameters");
        EditorGUI.indentLevel++;

        if(parametersExpanded)
        {

            for(int i = 0; i < fsm.parameters.Count; i++)
            {
                if (fsm.parameters[i] != null)
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUIUtility.labelWidth = 50;

                    EditorGUILayout.LabelField(fsm.parameters[i].name);
                    EditorGUILayout.LabelField(fsm.parameters[i].type.ToString());

                    Undo.RecordObject(target, "Changed Parameter Value");
                    switch (fsm.parameters[i].type)
                    {
                        case ParameterType.Bool:
                            fsm.parameters[i].boolValue = EditorGUILayout.Toggle(fsm.parameters[i].boolValue);
                            break;
                        case ParameterType.Int:
                            fsm.parameters[i].intValue = EditorGUILayout.IntField(fsm.parameters[i].intValue);
                            break;
                        case ParameterType.Float:
                            fsm.parameters[i].floatValue = EditorGUILayout.FloatField(fsm.parameters[i].floatValue);
                            break;
                        case ParameterType.Trigger:
                            fsm.parameters[i].triggerValue = EditorGUILayout.Toggle(fsm.parameters[i].triggerValue);
                            break;
                    }

                    EditorGUIUtility.labelWidth = 0;

                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.Space();
            DrawParameterButtons();
        }

        EditorGUI.indentLevel--;
    }

    private void DrawTransitionsButton()
    {
        EditorGUILayout.BeginHorizontal();
        newTransitionName = EditorGUILayout.TextField(newTransitionName);

        if (GUILayout.Button("Add Transition"))
        {
            if (newTransitionName != null)
            {
                Undo.RecordObject(target, "Transition Added");
                fsm.anyStateTransitions.Add(new Transition(newTransitionName));

                newTransitionName = null;
            }
        }

        if (GUILayout.Button("Delete All Transitions"))
        {
            fsm.DeleteAllAnyStateTransitions();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawTransitions(List<Transition> transitions)
    {
        anyStateExpanded = EditorGUILayout.Foldout(anyStateExpanded, "Any State Transitions");

        EditorGUI.indentLevel++;
        if (transitionExpanded.Length != transitions.Count)
            SetExpanded();

        if (anyStateExpanded)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                transitionExpanded[i] = EditorGUILayout.Foldout(transitionExpanded[i], transitions[i].name);
                transitions[i].destination = (State)EditorGUILayout.ObjectField(transitions[i].destination, typeof(State), true);
                EditorGUILayout.EndHorizontal();

                if (transitionExpanded[i])
                {
                    EditorGUI.indentLevel++;

                    DrawConditions(transitions[i]);

                    DrawConditionButton(i, transitions[i]);

                    EditorGUI.indentLevel--;
                }
            }

            EditorGUILayout.Space();
            DrawTransitionsButton();
        }

        EditorGUI.indentLevel--;
    }

    private void DrawConditions(Transition transition)
    {
        for (int i = 0; i < transition.conditions.Count; i++)
        {
            Parameter parameterToDraw = fsm.GetParameter(transition.conditions[i].selectedParameterName);

            if (parameterToDraw != null)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(parameterToDraw.name);

                Undo.RecordObject(target, "Condition Criteria Changed");
                switch (parameterToDraw.type)
                {
                    case ParameterType.Bool:
                        transition.conditions[i].boolCriteria = (Condition.BoolCriteria)EditorGUILayout.EnumPopup(transition.conditions[i].boolCriteria);
                        break;
                    case ParameterType.Int:
                        transition.conditions[i].intCriteria = (Condition.IntCriteria)EditorGUILayout.EnumPopup(transition.conditions[i].intCriteria);
                        transition.conditions[i].intValueComparision = EditorGUILayout.IntField(transition.conditions[i].intValueComparision);
                        break;
                    case ParameterType.Float:
                        transition.conditions[i].floatCriteria = (Condition.FloatCriteria)EditorGUILayout.EnumPopup(transition.conditions[i].floatCriteria);
                        transition.conditions[i].floatValueComparision = EditorGUILayout.FloatField(transition.conditions[i].floatValueComparision);
                        break;
                    case ParameterType.Trigger:
                        break;
                }

                EditorGUILayout.EndHorizontal();
            }
            else
            {
                Undo.RecordObject(target, "Deleted Condition");
                transition.DeleteCondition(transition.conditions[i]);
            }
        }
    }

    private void DrawConditionButton(int i, Transition transition)
    {
        EditorGUILayout.BeginHorizontal();

        string[] parametersName = new string[0];

        parametersName = new string[fsm.parameters.Count];

        for (int e = 0; e < parametersName.Length; e++)
        {
            if (fsm.parameters[e] != null)
                parametersName[e] = fsm.parameters[e].name;
        }

        parameterIndexes[i] = EditorGUILayout.Popup(parameterIndexes[i], parametersName);

        if (GUILayout.Button("Add Condition"))
        {
            Undo.RecordObject(target, "Condition Added");
            transition.AddCondition(fsm.parameters[parameterIndexes[i]].name);
        }

        if (GUILayout.Button("Delete All Conditions"))
        {
            transition.DeleteAll();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void SetExpanded()
    {
        if (fsm.anyStateTransitions != null)
        {
            transitionExpanded = new bool[fsm.anyStateTransitions.Count];
            parameterIndexes = new int[fsm.anyStateTransitions.Count];
        }
    }
}
