using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatForms
{
    public class Combat
    {
        public Combat() { }

        public Party activeParty;
        public Party inactiveParty;

        public Party playerParty = new Party();
        public Party enemyParty = new Party();
        public string Space = "-------------------------------------------------------------";
        /// <summary>
        /// List of parties
        /// </summary>
        private List<Party> combatParty = new List<Party>();
        /// <summary>
        /// List of all the entities
        /// </summary>
        private List<Entity> combatPartyMembers = new List<Entity>();

        public string combatLog;
        public string entitiesList;

        private static Combat instance = null;
        public static Combat Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Combat();
                }
                return instance;
            }
        }
        public Entity activePlaya
        {
            get
            {
                return activeParty.activePlayer;
            }
        }
        public List<Party> CombatParty
        {
            get
            {
                return combatParty;
            }
        }
        public List<Entity> CombatPartyMembers
        {
            get
            {
                return combatPartyMembers;
            }
        }
        /// <summary>
        /// Function to create a party, Party that contains 2 parties
        /// </summary>
        /// <param name="p"></param>
        public void AddToCombatParty(Party p)
        {
            if (combatParty.Count == 0)
            {
                combatParty.Add(p);
                activeParty = combatParty[0];
            }
            else
            {
                combatParty.Add(p);
            }
            p.onPartyEnd += NextParty;
        }
        /// <summary>
        /// Add the selcted entity to enemy Party and the CombatPartyMembers List
        /// </summary>
        /// <param name="e"></param>
        public void AddEnemyParty(Entity e)
        {
            enemyParty.AddPlayer(e);
            combatPartyMembers.Add(e);
        }
        /// <summary>
        /// Add the selcted entity to player Party and the CombatPartyMembers List
        /// </summary>
        /// <param name="e"></param>
        public void AddPlayerParty(Entity e)
        {
            playerParty.AddPlayer(e);
            combatPartyMembers.Add(e);
        }
        /// <summary>
        /// Function to go to the next Party 
        /// </summary>
        public void NextParty()
        {
            int i = 0;
            foreach (Party p in combatParty)
            {
                if (p == activeParty && i + 1 < combatParty.Count)
                {
                    inactiveParty = combatParty[i];
                    activeParty = combatParty[i + 1];
                    break;
                }
                else if (activeParty == combatParty[i] && i + 1 >= combatParty.Count)
                {
                    activeParty = combatParty[0];
                    inactiveParty = combatParty[1];
                }
                i++;
            }

            //If the active player is dead call the GetNext function
            while (activeParty.activePlayer.Alive == false)
            {
                activeParty.GetNext();
            }
        }
    }
}

