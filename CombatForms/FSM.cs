using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace CombatForms
{
    class FiniteStateMachine<T>
    {
        private State currentState;
        private Dictionary<string, State> states;
        private Dictionary<string, List<State>> transitions = new Dictionary<string, List<State>>();
        public FiniteStateMachine()
        {
            states = new Dictionary<string, State>(); //Ability to know when this happens
            transitions = new Dictionary<string, List<State>>();
            //var values = Enum.GetValues(typeof(T));
            //foreach(var v in values)
            //{
            //    State s = new State(v as Enum);
            //    states.Add(s.Name, s);
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void AddState(Enum e)
        {
            State s = new State(e);
            states.Add(s.Name, s);
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
            transitions.Add(tmp[0].Name + "->" + tmp[1].Name, tmp);
            return true;
        }
        /// <summary>
        /// Takes in a state and sets it as the current state
        /// </summary>
        /// <param name="state"></param>
        public void Start<V>(V state)
        {
            if (states.ContainsKey("INIT"))
                currentState = states["INIT"];
            else
                currentState = states.ElementAt(0).Value;
        }
        /// <summary>
        /// Sets the current state to the next state and invokes the exit and enter
        /// </summary>
        /// <param name="state"></param>
        public void ChangeState<V>(V state)
        {
            string key = currentState.ToString() + "->" + (state as Enum).ToString();
            string newState = (state as Enum).ToString();
            if (transitions.ContainsKey(key) == false)
            {
                Console.WriteLine("Invalid " + key);
                return;
            }
            if (currentState.onExit != null)
            {
                currentState.onExit.Invoke();
            }
            currentState = states[newState];
            if (currentState.onEnter != null)
            {
                currentState.onEnter.Invoke();
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public State GetState<V>(V e)
        {
            return states[(e as Enum).ToString()];
        }
        public State GetState()
        {

            return;
        }
    }
}
