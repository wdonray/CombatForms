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
        public Entity() { }
        public Entity(float h, string n, bool a, bool b, float s)
        {
            m_Health = h;
            m_Name = n;
            m_Alive = a;
            m_Speed = s;
            m_Block = b;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        public void DoDamage(IDamageable d)
        {
            Random rand = new Random();
            d.TakeDamage(rand.Next(10, 16));
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
        }
        /// <summary>
        /// 
        /// </summary>
        public void Attack()
        {
            Random rBlock = new Random();
            float f = (float)(rBlock.NextDouble() * (6d - 1d) + 1d);
            if (GameManager.Instance.player3.Alive == true &&
                GameManager.Instance.player1.Alive == true
                && Combat.Instance.activeParty.activePlaya.Name == "Cloud")
            {
                if (GameManager.Instance.player3.Block == false)
                    GameManager.Instance.player1.DoDamage(GameManager.Instance.player3);
                else
                {
                    GameManager.Instance.player1.DoDamage(GameManager.Instance.player3);
                    GameManager.Instance.player3.Health += f;
                    GameManager.Instance.player3.Block = false;
                }
            }
            else if (GameManager.Instance.player3.Alive == false &&
                GameManager.Instance.player1.Alive == true
                && Combat.Instance.activeParty.activePlaya.Name == "Cloud")
            {
                if (GameManager.Instance.player4.Block == false)
                    GameManager.Instance.player1.DoDamage(GameManager.Instance.player4);
                else
                {
                    GameManager.Instance.player1.DoDamage(GameManager.Instance.player4);
                    GameManager.Instance.player4.Health += f;
                    GameManager.Instance.player4.Block = false;
                }
            }
            else if (GameManager.Instance.player2.Alive == true &&
                Combat.Instance.activeParty.activePlaya.Name == "Aeris the Archer")
            {
                if (GameManager.Instance.player4.Alive == true && GameManager.Instance.player4.Block == false)
                    GameManager.Instance.player2.DoDamage(GameManager.Instance.player4);
                else if (GameManager.Instance.player4.Alive == true && GameManager.Instance.player4.Block == true)
                {
                    GameManager.Instance.player2.DoDamage(GameManager.Instance.player4);
                    GameManager.Instance.player4.Health += f;
                    GameManager.Instance.player4.Block = false;
                }
                if (GameManager.Instance.player3.Alive == true && GameManager.Instance.player3.Block == false)
                    GameManager.Instance.player2.DoDamage(GameManager.Instance.player3);
                else if (GameManager.Instance.player3.Alive == true && GameManager.Instance.player3.Block == true)
                {
                    GameManager.Instance.player2.DoDamage(GameManager.Instance.player3);
                    GameManager.Instance.player3.Health += f;
                    GameManager.Instance.player3.Block = false;
                }
            }
            else if (GameManager.Instance.player1.Alive == true &&
                GameManager.Instance.player3.Alive == true &&
                Combat.Instance.activeParty.activePlaya.Name == "Dwarf Soldier")
            {
                if (GameManager.Instance.player1.Block == false)
                    GameManager.Instance.player3.DoDamage(GameManager.Instance.player1);
                else
                {
                    GameManager.Instance.player3.DoDamage(GameManager.Instance.player1);
                    GameManager.Instance.player1.Health += f;
                    GameManager.Instance.player1.Block = false;
                }
            }
            else if (GameManager.Instance.player1.Alive == true &&
                GameManager.Instance.player3.Alive == true &&
                Combat.Instance.activeParty.activePlaya.Name == "Dwarf Soldier")
            {
                if (GameManager.Instance.player2.Block == false)
                    GameManager.Instance.player3.DoDamage(GameManager.Instance.player2);
                else
                {
                    GameManager.Instance.player3.DoDamage(GameManager.Instance.player2);
                    GameManager.Instance.player2.Health += f;
                    GameManager.Instance.player2.Block = false;
                }
            }
            else if (GameManager.Instance.player4.Alive == true &&
                  Combat.Instance.activeParty.activePlaya.Name == "Dwarf Archer")
            {
                if (GameManager.Instance.player2.Alive == true && GameManager.Instance.player2.Block == false)
                    GameManager.Instance.player4.DoDamage(GameManager.Instance.player2);
                else if (GameManager.Instance.player2.Alive == true && GameManager.Instance.player2.Block == true)
                {
                    GameManager.Instance.player4.DoDamage(GameManager.Instance.player2);
                    GameManager.Instance.player2.Health += f;
                    GameManager.Instance.player2.Block = false;
                }
                if (GameManager.Instance.player1.Alive == true && GameManager.Instance.player1.Block == false)
                    GameManager.Instance.player4.DoDamage(GameManager.Instance.player1);
                else if (GameManager.Instance.player2.Alive == true && GameManager.Instance.player1.Block == false)
                {
                    GameManager.Instance.player4.DoDamage(GameManager.Instance.player1);
                    GameManager.Instance.player1.Health += f;
                    GameManager.Instance.player1.Block = false;
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
            Combat.Instance.activeParty.activePlaya.Block = true;
        }

        private float m_Health;
        private string m_Name;
        private bool m_Alive;
        private float m_Speed;
        private bool m_Block;
        public delegate void Handler();
        public Handler onEndTurn;
        public bool Alive { get { return m_Alive; } set { m_Alive = value; } }
        public bool Block { get { return m_Block; } set { m_Block = value; } }
        public float Health { get { return m_Health; } set { m_Health = value; } }
        public string Name { get { return m_Name; } set { m_Name = value; } }
        public float Speed { get { return m_Speed; } }
    }
}
