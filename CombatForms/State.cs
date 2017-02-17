using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatForms
{
    [Serializable]
    class State
    {
        public delegate void Handler();
        public Handler onEnter;
        public Handler onExit;

        public State(Enum e)
        {
            onEnter = null;
            onExit = null;
            m_name = e.ToString();
        }
        private string m_name;
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
        public override string ToString() { return m_name; }
        public void AddEnter(Delegate d)
        {
            onEnter += d as Handler;
        }
        public void AddExit(Delegate d)
        {
            onExit += d as Handler;
        }
    }
}
