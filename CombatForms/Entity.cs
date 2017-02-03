using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatForms
{
    public class Entity : IDamageable, IDamager
    {
        public Entity (float h) { health = h; }
        public void DoDamage(IDamageable d)
        {
            Random rand = new Random();
            d.TakeDamage(rand.Next(10, 16));
        }
        public void TakeDamage(float f)
        {
            health -= f;
        }
        public float Health { get { return health; } }
        private float health;
    }
}
