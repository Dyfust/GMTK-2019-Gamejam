using System.Collections.Generic;
using UnityEngine;

namespace FSM.States
{
    public abstract class State
    {
        protected string m_name = "";
        public List<Transition> m_transitions = new List<Transition>();

        public void AddTransition(Transition transition)
        {
            m_transitions.Add(transition);
        }

        public abstract void OnStateEnter();
        public abstract void OnStateUpdate();
        public abstract void OnStateExit();

        public string GetName()
        {
            return m_name;
        }
    }

    public class BlankState : State
    {
        public BlankState(string name)
        {
            m_name = name;
        }

        public override void OnStateEnter()
        {
            Debug.Log(string.Format("Entering {0} state!", m_name));
        }

        public override void OnStateExit()
        {
            Debug.Log(string.Format("Exiting {0} state!", m_name));
        }

        public override void OnStateUpdate()
        {

        }
    }

}