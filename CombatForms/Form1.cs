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
            for (int i = 0; i < Combat.Instance.playerParty.members.Count; i++)
            {
                if (Combat.Instance.playerParty.members[i].Alive == false)
                    playersText[i].Text = "Dead : " + (int)Combat.Instance.playerParty.members[i].Health;
                else
                    playersText[i].Text = Combat.Instance.playerParty.members[i].Name + " : " + (int)Combat.Instance.playerParty.members[i].Health + "/" + Combat.Instance.playerParty.members[i].MaxHealth;
                playersProgess[i].Maximum = Combat.Instance.playerParty.members[i].MaxHealth;
                playersProgess[i].Value = (int)(((float)Combat.Instance.playerParty.members[i].Health / (float)Combat.Instance.playerParty.members[i].MaxHealth) * 100);
                playersEXP[i].Text = "Level " + Combat.Instance.playerParty.members[i].LevelUp + " : " + Combat.Instance.playerParty.members[i].Exp + " / " + Combat.Instance.playerParty.members[i].MaxExp;
            }
            for (int i = 0; i < Combat.Instance.enemyParty.members.Count; i++)
            {
                if (Combat.Instance.enemyParty.members[i].Alive == false)
                    enemiesText[i].Text = "Dead : " + (int)Combat.Instance.enemyParty.members[i].Health;
                else
                    enemiesText[i].Text = Combat.Instance.enemyParty.members[i].Name + " : " + (int)Combat.Instance.enemyParty.members[i].Health + "/" + Combat.Instance.enemyParty.members[i].MaxHealth;
                enemiesProgess[i].Maximum = Combat.Instance.enemyParty.members[i].MaxHealth;
                enemiesProgess[i].Value = (int)(((float)Combat.Instance.enemyParty.members[i].Health / (float)Combat.Instance.enemyParty.members[i].MaxHealth) * 100);
                enemiesEXP[i].Text = "Level " + Combat.Instance.enemyParty.members[i].LevelUp + " : " + Combat.Instance.enemyParty.members[i].Exp + " / " + Combat.Instance.enemyParty.members[i].MaxExp;
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
            Combat.Instance.combatLog += Combat.Instance.Space;
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
            Combat.Instance.activeParty.activePlayer.Attack();
            button1.Enabled = false;
            button3.Enabled = false;
            button6.Enabled = false;
            button2.Enabled = true;
            UpdateHud();
            EndTurn_Click(sender, e);
        }
        private void EndTurn_Click(object sender, EventArgs e)
        {
            Combat.Instance.activeParty.activePlayer.EndTurn();
            button1.Enabled = true;
            button6.Enabled = true;
            button3.Enabled = true;
            button2.Enabled = false;
            UpdateHud();
        }
        private void Defend_Click(object sender, EventArgs e)
        {
            Combat.Instance.activeParty.activePlayer.Defend();
            button1.Enabled = false;
            button3.Enabled = false;
            button6.Enabled = false;
            button2.Enabled = true;
            UpdateHud();
            EndTurn_Click(sender, e);
        }
        private void Flee_Click(object sender, EventArgs e)
        {
            Combat.Instance.activeParty.activePlayer.Flee();
            button1.Enabled = false;
            button3.Enabled = false;
            button6.Enabled = false;
            button2.Enabled = true;
            UpdateHud();
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
            DataManager<Entity>.Serialize("ACTIVE PLAYER", Combat.Instance.activeParty.activePlayer);
            DataManager<Party>.Serialize("ACTIVE PARTY", Combat.Instance.activeParty);
            DataManager<Party>.Serialize("INACTIVE PARTY", Combat.Instance.inactiveParty);
            DataManager<List<Entity>>.Serialize("PLAYER PARTY MEMBERS", Combat.Instance.playerParty.members);
            DataManager<List<Entity>>.Serialize("ENEMY PARTY MEMBERS", Combat.Instance.enemyParty.members);
        }

        private void Loader_Click(object sender, EventArgs e)
        {
            Save.Visible = false;
            Loader.Visible = false;
            Exit.Visible = false;
            Restart.Visible = false;
            button1.Enabled = true;
            button6.Enabled = true;
            button3.Enabled = true;
            Options.Visible = true;
            
            Combat.Instance.activeParty = DataManager<Party>.Deserialize("ACTIVE PARTY");
            Combat.Instance.inactiveParty = DataManager<Party>.Deserialize("INACTIVE PARTY");
            Combat.Instance.playerParty.members = DataManager<List<Entity>>.Deserialize("PLAYER PARTY MEMBERS");
            Combat.Instance.enemyParty.members = DataManager<List<Entity>>.Deserialize("ENEMY PARTY MEMBERS");
<<<<<<< HEAD
            List<Entity> tmp;
            List<Entity> tmp2;
            tmp = Combat.Instance.playerParty.members;
            tmp2 = Combat.Instance.enemyParty.members;
            foreach (Entity p in tmp.ToList())
            {
                Combat.Instance.AddPlayerParty(p);
                p.onEndTurn.Invoke();
            }
            foreach (Entity p in tmp2.ToList())
            {
                Combat.Instance.AddEnemyParty(p);
                p.onEndTurn.Invoke();
            }
            UpdateHub();
        }
=======
            Combat.Instance.activeParty.activePlayer = DataManager<Entity>.Deserialize("ACTIVE PLAYER");
            Combat.Instance.activeParty.activePlayer.fsm.RebuildFSM();
            Combat.Instance.activePlaya.fsm.Start(Combat.Instance.activePlaya.CurrentState);
>>>>>>> 87a4c79802b580ccf592e947ebeb33487baeac16

            Combat.Instance.activeParty.activePlayer.onEndTurn += Combat.Instance.activeParty.GetNext;
            foreach (var member in Combat.Instance.activeParty.members)
            {
                member.fsm.RebuildFSM();
                member.fsm.Start(member.CurrentState);
                member.onEndTurn += Combat.Instance.activeParty.GetNext;
            }
                
            foreach (var member in Combat.Instance.playerParty.members)
            {
                member.fsm.RebuildFSM();
                member.fsm.Start(member.CurrentState);
                member.onEndTurn += Combat.Instance.playerParty.GetNext;
            }
                
            foreach (var member in Combat.Instance.enemyParty.members)
            {
                
                member.fsm.RebuildFSM();
                member.fsm.Start(member.CurrentState);
                member.onEndTurn += Combat.Instance.enemyParty.GetNext;
            }
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
