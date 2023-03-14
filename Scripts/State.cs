using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFSM;

[RequireComponent(typeof(UFSM))]
public class State : MonoBehaviour
{
    [HideInInspector]
    public UFSM fSM;
    [HideInInspector]
    public List<Transition> transitions = new List<Transition>();

    public delegate void StateDelegate();
    public StateDelegate OnEnterDelegate { get; set; } = null;
    public StateDelegate OnFixedUpdateDelegate { get; set; } = null;
    public StateDelegate OnUpdateDelegate { get; set; } = null;
    public StateDelegate OnExitDelegate { get; set; } = null;

    
    protected virtual void Awake()
    {
        if (fSM == null)
            fSM = GetComponent<UFSM>();
    }

    protected virtual void OnEnable()
    {
        OnEnterDelegate?.Invoke();
    }

    protected virtual void FixedUpdate()
    {
        OnFixedUpdateDelegate?.Invoke();
    }

    protected virtual void Update()
    {
        OnUpdateDelegate?.Invoke();
    }

    protected virtual void OnDisable()
    {
        OnExitDelegate?.Invoke();
    }

    public void AddTransition(string name)
    {
        transitions.Add(new Transition(name));
    }

    public void DeleteTransition(Transition transition)
    {
        transitions.Remove(transition);

        transition = null;
    }

    public void DeleteAllTransitions()
    {
        for(int i = 0; i < transitions.Count; i++)
        {
            transitions[i] = null;
        }

        transitions.Clear();
    }
}
