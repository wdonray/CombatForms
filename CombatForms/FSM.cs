using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Xml.Serialization;
namespace CombatForms
{
    [Serializable]
    public class FiniteStateMachine<T>
    {
        [XmlElement(ElementName = "CurrentState")]
        public State currentState;       
        private Dictionary<string, State> states;
        [XmlElement(ElementName = "StateList")]
        public List<State> stateList;
        [XmlElement(ElementName = "TransitionNames")]
        public List<string> transitionNames;
        private Dictionary<string, List<State>> transitions = new Dictionary<string, List<State>>();
        public FiniteStateMachine()
        {
            states = new Dictionary<string, State>(); //Ability to know when this happens            
            transitions = new Dictionary<string, List<State>>();
            transitionNames = new List<string>();
            stateList = new List<State>();
        }
   
        
        /// <summary>
        /// Creates a state and adds a key and value to it
        /// </summary>
        /// <param name="e"></param>
        public void AddState(Enum e)
        {
            State s = new State(e);            
            stateList.Add(s);
            states.Add(s.Name, s);
        }

        /// <summary>
        /// Creates a state and adds a key and value to it
        /// </summary>
        /// <param name="s">the state to add</param>
        public void AddState(State s)
        {
            stateList.Add(s);
            states.Add(s.Name, s);
        }
        public void RebuildFSM()
        {
            states = new Dictionary<string, State>();
            foreach(var s in stateList)
                states.Add(s.Name, s);
            for(int i = 0; i < transitionNames.Count; i++)
            {
                var twostates = transitionNames[i].Split('-');
                AddTransition(twostates[0], twostates[1]);
            }
        }
        /// <summary>
        /// Takes in two states as enums and adds them on to a list 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public bool AddTransition<V>(V from, V to)
        {
            State s1 = new State(from as Enum);
            State s2 = new State(to as Enum);
            List<State> tmp = new List<State>();
            tmp.Add(s1);
            tmp.Add(s2);
            string tname = tmp[0].Name + "-" + tmp[1].Name;            
            transitionNames.Add(tname);
            transitions.Add(tname, tmp);

            return true;
        }

        /// <summary>
        /// when we build the transition names we need to split the first half and second half
        /// in the program
        /// </summary>
        /// <param name="s1">string name of state from</param>
        /// <param name="s2">string name of state to</param>
        /// <returns></returns>
        public bool AddTransition(string s1, string s2)
        {
            State from = states[s1];
            State to = states[s2];
            List<State> tmp = new List<State>();
            tmp.Add(from);
            tmp.Add(to);
            string tname = tmp[0].Name + "-" + tmp[1].Name;          
            transitions.Add(tname, tmp);

            return true;
        }
        /// <summary>
        /// Takes in a state and sets it as the current state
        /// </summary>
        /// <param name="state"></param>
        public void Start<V>(V state)
        {            
            string key = (state as Enum).ToString();
            if (states.ContainsKey(key))
                currentState = states[key];
            else
                currentState = states.ElementAt(0).Value;
        }
        public void Start(State s)
        {            
            if(states.ContainsKey(s.Name))
                currentState = states[s.Name];
            
        }
        /// <summary>
        /// Sets the current state to the next state and invokes the exit and enter
        /// </summary>
        /// <param name="state"></param>
        public bool ChangeState<V>(V state)
        {
            string key = currentState.ToString() + "-" + (state as Enum).ToString();
            string newState = (state as Enum).ToString();
            if (transitions.ContainsKey(key) == false)
            {
                Debug.WriteLine("Invalid Transition " + key);
                return false;
            }
            if (currentState.onExit != null)          
                currentState.onExit.Invoke();           

            currentState = states[newState];

            if (currentState.onEnter != null)
                currentState.onEnter.Invoke();
            Debug.WriteLine("Valid Transition " + key);
            return true;
            
        }
        /// <summary>
        /// Returns a state with the name passed to it
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public State GetState<V>(V e)
        {
            return states[(e as Enum).ToString()];
        }
        /// <summary>
        /// Used to return the current states name
        /// </summary>
        /// <returns></returns>
        public State GetState()
        {
            return states[currentState.Name];
        }
    }
}
