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
    public enum GameStart
    {
        INIT = 0,
        ATTACK = 1,
        REST = 2,
        DEFEND = 3,
        FLEE = 4,
    }
    public partial class Form1 : Form
    {
        Combat active = new Combat();
        bool ClickCheck = false;
        FiniteStateMachine<GameStart> FSM;

        Party a = new Party();
        Party b = new Party();
        Entity cl = new Entity(100, "Cloud", true, false, 4);
        Entity ae = new Entity(50, "Aeris the Archer", true, false, 3);
        Entity ds = new Entity(100, "Dwarf Soilder", true, false, 2);
        Entity da = new Entity(50, "Dwarf Archer", true, false, 1);

        #region Logic
        public void AliveCheck()
        {
            if (cl.m_Alive == true)
                richTextBox1.Text += cl.Name + " remaining hp: " + cl.Health + "\n";
            if (ae.m_Alive == true)
                richTextBox1.Text += ae.Name + " remaining hp: " + ae.Health + "\n";
            if (ds.m_Alive == true)
                richTextBox1.Text += ds.Name + " remaining hp: " + ds.Health + "\n";
            if (da.m_Alive == true)
                richTextBox1.Text += da.Name + " remaining hp: " + da.Health + "\n";
        }
        public void Attack()
        {
            while (ClickCheck == false)
            {
                if (ds.m_Alive == true && cl.m_Alive == true && active.activeParty.activePlaya.Name == "Cloud")
                {
                    //if (ds.m_Block == false)
                    cl.DoDamage(ds);
                    //else if (ds.m_Block == true)
                    //{
                    // cl.DoDamage(ds);
                    //ds.m_Health += 5;
                    //}
                    ClickCheck = true;
                    progressBar4.Value = (int)ds.Health;
                }
                else if (ds.m_Alive == false && cl.m_Alive == true && active.activeParty.activePlaya.Name == "Cloud" && ds.Health <= 0)
                {
                    cl.DoDamage(da);
                    progressBar3.Value = (int)da.Health;
                    ClickCheck = true;
                }
                else if (ae.m_Alive == true && active.activeParty.activePlaya.Name == "Aeris the Archer")
                {
                    if (da.m_Alive == true)
                    {
                        ae.DoDamage(da);
                        progressBar3.Value = (int)da.Health;
                    }
                    if (ds.m_Alive == true)
                    {
                        ae.DoDamage(ds);
                        progressBar4.Value = (int)ds.Health;
                    }
                    ClickCheck = true;
                }
                else if (cl.m_Alive == true && ds.m_Alive == true && active.activeParty.activePlaya.Name == "Dwarf Soilder")
                {
                    ds.DoDamage(cl);
                    progressBar1.Value = (int)cl.Health;
                    ClickCheck = true;
                }
                else if (cl.m_Alive == false && ds.m_Alive == true && active.activeParty.activePlaya.Name == "Dwarf Soilder")
                {
                    ds.DoDamage(ae);
                    progressBar2.Value = (int)ae.Health;
                    ClickCheck = true;
                }
                else if (da.m_Alive == true && active.activeParty.activePlaya.Name == "Dwarf Archer")
                {
                    if (ae.m_Alive == true)
                    {
                        da.DoDamage(ae);
                        progressBar2.Value = (int)ae.Health;
                    }
                    if (cl.m_Alive == true)
                    {
                        da.DoDamage(cl);
                        progressBar1.Value = (int)cl.Health;
                    }
                    ClickCheck = true;
                }
            }
        }
        public void Kill()
        {
            active.activeParty.activePlaya.TakeDamage(100);
            active.activeParty.activePlaya.m_Alive = false;
            MessageBox.Show(active.activeParty.activePlaya.Name + " has ran away!");
        }
        public void Flee()
        {
            while (ClickCheck == false)
            {
                if (active.activeParty.activePlaya.Name == "Cloud")
                {
                    Kill();
                    progressBar1.Value = (int)cl.Health;
                    ClickCheck = true;
                }
                else if (active.activeParty.activePlaya.Name == "Aeris the Archer")
                {
                    Kill();
                    progressBar2.Value = (int)ae.Health;
                    ClickCheck = true;
                }
                else if (active.activeParty.activePlaya.Name == "Dwarf Soilder")
                {
                    Kill();
                    progressBar4.Value = (int)ds.Health;
                    ClickCheck = true;
                }
                else if (active.activeParty.activePlaya.Name == "Dwarf Archer")
                {
                    Kill();
                    progressBar3.Value = (int)da.Health;
                    ClickCheck = true;
                }
            }
        }
        public void Defend()
        {
            active.activeParty.activePlaya.m_Block = true;
        }
        #endregion

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
            FSM.AddState(GameStart.REST);
            FSM.AddState(GameStart.DEFEND);
            FSM.AddState(GameStart.FLEE);

            FSM.AddTransition(GameStart.INIT, GameStart.ATTACK);
            FSM.AddTransition(GameStart.INIT, GameStart.DEFEND);
            FSM.AddTransition(GameStart.INIT, GameStart.FLEE);

            FSM.AddTransition(GameStart.ATTACK, GameStart.REST);
            FSM.AddTransition(GameStart.DEFEND, GameStart.REST);
            FSM.AddTransition(GameStart.FLEE, GameStart.REST);

            FSM.AddTransition(GameStart.REST, GameStart.ATTACK);
            FSM.AddTransition(GameStart.REST, GameStart.DEFEND);
            FSM.AddTransition(GameStart.REST, GameStart.FLEE);

            FSM.Start(GameStart.INIT);

            richTextBox1.Text = "Active Entity: " + active.activeParty.activePlaya.Name + "\n";

            richTextBox2.Text = cl.Name;
            richTextBox3.Text = ae.Name;
            richTextBox4.Text = ds.Name;
            richTextBox5.Text = da.Name;
        }

        #region Text Box and Health Bar
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
        #endregion

        private void Attack_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(GameStart.ATTACK);
            Attack();
            AliveCheck();
        }
        private void EndTurn_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(GameStart.REST);
            active.activeParty.activePlaya.EndTurn();
            ClickCheck = false;
            richTextBox1.Text = "Active Entity: " + active.activeParty.activePlaya.Name + "\n";
        }
        private void Defend_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(GameStart.DEFEND);
            //Defend();
        }
        private void Flee_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(GameStart.FLEE);
            Flee();
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
