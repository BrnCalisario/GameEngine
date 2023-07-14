using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SpriteExercise
{
    public partial class Form1 : Form
    {
        public static int FormWidth { get; set; }
        public static int FormHeight { get; set; }

        PictureBox pb = new PictureBox();
        Bitmap bmp = null;
        Graphics g = null;

        Player player;

        public static Dictionary<Keys, bool> KeyMap { get; set; } = new Dictionary<Keys, bool>()
        {
            { Keys.W, false },
            { Keys.S, false },
            { Keys.D, false },
            { Keys.A, false },
        };

        public Form1()
        {
            InitializeComponent();
            this.Text = "Sonic Running";

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.BackColor = Color.LightGray;
            this.TransparencyKey = Color.LightGray;

            this.Width = 1200;
            this.Height = 800;

            Form1.FormWidth = this.Width;
            Form1.FormHeight = this.Height;

            pb.Dock = DockStyle.Fill;
            this.Controls.Add(pb);

            timer1.Interval = 20;
        }

        private void OnFormKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }

            if (KeyMap.ContainsKey(e.KeyCode))
                KeyMap[e.KeyCode] = true;
        }

        private void OnFormKeyUp(object sender, KeyEventArgs e)
        {
            if (KeyMap.ContainsKey(e.KeyCode))
                KeyMap[e.KeyCode] = false;
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            bmp = new Bitmap(pb.Width, pb.Height);
            g = Graphics.FromImage(bmp);
            pb.Image = bmp;

            player = new Player();

            timer1.Start();
        }

        private void gameTick(object sender, System.EventArgs e)
        {
            g.Clear(Color.LightGray);

            player.Update();
            player.Draw(g);

            pb.Refresh();
        }
    }
}