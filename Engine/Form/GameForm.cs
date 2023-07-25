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

        CuttingBoard fb = new(new Rectangle().AlignBesideBottom(cb.CollisionMask.Box), Direction.Right);
        engine.AddBody(fb);

        FoodBench fb2 = new(new Rectangle().AlignBesideRight(cb.CollisionMask.Box));
        engine.AddBody(fb2);

        FoodBox<Tomato> tbx = new(new Rectangle().AlignBesideRight(fb2.CollisionMask.Box));
        engine.AddBody(tbx);

        Oven ov = new(new Rectangle().AlignBesideRight(tbx.CollisionMask.Box));
        engine.AddBody(ov);

        FoodBox<Onion> obx = new(new Rectangle().AlignBesideRight(ov.CollisionMask.Box));
        engine.AddBody(obx);

        FoodBox<Meat> mbx = new(new Rectangle().AlignBesideRight(obx.CollisionMask.Box));
        engine.AddBody(mbx);


        FoodBox<Fish> fbx = new(new Rectangle().AlignBesideRight(mbx.CollisionMask.Box));
        engine.AddBody(fbx);

        CuttingBoard cbo = new(new Rectangle().AlignBesideRight(fbx.CollisionMask.Box));
        engine.AddBody(cbo);

        Oven ov2 = new(new Rectangle().AlignBesideBottom(fb.CollisionMask.Box), Direction.Right);
        engine.AddBody(ov2);

        FoodBench fb3 = new(new Rectangle().AlignBesideBottom(ov2.CollisionMask.Box), Direction.Right);
        engine.AddBody(fb3);

        var player = new Player(new Rectangle(50, 50, 50, 50));
        player.Box = player.Box.AlignCenter(engine.Box);
        engine.AddBody(player);


        Pan pan = new(new Rectangle());
        engine.AddBody(pan);
        ov2.SetItem(pan);

        Pan pan2 = new(new Rectangle().AlignCenter(ov.Box));
        engine.AddBody(pan2);
        ov.SetItem(pan2);

        Trash t = new(new Rectangle().AlignBesideBottom(fb3.CollisionMask.Box));
        engine.AddBody(t);

        //Plate plate = new Plate(new Rectangle().AlignCenter(tileSet.Box), Sprites.PlateTypes.VoidPlate);
        //engine.AddBody(plate);

        Plate plateTomato = new Plate(new Rectangle().AlignCenter(tileSet.Box), Sprites.PlateTypes.VoidPlate);
        engine.AddBody(plateTomato);

        //Plate plateOnion = new Plate(new Rectangle().AlignCenter(tileSet.Box), Sprites.PlateTypes.OnionPlate);
        //engine.AddBody(plateOnion);

        //Plate plateTomOnion = new Plate(new Rectangle().AlignCenter(tileSet.Box), Sprites.PlateTypes.Tom_OnionPlate);
        //engine.AddBody(plateTomOnion);


        engine.Start();
    }
}
