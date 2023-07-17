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

            CollidableBody w = new Wall(new Rectangle(0, 0, 30, GameEngine.Height));
            engine.AddBody(w);

            CollidableBody w2 = new Wall(new Rectangle(0, GameEngine.Height - 30, GameEngine.Width, 30));
            engine.AddBody(w2);

            CollidableBody w3 = new Wall(new Rectangle(400, 300, 60, GameEngine.Height));
            engine.AddBody(w3);

            CollidableBody w4 = new Wall(new Rectangle(0, 0, GameEngine.Width, 30));
            engine.AddBody(w4);

            engine.Start();
        }


    }
}