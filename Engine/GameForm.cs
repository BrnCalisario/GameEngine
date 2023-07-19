using Engine.Tiles;
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


            var tileSet = new TileSet(16, 8, new Point(0, 0));
            tileSet.Box = tileSet.Box.AlignCenter(engine.Box);

            BasicEngine.Current.tileSet = tileSet;


            var player = new Player(new Rectangle(50, 50, 50, 50));
            player.Box = player.Box.AlignCenter(engine.Box);

            engine.AddBody(player);

            engine.Start();
        }
    }
}