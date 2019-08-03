namespace FSM
{
    public sealed class Parameter
    {
        public string m_name = "";
        public bool m_value = false;

        public Parameter(string name)
        {
            m_name = name;
            m_value = false;
        }
    }
}