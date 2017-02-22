using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace CombatForms
{
    [Serializable]  
    public class Party
    {
        public Party() { }
        public Party(List<Entity> entities)
        {
            players = entities;
        }
        public int currentID = 0;
        [XmlElement(ElementName = "ActivePlayer")]
        public Entity ActivePlayer;
        
        private List<Entity> players = new List<Entity>();

        public delegate void OnPartyEnd();
        [XmlIgnore]
        public OnPartyEnd onPartyEnd;
        [XmlElement(ElementName = "Members")]
        public List<Entity> members
        {
            get
            {
                return players;
            }
            set
            {
                players = value;
            }
        }
        /// <summary>
        /// Function to set the next player in the list to be the active player
        /// </summary>
        public void SetNextPlayer()
        {
            if (currentID >= players.Count - 1)
            {
                currentID = 0;
                ActivePlayer = players[currentID];
                if (onPartyEnd != null)
                    onPartyEnd.Invoke();
                return;
            }
            currentID++;
            ActivePlayer = players[currentID];
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
                ActivePlayer = players[currentID];
                p.onEndTurn += SetNextPlayer;
                return;
            }
            players.Add(p);
            p.onEndTurn += SetNextPlayer;
            Sort();
        }
        /// <summary>
        /// Sorts the player by speed
        /// </summary>
        public void Sort()
        {
            players.Sort((x, y) => -1 * x.Speed.CompareTo(y.Speed));
        }
    }
}

