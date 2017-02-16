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

        int currentID = 0;

        public Entity activePlayer;
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
        public void GetNext()
        {
            if (currentID >= players.Count - 1)
            {
                currentID = 0;
                activePlayer = players[currentID];
                if (onPartyEnd != null)
                    onPartyEnd.Invoke();
                return;
            }
            currentID++;
            activePlayer = players[currentID];
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
                activePlayer = players[currentID];
                p.onEndTurn += GetNext;
                return;
            }
            players.Add(p);
            p.onEndTurn += GetNext;
            Sort();
        }
        /// <summary>
        /// Sorts the player by speed
        /// </summary>
        public void Sort()
        {
            players.Sort((x, y) => -1 * x.Speed.CompareTo(y.Speed));
            activePlayer = players[currentID];
        }
    }
}

