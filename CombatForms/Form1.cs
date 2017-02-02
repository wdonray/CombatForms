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
        public enum Light
        {
            INIT = 0,
            RED = 1,
            GREEN = 2,
            YELLOW = 3,
            EXIT = 4,
        }
        public delegate void Callback();
        FiniteStateMachine<Light> FSM;
        public void Start() { }
        public void Exit() { }
        public Form1()
        {
            InitializeComponent();
            FSM = new FiniteStateMachine<Light>();
            FSM.AddState(Light.INIT);
            FSM.AddState(Light.RED);
            FSM.AddState(Light.GREEN);
            FSM.AddState(Light.YELLOW);

            FSM.AddTransition(Light.INIT, Light.RED);
            FSM.AddTransition(Light.RED, Light.GREEN);
            FSM.AddTransition(Light.GREEN, Light.YELLOW);
            FSM.AddTransition(Light.YELLOW, Light.RED);

            FSM.Start(Light.INIT);
            Console.WriteLine("Current State:" + FSM.GetState().Name);
            //FSM.GetState(Light.INIT).AddEnter((Callback)Start);
            FSM.GetState(Light.RED).AddEnter((Callback)Start);
        }
        private void Form1_Load(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(Light.RED);
            richTextBox1.Text = "Current State:" + FSM.GetState().Name;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(Light.GREEN);
            richTextBox1.Text = "Current State:" + FSM.GetState().Name;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            FSM.ChangeState(Light.YELLOW);
            richTextBox1.Text = "Current State:" + FSM.GetState().Name;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e) { }

        private void button4_Click(object sender, EventArgs e)
        {
            DataManager<FiniteStateMachine<Light>>.Serialize("Test", FSM.GetState().Name);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FSM = DataManager<FiniteStateMachine<Light>>.Deserialize("Test");
            this.richTextBox1.Text = "Current State:" + FSM.GetState().Name;
        }
    }
}
