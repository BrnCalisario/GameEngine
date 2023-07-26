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





        Player player = new Player(new Rectangle().AlignCenter(tileSet.Box));
        engine.AddBody(player);

        engine.Start();
    }

    private void GenerateTables()
    {
        CornerBench corner1 = new(new Rectangle().AlignTopLeft(BasicEngine.Current.tileSet.Box));
        this.engine.AddBody(corner1);

        FoodBox<Tomato> tmBox = new(new Rectangle().AlignBesideRight(corner1.Box));
        engine.AddBody(tmBox);

        FoodBench fb1 = new(new Rectangle().AlignBesideBottom(corner1.Box), Direction.Right);
        engine.AddBody(fb1);

        Oven fb2 = new(new Rectangle().AlignBesideBottom(fb1.CollisionMask.Box), Direction.Right);
        engine.AddBody(fb2);


        CuttingBoard fb3 = new(new Rectangle().AlignBesideBottom(fb2.CollisionMask.Box), Direction.Right);
        engine.AddBody(fb3);

        Oven fb4 = new(new Rectangle().AlignBesideBottom(fb3.CollisionMask.Box), Direction.Right);
        engine.AddBody(fb4);

        FoodBench fb5 = new(new Rectangle().AlignBesideBottom(fb4.CollisionMask.Box), Direction.Right);
        engine.AddBody(fb5);

        CornerBench corner2 = new(new Rectangle(0, 0, 48,48).AlignBottomLeft(BasicEngine.Current.tileSet.Box), Direction.Top);
        this.engine.AddBody(corner2);

        Pan pan2 = new(new Rectangle());
        fb4.SetItem(pan2);
        engine.AddBody(pan2);

        FryingPan fp = new(new Rectangle());
        engine.AddBody(fp);


        Pan pan1 = new(new Rectangle());
        engine.AddBody(pan1);
        fb2.SetItem(fp);

        
    }
}
