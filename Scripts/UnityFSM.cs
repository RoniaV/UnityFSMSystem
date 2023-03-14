using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityFSM
{
    public class Value<T>
    {
        public T value;
    }

    public enum ParameterType 
    {
        Int,
        Float,
        Bool,
        Trigger
    };

    [Serializable]
    public class Parameter //: ScriptableObject
    {
        public string name;
        public ParameterType type;

        public bool boolValue;
        public int intValue;
        public float floatValue;
        public bool triggerValue 
        { 
            get
            {
                if (tValue)
                {
                    tValue = false;
                    return true;
                }
                else
                    return false;
            }
            set { tValue = value; }
        }
        private bool tValue;

        public Parameter(ParameterType type, string name)
        {
            this.type = type;
            this.name = name;
        }

        public void SetValue(bool v)
        {
            boolValue = v;
        }

        public void SetValue(int v)
        {
            intValue = v;
        }

        public void SetValue(float v)
        {
            floatValue = v;
        }

        public void SetValue()
        {
            triggerValue = !triggerValue;
        }
    }

    [Serializable]
    public class Transition //: ScriptableObject
    {
        public string name;
        public State destination;

        public List<Condition> conditions = new List<Condition>();

        public Transition(string name)
        {
            this.name = name;
        }

        public bool CheckTransition(UFSM fsm)
        {
            bool result = false;

            for(int i = 0; i < conditions.Count; i++)
            {
                result = conditions[i].IsTrue(fsm);

                if (result == false)
                    break;
            }

            return result;
        }

        public void AddCondition(string parameterName)
        {
            conditions.Add(new Condition(parameterName));
        }

        public void DeleteCondition(Condition condition)
        {
            conditions.Remove(condition);
        }

        public void DeleteAll()
        {
            conditions.Clear();
        }
    }

    [Serializable]
    public class Condition //: ScriptableObject
    {
        public string selectedParameterName;

        public Condition(string parameterName)
        {
            selectedParameterName = parameterName;
        }

        public enum BoolCriteria
        {
            IsTrue,
            IsFalse
        }
        public BoolCriteria boolCriteria;

        public enum IntCriteria
        {
            Equal,
            LessThan,
            MoreThan
        }
        public IntCriteria intCriteria;
        public int intValueComparision;

        public enum FloatCriteria
        {
            Equal,
            LessThan,
            MoreThan
        }
        public FloatCriteria floatCriteria;
        public float floatValueComparision;

        public bool IsTrue(UFSM fsm)
        {
            bool result = false;

            Parameter selectedParameter = null;

            for(int i = 0; i < fsm.parameters.Count; i++)
            {
                if (fsm.parameters[i].name == selectedParameterName)
                {
                    selectedParameter = fsm.parameters[i];
                    break;
                }
            }

            switch (selectedParameter?.type)
            {
                #region BOOL
                case ParameterType.Bool:
                    //Debug.Log(selectedParameter.boolValue);
                    switch(boolCriteria)
                    {
                        case BoolCriteria.IsTrue:
                            if (selectedParameter.boolValue)
                                result = true;
                            break;
                        case BoolCriteria.IsFalse:
                            if (!selectedParameter.boolValue)
                                result = true;
                            break;
                        default:
                            result = false;
                            break;
                    }
                    break;
                #endregion
                #region INT
                case ParameterType.Int:
                    //Debug.Log(selectedParameter.intValue);
                    switch (intCriteria)
                    {
                        case IntCriteria.Equal:
                            if (selectedParameter.intValue == intValueComparision)
                                result = true;
                            break;
                        case IntCriteria.LessThan:
                            if (selectedParameter.intValue < intValueComparision)
                                result = true;
                            break;
                        case IntCriteria.MoreThan:
                            if (selectedParameter.intValue > intValueComparision)
                                result = true;
                            break;
                        default:
                            result = false;
                            break;
                    }
                    break;
                    #endregion
                #region FLOAT
                case ParameterType.Float:
                    //Debug.Log(selectedParameter.floatValue);
                    switch(floatCriteria)
                    {
                        case FloatCriteria.Equal:
                            if (selectedParameter.floatValue == floatValueComparision)
                                result = true;
                            break;
                        case FloatCriteria.LessThan:
                            if (selectedParameter.floatValue < floatValueComparision)
                                result = true;
                            break;
                        case FloatCriteria.MoreThan:
                            if (selectedParameter.floatValue > floatValueComparision)
                                result = true;
                            break;
                        default:
                            result = false;
                            break;
                    }
                    break;
                #endregion
                #region TRIGGER
                case ParameterType.Trigger:
                    //Debug.Log(selectedParameter.triggerValue);
                    if (selectedParameter.triggerValue)
                        result = true;
                    break;
                #endregion
                default:
                    result = false;
                    break;
            }

            return result;
        }
    }
}
