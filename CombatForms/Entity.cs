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

        public void DoDamage(IDamageable d)
        {
            Random rand = new Random();
            d.TakeDamage(rand.Next(10, 16));
        }
        public void TakeDamage(float f)
        {
            m_Health -= f;
            if (m_Health <= 0)
                m_Alive = false;
        }
        public void EndTurn()
        {
            if (onEndTurn != null)
                onEndTurn.Invoke();
        }

        public float m_Health;
        public string m_Name;
        public bool m_Alive;
        public float m_Speed;
        public bool m_Block;
        public delegate void Handler();
        public Handler onEndTurn;
        public bool Alive { get { return m_Alive; } }
        public bool Block { get { return m_Block; } }
        public float Health { get { return m_Health; } }
        public string Name { get { return m_Name; } set { m_Name = value; } }
        public float Speed { get { return m_Speed; } }
    }
}
