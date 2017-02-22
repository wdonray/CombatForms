using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CombatForms
{
    [Serializable]
    public class Combat
    {
        private Combat()
        {
            cv = new CombatVariables();
        }
        private CombatVariables cv;
        public CombatVariables CV
        {
            get
            {
                return cv;
            }
            set
            {
                cv = value;
            }
        }
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
        
        /// <summary>
        /// Function to create a party, Party that contains 2 parties
        /// </summary>
        /// <param name="p"></param>
        public void AddToCombatParty(Party p)
        {
            if (CV.CombatParty.Count == 0)
            {
                CV.CombatParty.Add(p);
                CV.ActiveParty = CV.CombatParty[0];
            }
            else
            {
                CV.CombatParty.Add(p);
            }
            p.onPartyEnd += NextParty;
        }
        /// <summary>
        /// Add the selcted entity to enemy Party and the CombatPartyMembers List
        /// </summary>
        /// <param name="e"></param>
        public void AddToEnemyParty(Entity e)
        {
            CV.EnemyParty.AddPlayer(e);
            CV.CombatPartyMembers.Add(e);
        }
        public void Reset()
        {
            CV.CombatPartyMembers.Clear();
            CV.CombatParty.Clear();
            CV.ActiveParty.members.Clear();
            CV.CombatParty.Clear();
            CV.PlayerParty.members.Clear();
            CV.EnemyParty.members.Clear();
        }
        /// <summary>
        /// Add the selcted entity to player Party and the CombatPartyMembers List
        /// </summary>
        /// <param name="e"></param>
        public void AddToPlayerParty(Entity e)
        {
            CV.PlayerParty.AddPlayer(e);
            CV.CombatPartyMembers.Add(e);
        }
        /// <summary>
        /// Function to go to the next Party 
        /// </summary>
        public void NextParty()
        {
            int i = 0;
            foreach (Party p in CV.CombatParty)
            {
                if (p == CV.ActiveParty && i + 1 < CV.CombatParty.Count)
                {

                    CV.ActiveParty = CV.CombatParty[i + 1];
                    break;
                }
                else if (CV.ActiveParty == CV.CombatParty[i] && i + 1 >= CV.CombatParty.Count)
                {
                    CV.ActiveParty = CV.CombatParty[0];
                }
                i++;
            }
            //If the active player is dead call the GetNext function
            while (CV.ActiveParty.ActivePlayer.Alive == false)
            {
                CV.ActiveParty.SetNextPlayer();
            }
        }
    }
}

