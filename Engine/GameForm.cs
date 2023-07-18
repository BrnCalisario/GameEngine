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
            BasicEngine.New(this);

            this.engine = BasicEngine.Current;

            this.engine.Interval = 25;

            var player = new Player(new Rectangle(40, 40, 50, 50));
            player.SetColllisionMask(new Rectangle(0, player.Height / 2, player.Width, player.Height / 2));
            engine.AddBody(player);

            CollidableBody w = new Wall(new Rectangle(0, 0, 30, engine.Height));
            engine.AddBody(w);

            CollidableBody w2 = new Wall(new Rectangle(0, engine.Height - 30, engine.Width, 30));
            engine.AddBody(w2);

            CollidableBody w3 = new Wall(new Rectangle(400, 300, 60, engine.Height));
            engine.AddBody(w3);

            CollidableBody w4 = new Wall(new Rectangle(0, 0, engine.Width, 30));
            engine.AddBody(w4);

            engine.Start();
        }


    }
}