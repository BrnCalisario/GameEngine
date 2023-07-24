using System.Runtime.Versioning;
using Engine.Tiles;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Engine;

using Engine.Kitchen;
using Engine.Resource;
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
        Resources.Load();

        this.engine = BasicEngine.Current;

        var tileSet = new TileSet(12, 6, new Point(0, 0));
        tileSet.Box = tileSet.Box.AlignCenter(engine.Box);

        BasicEngine.Current.tileSet = tileSet;

        CornerBench cb = new(new Rectangle().AlignTopLeft(tileSet.Box), Direction.Bottom);
        engine.AddBody(cb);

        FoodBench fb1 = new(new Rectangle().AlignBesideRight(cb.CollisionMask.Box), Direction.Bottom);
        engine.AddBody(fb1);

        var tomatoBox = new FoodBox<Tomato>(new Rectangle().AlignBesideRight(fb1.CollisionMask.Box));
        engine.AddBody(tomatoBox);



        FoodBench fb2 = new(new Rectangle().AlignBesideBottom(cb.CollisionMask.Box), Direction.Right);
        engine.AddBody(fb2);

        var player = new Player(new Rectangle(50, 50, 50, 50));
        player.Box = player.Box.AlignCenter(engine.Box);
        engine.AddBody(player); 


        engine.Start();
    }
}
