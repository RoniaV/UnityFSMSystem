using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityFSM;

[CanEditMultipleObjects]
[CustomEditor(typeof(State), true)]
public class StateEditor : Editor
{
    State state;
    string transitionName;
    bool isExpanded = true;

    //Transition Settings
    bool[] transitionExpanded;
    bool[] conditionsExpanded;
    int[] parametersIndex;

    private void OnEnable()
    {
        state = (State)target;

        state.fSM = state.GetComponent<UFSM>();

        SetExpanded();
    }

    public override void OnInspectorGUI()
    {
        DrawState();

        DrawTransitions(state.transitions);
    }

    private void DrawState()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();
        transitionName = EditorGUILayout.TextField(transitionName);

        if (GUILayout.Button("Add Transition"))
        {
            if (transitionName != null)
            {
                Undo.RecordObject(target, "Transition Added");
                state.AddTransition(transitionName);

                transitionName = null;
            }
        }

        if(GUILayout.Button("Delete All Transitions"))
        {
            state.DeleteAllTransitions();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawTransitions(List<Transition> transitions)
    {
        isExpanded = EditorGUILayout.Foldout(isExpanded, "Transitions");
        EditorGUI.indentLevel++;

        if (transitionExpanded.Length != transitions.Count)
            SetExpanded();

        if (isExpanded)
        {
            for(int i = 0; i < transitions.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                transitionExpanded[i] = EditorGUILayout.Foldout(transitionExpanded[i], transitions[i].name);
                transitions[i].destination = (State)EditorGUILayout.ObjectField(transitions[i].destination, typeof(State), true);
                EditorGUILayout.EndHorizontal();

                if (transitionExpanded[i])
                {
                    EditorGUI.indentLevel++;

                    DrawConditions(transitions[i]);

                    DrawConditionButton(i);

                    EditorGUI.indentLevel--;
                }
            }
        }

        EditorGUI.indentLevel--;
    }

    private void DrawConditions(Transition transition)
    {
        for (int i = 0; i < transition.conditions.Count; i++)
        {
            Parameter parameterToDraw = state.fSM.GetParameter(transition.conditions[i].selectedParameterName);

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

    private void DrawConditionButton(int i)
    {
        EditorGUILayout.BeginHorizontal();

        string[] parametersName = new string[0];

        parametersName = new string[state.fSM.parameters.Count];

        for (int e = 0; e < parametersName.Length; e++)
        {
            if (state.fSM.parameters[e] != null)
                parametersName[e] = state.fSM.parameters[e].name;
        }

        parametersIndex[i] = EditorGUILayout.Popup(parametersIndex[i], parametersName);

        if (GUILayout.Button("Add Condition"))
        {
            Undo.RecordObject(target, "Condition Added");
            state.transitions[i].AddCondition(state.fSM.parameters[parametersIndex[i]].name);
        }

        if(GUILayout.Button("Delete All Conditions"))
        {
            state.transitions[i].DeleteAll();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void SetExpanded()
    {
        transitionExpanded = new bool[state.transitions.Count];
        conditionsExpanded = new bool[state.transitions.Count];
        parametersIndex = new int[state.transitions.Count];
    }
}
