using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CombatForms
{
    public partial class Form1 : Form
    {
        public enum GameStart
        {
            INIT = 0,
            START = 1,
            SELECTMODE = 2,
            MULTIPLAYERMODE = 3,
            SINGLEPLAYERMODE = 4,
            ONLINEGAMES = 5,
            SHOWMAINSCREEN = 6,
            SERVERCONNECT = 7,
            EXIT = 8,

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
            FSM.AddState(GameStart.START);
            FSM.AddState(GameStart.SELECTMODE);
            FSM.AddState(GameStart.MULTIPLAYERMODE);
            FSM.AddState(GameStart.SINGLEPLAYERMODE);
            FSM.AddState(GameStart.ONLINEGAMES);
            FSM.AddState(GameStart.SHOWMAINSCREEN);
            FSM.AddState(GameStart.SERVERCONNECT);
            FSM.AddState(GameStart.EXIT);

            FSM.AddTransition(GameStart.INIT, GameStart.START);
            FSM.AddTransition(GameStart.START, GameStart.SELECTMODE);
            FSM.AddTransition(GameStart.SELECTMODE, GameStart.SINGLEPLAYERMODE);
            FSM.AddTransition(GameStart.SINGLEPLAYERMODE, GameStart.SHOWMAINSCREEN);
            FSM.AddTransition(GameStart.SELECTMODE, GameStart.MULTIPLAYERMODE);
            FSM.AddTransition(GameStart.MULTIPLAYERMODE, GameStart.ONLINEGAMES);
            FSM.AddTransition(GameStart.ONLINEGAMES, GameStart.SERVERCONNECT);
            FSM.AddTransition(GameStart.ONLINEGAMES, GameStart.SHOWMAINSCREEN);

            FSM.Start(GameStart.INIT);
            richTextBox1.Text = "Current State:" + FSM.GetState().Name;
        }
        private void Form1_Load(object sender, EventArgs e) { }
        private void richTextBox1_TextChanged(object sender, EventArgs e) { }
        private void Start_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(GameStart.START);
            richTextBox1.Text = "Current State:" + FSM.GetState().Name;
        }
        private void Multiplayer_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(GameStart.MULTIPLAYERMODE);
            richTextBox1.Text = "Current State:" + FSM.GetState().Name;
        }
        private void Select_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(GameStart.SELECTMODE);
            richTextBox1.Text = "Current State:" + FSM.GetState().Name;
        }
        private void Single_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(GameStart.SINGLEPLAYERMODE);
            richTextBox1.Text = "Current State:" + FSM.GetState().Name;
        }
        private void OnlineGames_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(GameStart.ONLINEGAMES);
            richTextBox1.Text = "Current State:" + FSM.GetState().Name;
        }
        private void ServerConnect_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(GameStart.SERVERCONNECT);
            richTextBox1.Text = "Current State:" + FSM.GetState().Name;
        }
        private void MainGameScreen_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(GameStart.SHOWMAINSCREEN);
            richTextBox1.Text = "Current State:" + FSM.GetState().Name;
        }
        private void Save_Click(object sender, EventArgs e)
        {
            DataManager<FiniteStateMachine<GameStart>>.Serialize("Test", FSM);
        }

        private void Load_Click(object sender, EventArgs e)
        {
            FSM = DataManager<FiniteStateMachine<GameStart>>.Deserialize("Test");
            this.richTextBox1.Text = "Current State:" + FSM.GetState().Name;
        }
    }
}
