using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CombatForms
{
    public enum GameStart
    {
        INIT = 0,
        ATTACK = 1,
        REST = 2,
        DEFEND = 3,
        FLEE = 4,
    }
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Party a = new Party();
            Party b = new Party();
            Entity cloud = new Entity(100, "Cloud", true, false, 1);
            Entity aeris = new Entity(50, "Aeris the Archer", true, false, 4);
            Entity entitySoldier = new Entity(100, "Dwarf Soldier", true, false, 2);
            Entity entityArcher = new Entity(50, "Dwarf Archer", true, false, 3);

            GameManager.Instance.player1 = cloud;
            GameManager.Instance.player2 = aeris;
            GameManager.Instance.player3 = entitySoldier;
            GameManager.Instance.player4 = entityArcher;
            GameManager.Instance.currentPlayer = cloud;

            Combat.Instance.AddParty(a);
            Combat.Instance.AddParty(b);
            Combat.Instance.AddPlaya(cloud, 1);
            Combat.Instance.AddPlaya(aeris, 1);
            Combat.Instance.AddPlaya(entitySoldier, 2);
            Combat.Instance.AddPlaya(entityArcher, 2);
            Combat.Instance.activeParty.Sort();

            FiniteStateMachine<GameStart>.Instance.AddState(GameStart.INIT);
            FiniteStateMachine<GameStart>.Instance.AddState(GameStart.ATTACK);
            FiniteStateMachine<GameStart>.Instance.AddState(GameStart.REST);
            FiniteStateMachine<GameStart>.Instance.AddState(GameStart.DEFEND);
            FiniteStateMachine<GameStart>.Instance.AddState(GameStart.FLEE);

            FiniteStateMachine<GameStart>.Instance.AddTransition(GameStart.INIT, GameStart.ATTACK);
            FiniteStateMachine<GameStart>.Instance.AddTransition(GameStart.INIT, GameStart.DEFEND);
            FiniteStateMachine<GameStart>.Instance.AddTransition(GameStart.INIT, GameStart.FLEE);

            FiniteStateMachine<GameStart>.Instance.AddTransition(GameStart.ATTACK, GameStart.REST);
            FiniteStateMachine<GameStart>.Instance.AddTransition(GameStart.DEFEND, GameStart.REST);
            FiniteStateMachine<GameStart>.Instance.AddTransition(GameStart.FLEE, GameStart.REST);

            FiniteStateMachine<GameStart>.Instance.AddTransition(GameStart.REST, GameStart.ATTACK);
            FiniteStateMachine<GameStart>.Instance.AddTransition(GameStart.REST, GameStart.DEFEND);
            FiniteStateMachine<GameStart>.Instance.AddTransition(GameStart.REST, GameStart.FLEE);

            FiniteStateMachine<GameStart>.Instance.Start(GameStart.INIT);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
