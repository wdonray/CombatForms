using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace CombatForms
{
    public partial class Form1 : Form
    {
        //Entity Player = new Entity(100);
        //Entity Enemy = new Entity(100);
        //bool defend = false;
        //Random flee = new Random();
        Combat active = new Combat();
        public delegate void Callback();
        FiniteStateMachine<GameStart> FSM;
        public enum GameStart
        {
            INIT = 0,
            ATTACK = 1,
            ENDTURN = 2,
            DEFEND = 3,
            FLEE = 4,
        }
        Party a = new Party();
        Party b = new Party();
        Entity cl = new Entity(100, "Cloud", true);
        Entity ae = new Entity(50, "Aeris the Archer", true);
        Entity ds = new Entity(100, "Dwarf Soilder", true);
        Entity da = new Entity(50, "Dwark Archer", true);

        public void AliveCheck()
        {
            if (cl.alive == true)
                richTextBox1.Text += cl.Name + " remaining hp: " + cl.Health + "\n";
            if (ae.alive == true)
                richTextBox1.Text += ae.Name + " remaining hp: " + ae.Health + "\n";
            if (ds.alive == true)
                richTextBox1.Text += ds.Name + " remaining hp: " + ds.Health + "\n";
            if (da.alive == true)
                richTextBox1.Text += da.Name + " remaining hp: " + da.Health + "\n";
        }
        public Form1()
        {
            InitializeComponent();

            active.AddParty(a);
            active.AddParty(b);
            active.AddPlaya(cl, 1);
            active.AddPlaya(ae, 1);
            active.AddPlaya(ds, 2);
            active.AddPlaya(da, 2);

            FSM = new FiniteStateMachine<GameStart>();
            FSM.AddState(GameStart.INIT);
            FSM.AddState(GameStart.ATTACK);
            FSM.AddState(GameStart.ENDTURN);
            FSM.AddState(GameStart.DEFEND);
            FSM.AddState(GameStart.FLEE);

            FSM.AddTransition(GameStart.INIT, GameStart.ATTACK);
            FSM.AddTransition(GameStart.INIT, GameStart.DEFEND);
            FSM.AddTransition(GameStart.INIT, GameStart.FLEE);

            FSM.AddTransition(GameStart.ATTACK, GameStart.ENDTURN);
            FSM.AddTransition(GameStart.DEFEND, GameStart.ENDTURN);
            FSM.AddTransition(GameStart.FLEE, GameStart.ENDTURN);

            FSM.AddTransition(GameStart.ENDTURN, GameStart.ATTACK);
            FSM.AddTransition(GameStart.ENDTURN, GameStart.DEFEND);
            FSM.AddTransition(GameStart.ENDTURN, GameStart.FLEE);

            FSM.Start(GameStart.INIT);
            richTextBox1.Text += "Current State:" + FSM.GetState().Name;
            richTextBox1.Text += "\nActive Entity: " + active.activeParty.activePlaya.Name;
        }
        private void Form1_Load(object sender, EventArgs e) { }
        private void richTextBox1_TextChanged(object sender, EventArgs e) { }
        private void Attack_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(GameStart.ATTACK);

            richTextBox1.Text = active.activeParty.activePlaya.Name + " Chose to: " + FSM.GetState().Name + "\n";

            if (cl.alive == true && active.activeParty.activePlaya.Name == "Cloud")
                cl.DoDamage(ds);
            else if (cl.alive == true && active.activeParty.activePlaya.Name == "Cloud" && ds.Health <= 0)
                cl.DoDamage(ds);
            else if (ae.alive == true && active.activeParty.activePlaya.Name == "Aeris the Archer")
            {
                ae.DoDamage(da);
                ae.DoDamage(ds);
            }
            else if (ds.alive == true && active.activeParty.activePlaya.Name == "Dwarf Soilder")
                ds.DoDamage(cl);
            else if (ds.alive == true && active.activeParty.activePlaya.Name == "Dwarf Soilder" && cl.Health <= 0)
                ds.DoDamage(ae);
            else if (da.alive == true && active.activeParty.activePlaya.Name == "Dwarf Archer")
            {
                da.DoDamage(ae);
                da.DoDamage(cl);
            }

            AliveCheck();

            //Player.DoDamage(Enemy);
            //richTextBox1.Text += "\nYou attacked, Enemy's Hp: " + Enemy.Health;
        }
        private void EndTurn_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(GameStart.ENDTURN);
            active.activeParty.activePlaya.EndTurn();
            richTextBox1.Text = active.activeParty.activePlaya.Name + " Chose to:" + FSM.GetState().Name;
            richTextBox1.Text += "\nActive Entity: " + active.activeParty.activePlaya.Name;

            //if (defend == false)
            //{
            //    Enemy.DoDamage(Player);
            //    richTextBox1.Text += "\nEnemy attacked you! Your Hp: " + Player.Health;
            //}
            //else
            //    richTextBox1.Text += "\nEnemy attacked you! Blocked! Your Hp: " + Player.Health;
        }
        private void Defend_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(GameStart.DEFEND);
            richTextBox1.Text = active.activeParty.activePlaya.Name + " Chose to:" + FSM.GetState().Name;

            //defend = true;
        }
        private void Flee_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(GameStart.FLEE);
            richTextBox1.Text = "You Chose to:" + FSM.GetState().Name;
            active.activeParty.activePlaya.alive = false;
            MessageBox.Show(active.activeParty.activePlaya.Name + " has ran away!");
            //Enemy.DoDamage(Player);
            //richTextBox1.Text += "\nYou took extra damage for being a wimp! \nEnemy attacked you! Your Hp: " + Player.Health;
        }
        private void Save_Click(object sender, EventArgs e)
        {
            //DataManager<FiniteStateMachine<GameStart>>.Serialize("Test", FSM);
        }

        private void Load_Click(object sender, EventArgs e)
        {
            //FSM = DataManager<FiniteStateMachine<GameStart>>.Deserialize("Test");
            //this.richTextBox1.Text = "Current State:" + FSM.GetState().Name;
        }
    }
}
