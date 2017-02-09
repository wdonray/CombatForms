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
        /// <summary>
        /// Function to create a party
        /// </summary>
        /// <param name="p"></param>
        public void AddParty(Party p)
        {
            if (party.Count == 0)
            {
                party.Add(p);
                activeParty = party[0];
            }
            else
            {
                party.Add(p);
            }
            p.onPartyEnd += NextParty;
        }

        public void AddPlaya(Entity a, int p)
        {
            party[p - 1].AddPlayer(a);
        }

        /// <summary>
        /// Function to go to the next Party
        /// </summary>
        public void NextParty()
        {
            int i = 0;
            foreach (Party p in party)
            {
                if (p == activeParty && i + 1 < party.Count)
                {
                    activeParty = party[i + 1];
                    break;
                }
                else if (activeParty == party[i] && i + 1 >= party.Count)
                {
                    activeParty = party[0];
                }
                i++;
            }
        }
        private List<Party> party = new List<Party>();
    }
}

