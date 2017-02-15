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
            Party playerParty = new Party();
            Party enemyParty = new Party(); 

            Entity cloud = new Entity(100, "Cloud", true, false, 1, Entity.EType.PLAYER);
            Entity aeris = new Entity(100, "Aeris the Archer", true, false, 4, Entity.EType.PLAYER);

            Entity entitySoldier = new Entity(100, "Dwarf Soldier", true, false, 2, Entity.EType.ENEMY);
            Entity entityArcher = new Entity(100, "Dwarf Archer", true, false, 3, Entity.EType.ENEMY);

            playerParty.AddPlayer(cloud);
            playerParty.AddPlayer(aeris);
            enemyParty.AddPlayer(entitySoldier);
            enemyParty.AddPlayer(entityArcher);

            Combat.Instance.AddToCombatParty(playerParty);
            Combat.Instance.AddToCombatParty(enemyParty);

            Combat.Instance.AddPlayerParty(cloud);
            Combat.Instance.AddPlayerParty(aeris);
            Combat.Instance.AddEnemyParty(entitySoldier);
            Combat.Instance.AddEnemyParty(entityArcher);

            Combat.Instance.CombatPartyMembers.Sort((a, b) => -1 * a.Speed.CompareTo(b.Speed));

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

           // Combat.Instance.NextParty();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
