using FSM.States;

namespace FSM
{
    public sealed class Transition
    {
        private State m_destination = null;
        private Parameter m_parameter = null;
        private bool m_condition = false;

        public Transition(State destination, Parameter parameter, bool condition)
        {
            m_destination = destination;
            m_parameter = parameter;
            m_condition = condition;            
        }

        public bool EvaluateCondition()
        {
            return (m_parameter.m_value == m_condition);
        }

        public State GetDestination()
        {
            return m_destination;
        }
    }
}