using System.Runtime.Versioning;
using Engine.Tiles;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Engine;

using Engine.Kitchen;
using Engine.Resource;
using Engine.Sprites;
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

        CornerBench cb = new(new Rectangle().AlignTopLeft(tileSet.Box));
        engine.AddBody(cb);

        FoodBench fb = new(new Rectangle().AlignBesideBottom(cb.CollisionMask.Box), Direction.Right);
        engine.AddBody(fb);

        FoodBench fb2 = new(new Rectangle().AlignBesideRight(cb.CollisionMask.Box));
        engine.AddBody(fb2);

        FoodBox<Tomato> tbx = new FoodBox<Tomato>(new Rectangle().AlignBesideRight(fb2.CollisionMask.Box));
        engine.AddBody(tbx);

        Oven ov = new(new Rectangle().AlignBesideRight(tbx.CollisionMask.Box));
        engine.AddBody(ov);

        FoodBox<Onion> obx = new FoodBox<Onion>(new Rectangle().AlignBesideRight(ov.CollisionMask.Box));
        engine.AddBody(obx);

        Oven ov2 = new(new Rectangle().AlignBesideBottom(fb.CollisionMask.Box), Direction.Right);
        engine.AddBody(ov2);



        var player = new Player(new Rectangle(50, 50, 50, 50));
        player.Box = player.Box.AlignCenter(engine.Box);
        engine.AddBody(player);


        Pan pan = new(new Rectangle().AlignCenter(tileSet.Box));
        engine.AddBody(pan);

        Plate plate = new Plate(new Rectangle().AlignCenter(tileSet.Box), Sprites.PlateTypes.TomatoPlate);
        engine.AddBody(plate);


        engine.Start();
    }
}
