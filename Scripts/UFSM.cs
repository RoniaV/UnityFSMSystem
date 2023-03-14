using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityFSM;

public class UFSM : MonoBehaviour
{
    [HideInInspector]
    public List<Parameter> parameters = new List<Parameter>();
    [HideInInspector] 
    public List<Transition> anyStateTransitions = new List<Transition>();

    [SerializeField] State initialState;

    protected Dictionary<int, State> m_states;
    protected State m_currentState;
    //private IEnumerator triggerCoroutine;

    protected virtual void Start()
    {
        m_states = new Dictionary<int, State>();

        SetCurrentState(initialState);
    }

    void Update()
    {
        if (GetParameter("Shot") != null)
            Debug.Log("Shot value: " + GetParameter("Shot").triggerValue);
    }

    protected virtual void LateUpdate()
    {
        if (!CheckTransitions(anyStateTransitions))
            CheckTransitions(m_currentState.transitions);
    }

    //protected virtual void OnEnable()
    //{
    //    triggerCoroutine = TriggerCoroutine();

    //    StartCoroutine(triggerCoroutine);
    //}

    //protected virtual void OnDisable()
    //{
    //    StopCoroutine(triggerCoroutine);
    //}

    public void AddState(int key, State state)
    {
        m_states.Add(key, state);
    }

    public State GetState(int key)
    {
        return m_states[key];
    }

    public void SetCurrentState(State state)
    {
        if (m_currentState != null)
        {
            m_currentState.enabled = false;
        }

        m_currentState = state;

        if (m_currentState != null)
        {
            m_currentState.enabled = true;
        }
    }

    public void AddParameter(ParameterType type, string name)
    {
        parameters.Add(new Parameter(type, name));
    }

    public void DeleteParameter(Parameter parameter)
    {
        parameters.Remove(parameter);

        parameter = null;
    }

    public void DeleteAllParameters()
    {
        parameters.Clear();
    }

    public Parameter GetParameter(string name)
    {
        Parameter parameterToReturn = null;

        for(int i = 0; i < parameters.Count; i++)
        {
            if(parameters[i].name == name)
            {
                parameterToReturn = parameters[i];
                break;
            }
        }

        return parameterToReturn;
    }

    public void SetBoolParameter(string name, bool value)
    {
        for(int i = 0; i < parameters.Count; i++)
        {
            if (parameters[i]?.name == name && parameters[i]?.type == ParameterType.Bool)
                parameters[i].boolValue = value;
        }
    }

    public void SetIntParameter(string name, int value)
    {
        for (int i = 0; i < parameters.Count; i++)
        {
            if (parameters[i]?.name == name && parameters[i]?.type == ParameterType.Int)
                parameters[i].intValue = value;
        }
    }

    public void SetFloatParameter(string name, float value)
    {
        for (int i = 0; i < parameters.Count; i++)
        {
            if (parameters[i]?.name == name && parameters[i]?.type == ParameterType.Float)
                parameters[i].floatValue = value;
        }
    }

    public void SetTriggerParameter(string name)
    {
        for (int i = 0; i < parameters.Count; i++)
        {
            if (parameters[i]?.name == name && parameters[i]?.type == ParameterType.Trigger)
                parameters[i].triggerValue = true;
        }
    }

    public void DeleteAllAnyStateTransitions()
    {
        for (int i = 0; i < anyStateTransitions.Count; i++)
        {
            anyStateTransitions[i] = null;
        }

        anyStateTransitions.Clear();
    }

    protected bool CheckTransitions(List<Transition> transitions)
    {
        for (int i = 0; i < transitions.Count; i++)
        {
            if (transitions[i].CheckTransition(this))
            {
                SetCurrentState(transitions[i].destination);
                return true;
            }
        }
        return false;
    }
}
