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
    public enum PlayerStates
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
            
            


            FiniteStateMachine<PlayerStates> entity_fsm = new FiniteStateMachine<PlayerStates>();
            entity_fsm.AddState(PlayerStates.INIT);
            entity_fsm.AddState(PlayerStates.ATTACK);
            entity_fsm.AddState(PlayerStates.REST);
            entity_fsm.AddState(PlayerStates.DEFEND);
            entity_fsm.AddState(PlayerStates.FLEE);

            entity_fsm.AddTransition(PlayerStates.INIT, PlayerStates.ATTACK);
            entity_fsm.AddTransition(PlayerStates.INIT, PlayerStates.DEFEND);
            entity_fsm.AddTransition(PlayerStates.INIT, PlayerStates.FLEE);

            entity_fsm.AddTransition(PlayerStates.ATTACK, PlayerStates.REST);
            entity_fsm.AddTransition(PlayerStates.DEFEND, PlayerStates.REST);
            entity_fsm.AddTransition(PlayerStates.FLEE, PlayerStates.REST);

            entity_fsm.AddTransition(PlayerStates.REST, PlayerStates.ATTACK);
            entity_fsm.AddTransition(PlayerStates.REST, PlayerStates.DEFEND);
            entity_fsm.AddTransition(PlayerStates.REST, PlayerStates.FLEE);

            Party playerParty = new Party();
            Party enemyParty = new Party();

            Entity cloud = new Entity(100, "Cloud", true, false, 1, Entity.EType.PLAYER, entity_fsm);
            Entity aeris = new Entity(100, "Aeris the Archer", true, false, 4, Entity.EType.PLAYER, entity_fsm);

            Entity entitySoldier = new Entity(100, "Dwarf Soldier", true, false, 3, Entity.EType.ENEMY, entity_fsm);
            Entity entityArcher = new Entity(100, "Dwarf Archer", true, false, 3, Entity.EType.ENEMY, entity_fsm);

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

            Combat.Instance.NextParty();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new WaterEmblem());
        }
    }
}
