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
        public Entity(float h, string n, bool a) { health = h; name = n; alive = a; }

        public delegate void OnEndTurn();
        [XmlIgnore]
        public OnEndTurn onEndTurn;

        public void DoDamage(IDamageable d)
        {
            Random rand = new Random();
            d.TakeDamage(rand.Next(10, 16));
        }
        public void TakeDamage(float f)
        {
            health -= f;
            if (health <= 0)
                alive = false;
        }
        public void EndTurn()
        {
            if (onEndTurn != null)
                onEndTurn.Invoke();
        }

        public float Health { get { return health; } }
        public float health;
        public string name;
        public string Name { get { return name; } set { name = value; } }
        public bool alive;
        public bool Alive { get { return alive; } }
    }
}
