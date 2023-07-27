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

        var tileSet = new TileSet(14, 6, new Point(0, 0));
        tileSet.Box = tileSet.Box.AlignCenter(engine.Box);

        BasicEngine.Current.tileSet = tileSet;

        GenerateTables();

        Player player = new(new Rectangle().AlignCenter(tileSet.Box));
        engine.AddBody(player);

        OrderTab.New();
        OrderTab tab = OrderTab.Current;

        //TimeBar pb = new TimeBar(new(50, 0, 400, 30), TimeSpan.FromSeconds(1), true);
        //engine.AddBody(pb);

        //pb.OnFinish += delegate
        //{
        //    pb.Restart();
        //    pb.Descending = !pb.Descending;
        //};

        engine.AddBody(tab);

        engine.Start();
    }

    private void GenerateTables()
    {
        CornerBench corner1 = new(new Rectangle().AlignTopLeft(BasicEngine.Current.tileSet.Box));
        this.engine.AddBody(corner1);

        FoodBench fb3 = new(new Rectangle().AlignBesideRight(corner1.Box));
        engine.AddBody(fb3);

        FoodBench fb1 = new(new Rectangle().AlignBelow(corner1.Box), Direction.Right);
        engine.AddBody(fb1);

        Oven ov1 = new(new Rectangle().AlignBelow(fb1.CollisionMask.Box), Direction.Right);
        engine.AddBody(ov1);

        FoodBench tbx = new(new Rectangle().AlignBelow(ov1.CollisionMask.Box), Direction.Right);
        engine.AddBody(tbx);

        Oven ov2 = new(new Rectangle().AlignBelow(tbx.CollisionMask.Box), Direction.Right);
        engine.AddBody(ov2);

        FoodBench fb5 = new(new Rectangle().AlignBelow(ov2.CollisionMask.Box), Direction.Right);
        engine.AddBody(fb5);

        CornerBench corner2 = new(new Rectangle(0, 0, 48,48).AlignBottomLeft(BasicEngine.Current.tileSet.Box), Direction.Top);
        this.engine.AddBody(corner2);

        FoodBench fb7 = new(new Rectangle().AlignBesideRight(corner2.CollisionMask.Box), Direction.Top);
        engine.AddBody(fb7);

        FoodBox<Tomato> fbt = new(new Rectangle().AlignBesideRight(fb7.CollisionMask.Box), Direction.Top);
        this.engine.AddBody(fbt);

        FoodBox<Onion> obt = new(new Rectangle().AlignBesideRight(fbt.CollisionMask.Box), Direction.Top);
        this.engine.AddBody(obt);

        FoodBench fb6 = new(new Rectangle().AlignBesideRight(obt.CollisionMask.Box), Direction.Top);
        this.engine.AddBody(fb6);


        // COLUNA DA ESQUERDA /////////////////////

        CornerBench corner3 = new(new Rectangle(0, 0, 48, 48).AlignBesideRight(fb6.CollisionMask.Box), Direction.Right);
        this.engine.AddBody(corner3);

        FoodBench fb8 = new(new Rectangle(0, 0, 96, 48).AlignOver(corner3.CollisionMask.Box), Direction.Left);
        this.engine.AddBody(fb8);

        CuttingBoard fb9 = new(new Rectangle(0, 0, 96, 48).AlignOver(fb8.CollisionMask.Box), Direction.Left);
        this.engine.AddBody(fb9);

        FoodBench fb10 = new(new Rectangle(0, 0, 96, 48).AlignOver(fb9.CollisionMask.Box), Direction.Left);
        this.engine.AddBody(fb10);

        FoodBench fb11 = new(new Rectangle(0, 0, 96, 48).AlignOver(fb10.CollisionMask.Box), Direction.Left);
        this.engine.AddBody(fb11);

        Plate p = new Plate(new Rectangle());
        this.engine.AddBody(p);
        fb10.SetItem(p);


        FoodBench fb12 = new(new Rectangle(0, 0, 96, 48).AlignBesideRight(fb11.CollisionMask.Box), Direction.Right);
        this.engine.AddBody(fb12);

        FoodBench fb13 = new(new Rectangle(0, 0, 96, 48).AlignBesideRight(fb10.CollisionMask.Box), Direction.Right);
        this.engine.AddBody(fb13);

        FoodBench fb14 = new(new Rectangle(0, 0, 96, 48).AlignBesideRight(fb9.CollisionMask.Box), Direction.Right);
        this.engine.AddBody(fb14);

        FoodBench fb15 = new(new Rectangle(0, 0, 96, 48).AlignBesideRight(fb8.CollisionMask.Box), Direction.Right);
        this.engine.AddBody(fb15);

        CornerBench corner4 = new(new Rectangle(0, 0, 48, 48).AlignBesideRight(corner3.CollisionMask.Box), Direction.Top);
        this.engine.AddBody(corner4);

        Plate p2 = new Plate(new Rectangle());
        this.engine.AddBody(p2);
        fb13.SetItem(p2);

        // MEIO ///////////////////////

        FoodBench fb16 = new(new Rectangle(0, 0, 96, 48).AlignBesideRight(corner4.CollisionMask.Box), Direction.Top);
        this.engine.AddBody(fb16);

        CuttingBoard cb1 = new(new Rectangle().AlignBesideRight(fb16.CollisionMask.Box), Direction.Top);
        this.engine.AddBody(cb1);

        FoodBench fb17 = new(new Rectangle(0, 0, 96, 48).AlignBesideRight(cb1.CollisionMask.Box), Direction.Top);
        this.engine.AddBody(fb17);


        // COLUNA DA DIREITA /////////////////////

        CornerBench corner5 = new(new Rectangle(0, 0, 48, 48).AlignBesideRight(fb17.CollisionMask.Box), Direction.Right);
        this.engine.AddBody(corner5);

        CornerBench corner6 = new(new Rectangle(0, 0, 48, 48).AlignBesideRight(corner5.CollisionMask.Box), Direction.Top);
        this.engine.AddBody(corner6);
        
        FoodBench fb38 = new(new Rectangle(0, 0, 96, 48).AlignOver(corner5.CollisionMask.Box), Direction.Left);
        this.engine.AddBody(fb38);

        FoodBench fb39 = new(new Rectangle(0, 0, 96, 48).AlignOver(fb38.CollisionMask.Box), Direction.Left);
        this.engine.AddBody(fb39);

        FoodBench fb40 = new(new Rectangle(0, 0, 96, 48).AlignOver(fb39.CollisionMask.Box), Direction.Left);
        this.engine.AddBody(fb40);

        FoodBench fb41 = new(new Rectangle(0, 0, 96, 48).AlignOver(fb40.CollisionMask.Box), Direction.Left);
        this.engine.AddBody(fb41);

        Plate p3 = new Plate(new Rectangle());
        this.engine.AddBody(p3);
        fb40.SetItem(p3);


        FoodBench fb42 = new(new Rectangle().AlignBesideRight(fb41.CollisionMask.Box), Direction.Right);
        this.engine.AddBody(fb42);


        FoodBench fb43 = new(new Rectangle().AlignBesideRight(fb40.CollisionMask.Box), Direction.Right);
        this.engine.AddBody(fb43);

        FoodBench fb44 = new(new Rectangle().AlignBesideRight(fb39.CollisionMask.Box), Direction.Right);
        this.engine.AddBody(fb44);

        FoodBench fb45 = new(new Rectangle().AlignBesideRight(fb38.CollisionMask.Box), Direction.Right);
        this.engine.AddBody(fb45);

        Plate p4 = new Plate(new Rectangle());
        this.engine.AddBody(p4);
        fb43 .SetItem(p4);

        // DIRETA


        FoodBench fb18 = new(new Rectangle(0, 0, 96, 48).AlignBesideRight(corner6.CollisionMask.Box), Direction.Top);
        this.engine.AddBody(fb18);

        FoodBox<Meat> mbx = new(new Rectangle().AlignBesideRight(fb18.CollisionMask.Box), Direction.Top);
        this.engine.AddBody(mbx);

        FoodBox<Fish> fbx = new(new Rectangle().AlignBesideRight(mbx.CollisionMask.Box), Direction.Top);
        this.engine.AddBody(fbx);

        FoodBench fb19 = new(new Rectangle().AlignBesideRight(fbx.CollisionMask.Box), Direction.Top);
        this.engine.AddBody(fb19);

        CornerBench corner7 = new(new Rectangle(0, 0, 48, 48).AlignBesideRight(fb19.CollisionMask.Box), Direction.Right);
        this.engine.AddBody(corner7);

        // PAREDE DIREITA
        
        CornerBench corner8 = new(new Rectangle(0, 0, 48, 48).AlignTopRight(BasicEngine.Current.tileSet.Box), Direction.Left);
        this.engine.AddBody(corner8);

        FoodBench fb20 = new(new Rectangle().AlignBelow(corner8.CollisionMask.Box), Direction.Left);
        this.engine.AddBody(fb20);

        Oven ov3 = new(new Rectangle().AlignBelow(fb20.CollisionMask.Box), Direction.Left);
        this.engine.AddBody(ov3);

        FoodBench fb22 = new(new Rectangle().AlignBelow(ov3.CollisionMask.Box), Direction.Left);
        this.engine.AddBody(fb22);

        Oven ov4 = new(new Rectangle().AlignBelow(fb22.CollisionMask.Box), Direction.Left);
        this.engine.AddBody(ov4);

        FoodBench fb24 = new(new Rectangle().AlignBelow(ov4.CollisionMask.Box), Direction.Left);
        this.engine.AddBody(fb24);

        // PAREDE TOPO //////////////////

        FoodBench fb25 = new(new Rectangle().AlignBesideRight(corner1.CollisionMask.Box));
        this.engine.AddBody(fb25);

        FoodBench fb26 = new(new Rectangle().AlignBesideRight(fb25.CollisionMask.Box));
        this.engine.AddBody(fb26);

        FoodBench fb27 = new(new Rectangle().AlignBesideRight(fb26.CollisionMask.Box));
        this.engine.AddBody(fb27);

        SmallBench sm1 = new(new Rectangle().AlignBesideRight(fb27.CollisionMask.Box), Direction.Bottom);
        this.engine.AddBody(sm1);

        Trash trash = new(new Rectangle().AlignBesideRight(sm1.CollisionMask.Box));
        this.engine.AddBody(trash);

        FoodBench fb29 = new(new Rectangle().AlignBesideRight(trash.Box));
        this.engine.AddBody(fb29);

        FoodBench fb30 = new(new Rectangle().AlignBesideRight(fb29.CollisionMask.Box));
        this.engine.AddBody(fb30);

        CheckOut checkout = new CheckOut(new Rectangle().AlignBesideRight(fb30.CollisionMask.Box), Direction.Top);
        engine.AddBody(checkout);

        FoodBench fb32 = new(new Rectangle().AlignBesideRight(checkout.CollisionMask.Box));
        this.engine.AddBody(fb32);

        FoodBench fb33 = new(new Rectangle().AlignBesideRight(fb32.CollisionMask.Box));
        this.engine.AddBody(fb33);

        Trash trash2 = new(new Rectangle().AlignBesideRight(fb33.CollisionMask.Box));
        this.engine.AddBody(trash2);

        SmallBench sm2 = new(new Rectangle().AlignBesideRight(trash2.Box), Direction.Bottom);
        this.engine.AddBody(sm2);



        FoodBench fb35 = new(new Rectangle().AlignBesideRight(sm2.CollisionMask.Box));
        this.engine.AddBody(fb35);

        FoodBench fb36 = new(new Rectangle().AlignBesideRight(fb35.CollisionMask.Box));
        this.engine.AddBody(fb36);

        FoodBench fb37 = new(new Rectangle().AlignBesideRight(fb36.CollisionMask.Box));
        this.engine.AddBody(fb37);




        Pan pan2 = new(new Rectangle());
        ov2.SetItem(pan2);
        engine.AddBody(pan2);

        Pan pan1 = new(new Rectangle());
        engine.AddBody(pan1);
        ov1.SetItem(pan1);

        FryingPan pan3 = new(new Rectangle());
        engine.AddBody(pan3);
        ov3.SetItem(pan3);

        FryingPan pan4 = new(new Rectangle());
        engine.AddBody(pan4);
        ov4.SetItem(pan4);
    }
}
