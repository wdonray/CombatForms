using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatForms
{
    class GameManager
    {
        public Entity player1, player2, player3, player4;
        //new up when we access the property Instance
        private static GameManager instance = null;
        //private bc we dont want someone to new up this instance
        private GameManager() { }
        //this is actually how we access it
        //how u use it Singleton.Instance."variables"
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameManager();
                }
                return instance;
            }
        }
    }
}
