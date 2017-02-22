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
    public partial class WaterEmblem : Form
    {
        List<RichTextBox> playersText = new List<RichTextBox>();
        List<TextBox> playersEXP = new List<TextBox>();
        List<RichTextBox> enemiesText = new List<RichTextBox>();
        List<TextBox> enemiesEXP = new List<TextBox>();

        List<ProgressBar> playersProgess = new List<ProgressBar>();
        List<ProgressBar> enemiesProgess = new List<ProgressBar>();
        public void UpdateHud()
        {
            for (int i = 0; i < Combat.Instance.CV.PlayerParty.members.Count; i++)
            {
                if (Combat.Instance.CV.PlayerParty.members[i].Alive == false)
                    playersText[i].Text = "Dead : " + (int)Combat.Instance.CV.PlayerParty.members[i].Health;
                else
                    playersText[i].Text = Combat.Instance.CV.PlayerParty.members[i].Name + " : " + (int)Combat.Instance.CV.PlayerParty.members[i].Health + "/" + Combat.Instance.CV.PlayerParty.members[i].MaxHealth;
                playersProgess[i].Maximum = Combat.Instance.CV.PlayerParty.members[i].MaxHealth;
                playersProgess[i].Value = (int)(((float)Combat.Instance.CV.PlayerParty.members[i].Health / (float)Combat.Instance.CV.PlayerParty.members[i].MaxHealth) * 100);
                playersEXP[i].Text = "Level " + Combat.Instance.CV.PlayerParty.members[i].LevelUp + " : " + Combat.Instance.CV.PlayerParty.members[i].Exp + " / " + Combat.Instance.CV.PlayerParty.members[i].MaxExp;
            }
            for (int i = 0; i < Combat.Instance.CV.EnemyParty.members.Count; i++)
            {
                if (Combat.Instance.CV.EnemyParty.members[i].Alive == false)
                    enemiesText[i].Text = "Dead : " + (int)Combat.Instance.CV.EnemyParty.members[i].Health;
                else
                    enemiesText[i].Text = Combat.Instance.CV.EnemyParty.members[i].Name + " : " + (int)Combat.Instance.CV.EnemyParty.members[i].Health + "/" + Combat.Instance.CV.EnemyParty.members[i].MaxHealth;
                enemiesProgess[i].Maximum = Combat.Instance.CV.EnemyParty.members[i].MaxHealth;
                enemiesProgess[i].Value = (int)(((float)Combat.Instance.CV.EnemyParty.members[i].Health / (float)Combat.Instance.CV.EnemyParty.members[i].MaxHealth) * 100);
                enemiesEXP[i].Text = "Level " + Combat.Instance.CV.EnemyParty.members[i].LevelUp + " : " + Combat.Instance.CV.EnemyParty.members[i].Exp + " / " + Combat.Instance.CV.EnemyParty.members[i].MaxExp;
            }
            int d = 0;
            int m = 0;
            foreach (Entity e in Combat.Instance.CV.EnemyParty.members)
                if (!e.Alive)
                    d++;
            foreach (Entity e in Combat.Instance.CV.PlayerParty.members)
                if (!e.Alive)
                    m++;
            if (d >= Combat.Instance.CV.EnemyParty.members.Count || m >= Combat.Instance.CV.PlayerParty.members.Count)
            {
                if (d >= Combat.Instance.CV.EnemyParty.members.Count)
                    MessageBox.Show("Winning Party: 1");
                else if (m >= Combat.Instance.CV.PlayerParty.members.Count)
                    MessageBox.Show("Winning Party: 2");
                this.Close();
                return;
            }
            richTextBox1.Text = Combat.Instance.combatLog;
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
            Active.Text = ("Active Player: " + Combat.Instance.CV.ActiveParty.ActivePlayer.Name);
        }
        public WaterEmblem()
        {
            InitializeComponent();

            playersText.Add(richTextBox2);
            playersEXP.Add(textBox1);
            playersText.Add(richTextBox3);
            playersEXP.Add(textBox2);
            playersProgess.Add(progressBar1);
            playersProgess.Add(progressBar2);

            enemiesText.Add(richTextBox4);
            enemiesEXP.Add(textBox3);
            enemiesText.Add(richTextBox5);
            enemiesEXP.Add(textBox4);
            enemiesProgess.Add(progressBar3);
            enemiesProgess.Add(progressBar4);

            UpdateHud();

            button2.Enabled = false;
            button2.Visible = false;
            Save.Visible = false;
            Loader.Visible = false;
            Exit.Visible = false;
            Restart.Visible = false;
            Combat.Instance.combatLog += "-------------------------------------------------------------";
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
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
        #endregion
        private void Attack_Click(object sender, EventArgs e)
        {
            Combat.Instance.CV.ActiveParty.ActivePlayer.Attack();
            button1.Enabled = false;
            button3.Enabled = false;
            button6.Enabled = false;
            button2.Enabled = true;
            UpdateHud();
            EndTurn_Click(sender, e);
        }
        private void EndTurn_Click(object sender, EventArgs e)
        {
            Combat.Instance.CV.ActiveParty.ActivePlayer.EndTurn();
            button1.Enabled = true;
            button6.Enabled = true;
            button3.Enabled = true;
            button2.Enabled = false;
            UpdateHud();
        }
        private void Defend_Click(object sender, EventArgs e)
        {
            Combat.Instance.CV.ActiveParty.ActivePlayer.Defend();
            button1.Enabled = false;
            button3.Enabled = false;
            button6.Enabled = false;
            button2.Enabled = true;
            UpdateHud();
            EndTurn_Click(sender, e);
        }
        private void Flee_Click(object sender, EventArgs e)
        {
            Combat.Instance.CV.ActiveParty.ActivePlayer.Flee();
            button1.Enabled = false;
            button3.Enabled = false;
            button6.Enabled = false;
            button2.Enabled = true;
            UpdateHud();
            MessageBox.Show(Combat.Instance.CV.ActiveParty.ActivePlayer.Name + " has chosen to leave the battle!");
            EndTurn_Click(sender, e);
        }
        private void Options_Click(object sender, EventArgs e)
        {
            if (Save.Visible == false)
            {
                Save.Visible = true;
                Loader.Visible = true;
                Exit.Visible = true;
                Restart.Visible = true;
                button1.Enabled = false;
                button6.Enabled = false;
                button3.Enabled = false;
            }
            else
            {
                Save.Visible = false;
                Loader.Visible = false;
                Exit.Visible = false;
                Restart.Visible = false;
                button1.Enabled = true;
                button6.Enabled = true;
                button3.Enabled = true;
            }
        }
        private void Save_Click(object sender, EventArgs e)
        {
            Save.Visible = false;
            Loader.Visible = false;
            Exit.Visible = false;
            Restart.Visible = false;
            button1.Enabled = true;
            button6.Enabled = true;
            button3.Enabled = true;
            Options.Visible = true;

            DataManager<CombatVariables>.Serialize("PARTY MEMBERS", Combat.Instance.CV);
        }

        private void Loader_Click(object sender, EventArgs e)
        {
            Combat.Instance.Reset();
            Save.Visible = false;
            Loader.Visible = false;
            Exit.Visible = false;
            Restart.Visible = false;
            button1.Enabled = true;
            button6.Enabled = true;
            button3.Enabled = true;
            Options.Visible = true;

            Combat.Instance.CV = DataManager<CombatVariables>.Deserialize("PARTY MEMBERS");

            foreach (var member in Combat.Instance.CV.EnemyParty.members)
            {
                member.fsm.RebuildFSM();
                member.fsm.Start(member.CurrentState);
                member.onEndTurn += Combat.Instance.CV.EnemyParty.SetNextPlayer;
                member.onEndTurn += Combat.Instance.NextParty;
            }
            foreach (var member in Combat.Instance.CV.PlayerParty.members)
            {
                member.fsm.RebuildFSM();
                member.fsm.Start(member.CurrentState);
                member.onEndTurn += Combat.Instance.CV.PlayerParty.SetNextPlayer;
                 member.onEndTurn += Combat.Instance.NextParty;
            }
            foreach (var member in Combat.Instance.CV.ActiveParty.members)
            {
                member.fsm.RebuildFSM();
                member.fsm.Start(member.CurrentState);
                member.onEndTurn += Combat.Instance.CV.ActiveParty.SetNextPlayer;
                member.onEndTurn += Combat.Instance.NextParty;
            }
            foreach (var parties in Combat.Instance.CV.CombatParty)
            {
                parties.onPartyEnd += Combat.Instance.NextParty;
            }

            //Combat.Instance.AddToCombatParty(Combat.Instance.CV.EnemyParty);
            //Combat.Instance.AddToCombatParty(Combat.Instance.CV.PlayerParty);
            //Combat.Instance.AddToCombatParty(Combat.Instance.CV.ActiveParty);

            Combat.Instance.CV.ActiveParty.ActivePlayer.fsm.RebuildFSM();
            Combat.Instance.CV.ActiveParty.ActivePlayer.fsm.Start(Combat.Instance.CV.ActiveParty.ActivePlayer.CurrentState);
            Combat.Instance.CV.ActiveParty.ActivePlayer.onEndTurn += Combat.Instance.CV.EnemyParty.SetNextPlayer;
           // Combat.Instance.CV.ActiveParty.ActivePlayer.onEndTurn += Combat.Instance.NextParty;
            Combat.Instance.CV.ActiveParty.onPartyEnd += Combat.Instance.NextParty;

            Combat.Instance.CV.PlayerParty.ActivePlayer.fsm.RebuildFSM();
            Combat.Instance.CV.PlayerParty.ActivePlayer.fsm.Start(Combat.Instance.CV.PlayerParty.ActivePlayer.CurrentState);
            Combat.Instance.CV.PlayerParty.ActivePlayer.onEndTurn += Combat.Instance.CV.PlayerParty.SetNextPlayer;
           // Combat.Instance.CV.PlayerParty.ActivePlayer.onEndTurn += Combat.Instance.NextParty;
            Combat.Instance.CV.PlayerParty.onPartyEnd += Combat.Instance.NextParty;

            Combat.Instance.CV.EnemyParty.ActivePlayer.fsm.RebuildFSM();
            Combat.Instance.CV.EnemyParty.ActivePlayer.fsm.Start(Combat.Instance.CV.EnemyParty.ActivePlayer.CurrentState);
            Combat.Instance.CV.EnemyParty.ActivePlayer.onEndTurn += Combat.Instance.CV.EnemyParty.SetNextPlayer;
          //  Combat.Instance.CV.EnemyParty.ActivePlayer.onEndTurn += Combat.Instance.NextParty;
            Combat.Instance.CV.EnemyParty.onPartyEnd += Combat.Instance.NextParty;

            UpdateHud();
        }
        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Restart_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
