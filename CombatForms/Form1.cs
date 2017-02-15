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
        List<RichTextBox> playersText = new List<RichTextBox>();
        List<RichTextBox> enemiesText = new List<RichTextBox>();

        List<ProgressBar> playerProgess = new List<ProgressBar>();
        List<ProgressBar> enemiesProgess = new List<ProgressBar>();
        public void UpdateHub()
        {
            for (int i = 0; i < Combat.Instance.playerParty.members.Count; i++)
            {
                playersText[i].Text = Combat.Instance.playerParty.members[i].Name;
                playerProgess[i].Value = (int)Combat.Instance.playerParty.members[i].Health;
            }
            for (int i = 0; i < Combat.Instance.enemyParty.members.Count; i++)
            {
                enemiesText[i].Text = Combat.Instance.enemyParty.members[i].Name;
                enemiesProgess[i].Value = (int)Combat.Instance.enemyParty.members[i].Health;
            }
            int d = 0;
            int m = 0;
            foreach (Entity e in Combat.Instance.enemyParty.members)
                if (!e.Alive)
                    d++;
            foreach (Entity e in Combat.Instance.playerParty.members)
                if (!e.Alive)
                    m++;
            if (d >= Combat.Instance.enemyParty.members.Count || m >= Combat.Instance.playerParty.members.Count)
            {
                if (d >= Combat.Instance.enemyParty.members.Count)
                    MessageBox.Show("Winning Party: 1");
                else if (m >= Combat.Instance.playerParty.members.Count)
                    MessageBox.Show("Winning Party: 2");
                this.Close();
                return;
            }
            richTextBox1.Text = Combat.Instance.combatLog;
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
            Active.Text = ("Active Player: " + Combat.Instance.activeParty.activePlayer.Name);
        }
        public Form1()
        {
            InitializeComponent();

            playersText.Add(richTextBox2);
            playersText.Add(richTextBox3);
            playerProgess.Add(progressBar1);
            playerProgess.Add(progressBar2);

            enemiesText.Add(richTextBox4);
            enemiesText.Add(richTextBox5);
            enemiesProgess.Add(progressBar3);
            enemiesProgess.Add(progressBar4);

            UpdateHub();
            button2.Enabled = false;
            button2.Visible = false;
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
        private void richTextBox6_TextChanged(object sender, EventArgs e) { }
        #endregion

        private void Attack_Click(object sender, EventArgs e)
        {
            FiniteStateMachine<GameStart>.Instance.ChangeState(GameStart.ATTACK);
            Combat.Instance.activePlaya.Attack();
            button1.Enabled = false;
            button3.Enabled = false;
            button6.Enabled = false;
            button2.Enabled = true;
            UpdateHub();
            EndTurn_Click(sender, e);
        }
        private void EndTurn_Click(object sender, EventArgs e)
        {
            FiniteStateMachine<GameStart>.Instance.ChangeState(GameStart.REST);
            Combat.Instance.activePlaya.EndTurn();
            button1.Enabled = true;
            button6.Enabled = true;
            button3.Enabled = true;
            button2.Enabled = false;
            UpdateHub();
        }
        private void Defend_Click(object sender, EventArgs e)
        {
            FiniteStateMachine<GameStart>.Instance.ChangeState(GameStart.DEFEND);
            Combat.Instance.activePlaya.Defend();
            button1.Enabled = false;
            button3.Enabled = false;
            button6.Enabled = false;
            button2.Enabled = true;
            UpdateHub();
            EndTurn_Click(sender, e);
        }
        private void Flee_Click(object sender, EventArgs e)
        {
            FiniteStateMachine<GameStart>.Instance.ChangeState(GameStart.FLEE);
            Combat.Instance.activePlaya.Flee();
            button1.Enabled = false;
            button3.Enabled = false;
            button6.Enabled = false;
            button2.Enabled = true;
            MessageBox.Show(Combat.Instance.activeParty.activePlayer.Name + " has chose to flee!");
            EndTurn_Click(sender, e);
            UpdateHub();
        }
    }
}
