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
        public Entity(float health, string name, bool alive, bool block, float speed, EType e)
        {
            m_Health = health;
            m_Name = name;
            m_Alive = alive;
            m_Speed = speed;
            m_Block = block;
            eType = e;
        }
        public string Space = "-------------------------------------------------------------";
        /// <summary>
        /// Does damage based on if the selected target is blocking or not and adds log to combatLog
        /// </summary>
        /// <param name="d"></param>
        public void DoDamage(IDamageable d)
        {
            Random rand = new Random();
            Random crit = new Random();
            float damage = rand.Next(10, 16);
            if (d.isBlocking == false)
            {
                float critChance = crit.Next(1, 101);
                //Added a crit chance of 15%
                if (critChance <= 15)
                {
                    damage = damage * 2;
                    d.TakeDamage(damage);
                    Combat.Instance.combatLog += this.Name + " is attacking and CRIT "
                         + (d as Entity).Name + " for " + damage.ToString() + " damage!" + Environment.NewLine + Space + Environment.NewLine;
                }

                else
                {
                    d.TakeDamage(damage);
                    Combat.Instance.combatLog += this.Name + " is attacking "
                        + (d as Entity).Name + " for " + damage.ToString() + " damage!" + Environment.NewLine + Space + Environment.NewLine;
                }
            }
            else if (d.isBlocking == true)
            {
                damage = damage / 2;
                d.TakeDamage(damage);
                Combat.Instance.combatLog += this.Name + " is attacking "
                   + (d as Entity).Name + "(Blocked half the damage)" + " for " + damage.ToString() + " damage!" + Environment.NewLine + Space + Environment.NewLine;
                d.isBlocking = false;
            }
        }
        /// <summary>
        /// Sets a selected entity to set damage
        /// </summary>
        /// <param name="f"></param>
        public void TakeDamage(float f)
        {
            m_Health -= f;
            if (m_Health <= 0)
            {
                m_Alive = false;
                m_Health = 0;
                Combat.Instance.combatLog += this.Name + " has died" + Environment.NewLine + Space + Environment.NewLine;
            }
        }
        /// <summary>
        /// Invokes the end turn delegate
        /// </summary>
        public void EndTurn()
        {
            if (onEndTurn != null)
                onEndTurn.Invoke();
        }
        /// <summary>
        /// Checks the current eType and attacks a random opposing target
        /// </summary>
        public void Attack()
        {
            Random random = new Random();
            int targetP = random.Next(0, (Combat.Instance.playerParty.members.Count));
            int targetE = random.Next(0, (Combat.Instance.enemyParty.members.Count));
            if (this.eType == EType.ENEMY && Combat.Instance.activeParty.activePlayer.Alive == true)
            {
                do
                {
                    targetP = random.Next(0, (Combat.Instance.playerParty.members.Count));
                } while (Combat.Instance.playerParty.members[targetP].Alive == false);
                DoDamage(Combat.Instance.playerParty.members[targetP]);
            }
            else if (this.eType == EType.PLAYER && Combat.Instance.activeParty.activePlayer.Alive == true)
            {
                do
                {
                    targetE = random.Next(0, (Combat.Instance.enemyParty.members.Count));
                } while (Combat.Instance.enemyParty.members[targetE].Alive == false);
                DoDamage(Combat.Instance.enemyParty.members[targetE]);
            }
        }
        /// <summary>
        /// Sets the current player to take its remaining HP as damage
        /// </summary>
        public void Flee()
        {
            Combat.Instance.activeParty.activePlayer.TakeDamage(Combat.Instance.activeParty.activePlayer.Health);
        }
        /// <summary>
        /// Sets the current player boolean (isBlocking) to true and adds text to the combatLog String
        /// </summary>
        public void Defend()
        {
            Combat.Instance.activeParty.activePlayer.isBlocking = true;
            Combat.Instance.combatLog += this.Name + " prepared a block! " + Environment.NewLine + Space + Environment.NewLine;
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
