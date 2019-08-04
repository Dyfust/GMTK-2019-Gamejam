using FSM.States;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class StateMachine
    {
        private State m_currentState = null;
        private State m_defaultState = null;
        private List<State> m_states = new List<State>();
        private List<Parameter> m_parameters = new List<Parameter>();

        public void UpdateFSM()
        {
            if (m_currentState != null)
            {
                m_currentState.OnStateUpdate();
                CheckTransitions();
            }
            else
            {
                Debug.LogWarning("This state machine contains no states!");
            }
        }

        private void CheckTransitions()
        {
            // Check transitions for the current state.
            if (m_currentState.m_transitions.Count < 1)
            { Debug.Log(string.Format("State: {0} contains no transitions!", m_currentState.GetName())); }

            for (int index = 0; index < m_currentState.m_transitions.Count; index++)
            {
                if (m_currentState.m_transitions[index].EvaluateCondition() == true)
                {
                    m_currentState.OnStateExit();
                    m_currentState = m_currentState.m_transitions[index].GetDestination();
                    m_currentState.OnStateEnter();
                    return;
                }
            }
        }

        public void AddState(State state)
        {
            m_states.Add(state);
        }

        public State GetState(string name)
        {
            for (int index = 0; index < m_states.Count; index++)
            {
                if (m_states[index].GetName() == name)
                { return m_states[index]; }
            }

            Debug.LogError(string.Format("{0} state could not be found!", name));
            return null;
        }

        public void SetDefaultState(State state)
        {
            m_defaultState = state;
            
            if (m_currentState != null)
            { m_currentState.OnStateExit(); }

            m_currentState = m_defaultState;
            m_currentState.OnStateEnter();
        }

        public void CreateParameter(string name)
        {
            Parameter parameter = new Parameter(name);
            m_parameters.Add(parameter);
        }

        public Parameter GetParameter(string name)
        {
            for (int index = 0; index < m_parameters.Count; index++)
            {
                if (m_parameters[index].m_name == name)
                { return m_parameters[index]; }
            }

            Debug.LogError(string.Format("Parameter with the name: {0} could not be found!", name));
            return null;
        }

        public bool GetBool(string name)
        {
            for (int index = 0; index < m_parameters.Count; index++)
            {
                if (m_parameters[index].m_name == name)
                { return m_parameters[index].m_value; }
            }

            Debug.LogError(string.Format("Parameter with the name: {0} could not be found!", name));

            return false;
        }

        public void SetBool(string name, bool value)
        {
            for (int index = 0; index < m_parameters.Count; index++)
            {
                if (m_parameters[index].m_name == name)
                {
                    m_parameters[index].m_value = value;
                    return;
                }
            }

            Debug.LogError(string.Format("Parameter with the name {0} could not be found!", name));
            return;
        }

        public State GetCurrentState()
        {
            return m_currentState;
        }
    }
}