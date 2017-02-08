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
        Combat active = new Combat();
        bool HitCheck = false;
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
        Entity da = new Entity(50, "Dwarf Archer", true);

        //static public void Attack()
        //{
        //    while (HitCheck == false)
        //    {
        //        if (cl.m_Alive == true && active.activeParty.activePlaya.Name == "Cloud")
        //        {
        //            cl.DoDamage(ds);
        //            HitCheck = true;
        //            progressBar4.Value = (int)ds.Health;
        //        }
        //        else if (cl.m_Alive == true && active.activeParty.activePlaya.Name == "Cloud" && ds.Health <= 0)
        //        {
        //            cl.DoDamage(da);
        //            progressBar3.Value = (int)da.Health;
        //            HitCheck = true;
        //        }
        //        else if (ae.m_Alive == true && active.activeParty.activePlaya.Name == "Aeris the Archer")
        //        {
        //            ae.DoDamage(da);
        //            ae.DoDamage(ds);
        //            progressBar4.Value = (int)ds.Health;
        //            progressBar3.Value = (int)da.Health;
        //            HitCheck = true;
        //        }
        //        else if (ds.m_Alive == true && active.activeParty.activePlaya.Name == "Dwarf Soilder")
        //        {
        //            ds.DoDamage(cl);
        //            progressBar1.Value = (int)cl.Health;
        //            HitCheck = true;
        //        }
        //        else if (ds.m_Alive == true && active.activeParty.activePlaya.Name == "Dwarf Soilder" && cl.Health <= 0)
        //        {
        //            ds.DoDamage(ae);
        //            progressBar2.Value = (int)ae.Health;
        //            HitCheck = true;
        //        }
        //        else if (da.m_Alive == true && active.activeParty.activePlaya.Name == "Dwarf Archer")
        //        {
        //            da.DoDamage(ae);
        //            da.DoDamage(cl);
        //            progressBar2.Value = (int)ae.Health;
        //            progressBar1.Value = (int)cl.Health;
        //            HitCheck = true;
        //        }
        //    }
        //    AliveCheck();
        //}

          static public void Attack()
        {

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
            richTextBox1.Text += "Current State: " + FSM.GetState().Name;
            richTextBox1.Text += "\nActive Entity: " + active.activeParty.activePlaya.Name;

            richTextBox2.Text = cl.Name;
            richTextBox3.Text = ae.Name;
            richTextBox4.Text = ds.Name;
            richTextBox5.Text = da.Name;
        }
        private void Form1_Load(object sender, EventArgs e) { }
        private void richTextBox1_TextChanged(object sender, EventArgs e) { }
        private void richTextBox2_TextChanged(object sender, EventArgs e) { }
        private void richTextBox3_TextChanged(object sender, EventArgs e) { }
        private void richTextBox4_TextChanged(object sender, EventArgs e) { }
        private void richTextBox5_TextChanged(object sender, EventArgs e) { }
        private void progressBar1_Click(object sender, EventArgs e) { }
        private void progressBar2_Click(object sender, EventArgs e) { }
        private void progressBar3_Click(object sender, EventArgs e) { }
        private void progressBar4_Click(object sender, EventArgs e) { }

        private void Attack_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(GameStart.ATTACK);

            richTextBox1.Text = active.activeParty.activePlaya.Name + " Chose to: " + FSM.GetState().Name + "\n";

            //This is a freaking mess honestly 

        }
        private void EndTurn_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(GameStart.ENDTURN);
            active.activeParty.activePlaya.EndTurn();
            richTextBox1.Text = active.activeParty.activePlaya.Name + " Chose to: " + FSM.GetState().Name;
            richTextBox1.Text += "\nActive Entity: " + active.activeParty.activePlaya.Name;
            HitCheck = false;
        }
        private void Defend_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(GameStart.DEFEND);
            //richTextBox1.Text = active.activeParty.activePlaya.Name + " Chose to:" + FSM.GetState().Name;
        }
        private void Flee_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(GameStart.FLEE);
            //richTextBox1.Text = "You Chose to: " + FSM.GetState().Name;
            //active.activeParty.activePlaya.TakeDamage(100);
            //active.activeParty.activePlaya.alive = false;
            //MessageBox.Show(active.activeParty.activePlaya.Name + " has ran away!");
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
