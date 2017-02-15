using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace CombatForms
{
    public class Party
    {
        public Party() { }
        public Entity activePlaya;
        private List<Entity> players = new List<Entity>();
        public delegate void OnPartyEnd();
        public OnPartyEnd onPartyEnd;
        public List<Entity> members
        {
            get
            {
                return players;
            }
        }
        /// <summary>
        /// Function to set the next player in the list to be the active player
        /// </summary>
        int currentID = 0;
        public void GetNext()
        {
            if (currentID >= players.Count - 1)
            {
                currentID = 0;
                activePlaya = players[currentID];
                if (onPartyEnd != null)
                    onPartyEnd.Invoke();
                return;
            }
            currentID++;
            activePlaya = players[currentID];    
        }
        /// <summary>
        /// Bool to check if you can go to the next player
        /// </summary>
        /// <returns></returns>
        public bool CanNextActivePlaya()
        {
            int i = 0;
            foreach (Entity p in players)
            {
                if (i == players.Count - 1)
                {
                    activePlaya = players[0];
                    return false;
                }
                else if (p == activePlaya)
                {
                    return true;
                }
                i++;
            }
            return false;
        }
        /// <summary>
        /// Function to be able to create a player and add it to a party
        /// </summary>
        /// <param name="p"></param>
        /// <param name="party"></param>
        public void AddPlayer(Entity p)
        {
            if (players.Count <= currentID)
            {
                players.Add(p);

                p.onEndTurn += GetNext;
                return;
            }
            players.Add(p);
            p.onEndTurn += GetNext;
            Sort();
        }
        public void Sort()
        {
            players.Sort((x, y) => -1 * x.Speed.CompareTo(y.Speed));
            activePlaya = players[currentID];
        }
    }
}

