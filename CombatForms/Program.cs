using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CombatForms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Entity cl = new Entity(100, "Cloud", true, false, 4);
            Entity ae = new Entity(50, "Aeris the Archer", true, false, 3);
            Entity ds = new Entity(100, "Dwarf Soilder", true, false, 2);
            Entity da = new Entity(50, "Dwarf Archer", true, false, 1);
            GameManager.Instance.player1 = cl;
            GameManager.Instance.player2 = ae;
            GameManager.Instance.player3 = ds;
            GameManager.Instance.player4 = da;



        //     public void AliveCheck()
        //{
        //    if (cl.m_Alive == true)
        //        richTextBox1.Text += cl.Name + " remaining hp: " + cl.Health + "\n";
        //    if (ae.m_Alive == true)
        //        richTextBox1.Text += ae.Name + " remaining hp: " + ae.Health + "\n";
        //    if (ds.m_Alive == true)
        //        richTextBox1.Text += ds.Name + " remaining hp: " + ds.Health + "\n";
        //    if (da.m_Alive == true)
        //        richTextBox1.Text += da.Name + " remaining hp: " + da.Health + "\n";
        //    if (cl.m_Alive == false && ae.m_Alive == false)
        //        this.Close();
        //    if (ds.m_Alive == false && da.m_Alive == false)
        //        this.Close();
        //}

        Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
