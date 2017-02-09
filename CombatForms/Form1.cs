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
using System.Diagnostics;
namespace CombatForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            richTextBox1.Text = "Active Entity: " + Combat.Instance.activeParty.activePlaya.Name + "\n";
            richTextBox2.Text = GameManager.Instance.player1.Name;
            richTextBox3.Text = GameManager.Instance.player2.Name;
            richTextBox4.Text = GameManager.Instance.player3.Name;
            richTextBox5.Text = GameManager.Instance.player4.Name;

            progressBar1.Value = (int)GameManager.Instance.player1.Health;
            progressBar2.Value = (int)GameManager.Instance.player2.Health;
            progressBar3.Value = (int)GameManager.Instance.player4.Health;
            progressBar4.Value = (int)GameManager.Instance.player3.Health;

            button2.Enabled = false;
        }
        public void AliveCheck()
        {
            if (GameManager.Instance.player1.Alive == true)
                richTextBox1.Text += GameManager.Instance.player1.Name + " remaining hp: " + GameManager.Instance.player1.Health + "\n";
            if (GameManager.Instance.player2.Alive == true)
                richTextBox1.Text += GameManager.Instance.player2.Name + " remaining hp: " + GameManager.Instance.player2.Health + "\n";
            if (GameManager.Instance.player3.Alive == true)
                richTextBox1.Text += GameManager.Instance.player3.Name + " remaining hp: " + GameManager.Instance.player3.Health + "\n";
            if (GameManager.Instance.player4.Alive == true)
                richTextBox1.Text += GameManager.Instance.player4.Name + " remaining hp: " + GameManager.Instance.player4.Health + "\n";
            if (GameManager.Instance.player1.Alive == false && GameManager.Instance.player2.Alive == false)
                this.Close();
            if (GameManager.Instance.player3.Alive == false && GameManager.Instance.player4.Alive == false)
                this.Close();
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
            FiniteStateMachine<GameStart>.Instance.ChangeState(GameStart.ATTACK);
            richTextBox1.Text = "Active Entity: " + Combat.Instance.activeParty.activePlaya.Name + "\n";
            GameManager.Instance.currentPlayer.Attack();
            button1.Enabled = false;
            button3.Enabled = false;
            button6.Enabled = false;
            button2.Enabled = true;
            AliveCheck();
        }
        private void EndTurn_Click(object sender, EventArgs e)
        {
            FiniteStateMachine<GameStart>.Instance.ChangeState(GameStart.REST);
            Combat.Instance.activeParty.activePlaya.EndTurn();
            button1.Enabled = true;
            button6.Enabled = true;
            button3.Enabled = true;
            button2.Enabled = false;
            richTextBox1.Text = "Active Entity: " + Combat.Instance.activeParty.activePlaya.Name + "\n";
            progressBar1.Value = (int)GameManager.Instance.player1.Health;
            progressBar2.Value = (int)GameManager.Instance.player2.Health;
            progressBar3.Value = (int)GameManager.Instance.player4.Health;
            progressBar4.Value = (int)GameManager.Instance.player3.Health;
        }
        private void Defend_Click(object sender, EventArgs e)
        {
            FiniteStateMachine<GameStart>.Instance.ChangeState(GameStart.DEFEND);
            button1.Enabled = false;
            button3.Enabled = false;
            button6.Enabled = false;
            button2.Enabled = true;
            GameManager.Instance.currentPlayer.Defend();
        }
        private void Flee_Click(object sender, EventArgs e)
        {
            FiniteStateMachine<GameStart>.Instance.ChangeState(GameStart.FLEE);
            button1.Enabled = false;
            button3.Enabled = false;
            button6.Enabled = false;
            button2.Enabled = true;
            MessageBox.Show(Combat.Instance.activeParty.activePlaya.Name + " has chose to flee!");
            GameManager.Instance.currentPlayer.Flee();
            AliveCheck();
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
