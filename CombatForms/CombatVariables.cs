using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatForms
{
    public class CombatVariables
    {
        private List<Party> combatParty;
        private List<Entity> combatPartyMembers;
        public CombatVariables()
        {
            combatParty = new List<Party>();
            combatPartyMembers = new List<Entity>();
        }
        public Party ActiveParty { get; set; }
        public Party PlayerParty = new Party();
        public Party EnemyParty = new Party();

        public Entity ActivePlayer
        {
            get
            {
                return ActiveParty.ActivePlayer;
            }
            set
            {
                ActiveParty.ActivePlayer = value;
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
    }
}
