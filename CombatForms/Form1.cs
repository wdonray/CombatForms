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
        public enum GameStart
        {
            INIT = 0,
            ATTACK = 1,
            ENDTURN = 2,
            DEFEND = 3,
            FLEE = 4,

        }
        public delegate void Callback();
        FiniteStateMachine<GameStart> FSM;
        public void Start() { }
        public void Exit() { }
        public Form1()
        {
            InitializeComponent();
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
            richTextBox1.Text = "Current State:" + FSM.GetState().Name;
        }
        private void Form1_Load(object sender, EventArgs e) { }
        private void richTextBox1_TextChanged(object sender, EventArgs e) { }
        private void Attack_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(GameStart.ATTACK);
            richTextBox1.Text = "You Chose to:" + FSM.GetState().Name;
        }
        private void EndTurn_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(GameStart.ENDTURN);
            richTextBox1.Text = "You Chose to:" + FSM.GetState().Name;
        }
        private void Defend_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(GameStart.DEFEND);
            richTextBox1.Text = "You Chose to:" + FSM.GetState().Name;
        }
        private void Flee_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(GameStart.FLEE);
            richTextBox1.Text = "You Chose to:" + FSM.GetState().Name;
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
