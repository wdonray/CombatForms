using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace CombatForms
{
    public class Entity : IDamageable, IDamager
    {
        public enum EType
        {
            PLAYER,
            ENEMY,
        }
        public EType eType;
        public Entity() { }
        public Entity(float h, string n, bool a, bool b, float s, EType e)
        {
            m_Health = h;
            m_Name = n;
            m_Alive = a;
            m_Speed = s;
            m_Block = b;
            eType = e;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        public void DoDamage(IDamageable d)
        {
            Random rand = new Random();
            Random rBlock = new Random();
            //float f = (float)(rBlock.NextDouble() * (6d - 1d) + 1d);
            if (d.isBlocking == false)
            {
                int damage = rand.Next(10, 16);
                d.TakeDamage(damage);
                Combat.Instance.combatLog += this.Name + " is attacking "
                    + (d as Entity).Name + " for " + damage.ToString() + " damage!" + Environment.NewLine;
            }
            else if (d.isBlocking == true)
            {
                float damageB = rand.Next(5, 10);
                d.TakeDamage(damageB);
                Combat.Instance.combatLog += this.Name + " is attacking "
                   + (d as Entity).Name + "(Blocked some damage)" + " for " + damageB.ToString() + " damage!" + Environment.NewLine;
                d.isBlocking = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        public void TakeDamage(float f)
        {
            m_Health -= f;
            if (m_Health <= 0)
                m_Alive = false;
        }
        /// <summary>
        /// 
        /// </summary>
        public void EndTurn()
        {
            if (onEndTurn != null)
                onEndTurn.Invoke();
            //Combat.Instance.combatLog += ("Active Player: " + this.Name + Environment.NewLine + "\n");
        }
        /// <summary>
        /// 
        /// </summary>
        public void Attack()
        {
            Random random = new Random();
            int target = random.Next(0, (Combat.Instance.CombatPartyMembers.Count / 2));
            if (this.eType == EType.ENEMY)
            {
                if (Combat.Instance.playerParty.members[target].Alive == true)
                    DoDamage(Combat.Instance.playerParty.members[target]);
                else
                {
                    target = random.Next(0, (Combat.Instance.CombatPartyMembers.Count / 2));
                    DoDamage(Combat.Instance.playerParty.members[target]);
                }

            }
            else if (this.eType == EType.PLAYER)
            {
                if (Combat.Instance.enemyParty.members[target].Alive == true)
                    DoDamage(Combat.Instance.enemyParty.members[target]);
                else
                {
                    target = random.Next(0, (Combat.Instance.CombatPartyMembers.Count / 2));
                    DoDamage(Combat.Instance.enemyParty.members[target]);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Kill()
        {
            if (Combat.Instance.activeParty.activePlaya.Health <= 100)
                Combat.Instance.activeParty.activePlaya.TakeDamage(Combat.Instance.activeParty.activePlaya.Health);
            Combat.Instance.activeParty.activePlaya.Alive = false;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Flee()
        {
            if (Combat.Instance.activeParty.activePlaya.Name == "Cloud")
                Kill();
            else if (Combat.Instance.activeParty.activePlaya.Name == "Aeris the Archer")
                Kill();
            else if (Combat.Instance.activeParty.activePlaya.Name == "Dwarf Soldier")
                Kill();
            else if (Combat.Instance.activeParty.activePlaya.Name == "Dwarf Archer")
                Kill();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Defend()
        {
            Combat.Instance.activeParty.activePlaya.isBlocking = true;
            Combat.Instance.combatLog += this.Name + " prepared a block! " + Environment.NewLine;
        }

        private float m_Health;
        private string m_Name;
        private bool m_Alive;
        private float m_Speed;
        private bool m_Block;
        public delegate void Handler();
        public Handler onEndTurn;
        public bool Alive { get { return m_Alive; } set { m_Alive = value; } }
        public bool isBlocking { get { return m_Block; } set { m_Block = value; } }
        public float Health { get { return m_Health; } set { m_Health = value; } }
        public string Name { get { return m_Name; } set { m_Name = value; } }
        public float Speed { get { return m_Speed; } }
    }
}
