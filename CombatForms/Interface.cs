using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatForms
{
    public interface IDamager
    {
        void DoDamage(IDamageable d);
    }
    public interface IDamageable
    {
        void TakeDamage(float f);
        bool isBlocking
        {
            get;
            set;
        }
    }

}
