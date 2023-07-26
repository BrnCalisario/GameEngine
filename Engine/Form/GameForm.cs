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

        GenerateTables();

        Player player = new(new Rectangle().AlignCenter(tileSet.Box));
        engine.AddBody(player);

        engine.Start();
    }

    private void GenerateTables()
    {
        CornerBench corner1 = new(new Rectangle().AlignTopLeft(BasicEngine.Current.tileSet.Box));
        this.engine.AddBody(corner1);

        FoodBench fb3 = new(new Rectangle().AlignBesideRight(corner1.Box));
        engine.AddBody(fb3);

        FoodBench fb1 = new(new Rectangle().AlignBesideBottom(corner1.Box), Direction.Right);
        engine.AddBody(fb1);

        Oven ov1 = new(new Rectangle().AlignBesideBottom(fb1.CollisionMask.Box), Direction.Right);
        engine.AddBody(ov1);

        FoodBench tbx = new(new Rectangle().AlignBesideBottom(ov1.CollisionMask.Box), Direction.Right);
        engine.AddBody(tbx);

        Oven ov2 = new(new Rectangle().AlignBesideBottom(tbx.CollisionMask.Box), Direction.Right);
        engine.AddBody(ov2);

        FoodBench fb5 = new(new Rectangle().AlignBesideBottom(ov2.CollisionMask.Box), Direction.Right);
        engine.AddBody(fb5);

        CornerBench corner2 = new(new Rectangle(0, 0, 48,48).AlignBottomLeft(BasicEngine.Current.tileSet.Box), Direction.Top);
        this.engine.AddBody(corner2);

        FoodBox<Tomato> fbt = new(new Rectangle().AlignBesideRight(corner2.Box), Direction.Top);
        this.engine.AddBody(fbt);

        FoodBox<Onion> obt = new(new Rectangle().AlignBesideRight(fbt.CollisionMask.Box), Direction.Top);
        this.engine.AddBody(obt);

        FoodBench fb6 = new(new Rectangle().AlignBesideRight(obt.CollisionMask.Box), Direction.Top);
        this.engine.AddBody(fb6);
        


        Pan pan2 = new(new Rectangle());
        ov2.SetItem(pan2);
        engine.AddBody(pan2);



        Pan pan1 = new(new Rectangle());
        engine.AddBody(pan1);
        fb2.SetItem(fp);

        
    }
}
