using Engine.Tiles;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Engine;

using Extensions;

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
        if (e.KeyCode == Keys.Escape)
            Application.Exit();

        engine.HandleKey(e, true);
    }

    private void GameForm_Load(object sender, EventArgs e)
    {
        BasicEngine.New(this);

        this.engine = BasicEngine.Current;


        var tileSet = new TileSet(12, 6, new Point(0, 0));
        tileSet.Box = tileSet.Box.AlignCenter(engine.Box);

        BasicEngine.Current.tileSet = tileSet;

        var rct1 = new Rectangle(new Point(tileSet.Location.X - 20, tileSet.Location.Y - 20), new Size(tileSet.BoxWidth + 40,20));
        var w1 = new Wall(rct1);

        var rct2 = new Rectangle(new Point(tileSet.Location.X - 20, tileSet.Location.Y - 20), new Size(20, tileSet.BoxHeight + 40));
        var w2 = new Wall(rct2);

        var rct3 = new Rectangle(new Point(tileSet.Location.X - 20, tileSet.Location.Y + tileSet.BoxHeight + 1), new Size(tileSet.BoxWidth + 40, 20));
        var w3 = new Wall(rct3);

        var rct4 = new Rectangle(new Point(tileSet.Location.X + tileSet.BoxWidth + 1, tileSet.Location.Y - 20), new Size(20, tileSet.BoxHeight + 40));
        var w4 = new Wall(rct4);

        engine.AddBody(w1);
        engine.AddBody(w2);
        engine.AddBody(w3);
        engine.AddBody(w4);

        

        ItemBox<Tomato> tomatoBox = new(new Rectangle(0, 0, 50, 50).AlignTopLeft(tileSet.Box));
        engine.AddBody(tomatoBox);

        ItemBox<Onion> onionBox = new(new Rectangle(0, 0, 50, 50).AlignBesideRight(tomatoBox.Box, 30));
        engine.AddBody(onionBox);

        ItemBox<Meat> meatBox = new(new Rectangle(0, 0, 50, 50).AlignBesideRight(onionBox.Box, 30));
        engine.AddBody(meatBox);

        ItemBox<Fish> fishBox = new(new Rectangle(0, 0, 50, 50).AlignBesideRight(meatBox.Box, 30));
        engine.AddBody(fishBox);

        Bench bench = new(new Rectangle(0,0, 100, 50).AlignBesideRight(fishBox.Box, 100));
        engine.AddBody(bench);

        Bench bench2 = new(new Rectangle(0,0, 100, 50).AlignBesideRight(bench.Box, 100));
        engine.AddBody(bench2);


        Trash trash = new(new Rectangle(0, 0, 64, 64).AlignMiddleCenter(tileSet.Box), 2, Pens.Purple);
        engine.AddBody(trash);

        var player = new Player(new Rectangle(50, 50, 50, 50));
        player.Box = player.Box.AlignCenter(engine.Box);
        engine.AddBody(player);


        engine.Start();
    }
}
