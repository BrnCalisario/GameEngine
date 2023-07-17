using System;
using System.Drawing;
using System.Windows.Forms;

namespace Engine
{
    public partial class GameForm : Form
    {
        GameEngine engine;

        public GameForm()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
        }

        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            engine.HandleKey(e, false);
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
                Application.Exit();

            engine.HandleKey(e, true);
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            engine = new BasicEngine(this);
            engine.Interval = 25;
            engine.Start();
        }


    }
}