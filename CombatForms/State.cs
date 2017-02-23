using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace CombatForms
{
    [Serializable]
    [XmlInclude(typeof(State))]     
    public class State
    {
        public State() { }
        public delegate void Handler();
        [XmlIgnore]
        public Handler onEnter;
        [XmlIgnore]
        public Handler onExit;
        public State(Enum e)
        {
            onEnter = null;
            onExit = null;
            Name = e.ToString();  
        }
        /// <summary>
        /// Used to represent the State name
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// Overriding the ToString to return a name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
