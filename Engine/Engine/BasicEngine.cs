using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Engine;

using Engine.Extensions;
using System;
using System.Linq;
using Tiles;

public class BasicEngine : GameEngine
{
    private BasicEngine(GameForm form) : base(form) { }
    private BasicEngine() { }

    private static BasicEngine current = null;
    public static BasicEngine Current { 
        get 
        {
            current ??= new BasicEngine();
            return current;
        } 
    }
    
    public static void New(GameForm form)
        => current = new BasicEngine(form);

    public static void New()
        => current = new BasicEngine();

    public List<IBody> RenderStack = new();

    public readonly List<CollidableBody> Walls = new();
    public readonly List<Interactable> Interactables = new();
    public readonly List<Trash> Trashes = new();

    public TimeSpan GameTime { get; set; } = TimeSpan.FromMinutes(3);

    public DateTime GameStart { get; set; } 

    public TimeSpan TimeDisplay { get; set; } 

    public Player Player = null;

    public TileSet tileSet { get; set; }

    public int Points { get; set; } = 0;

    public override void Load()
    {
        GameStart = DateTime.Now;

        base.Load();

        this.RenderStack = RenderStack.OrderBy(r =>
        {
            if (r is Bench)
                return 0;
            if (r is CookingTool)
                return 1;
            if (r is Player)
                return 2;
            return 3;
        }).ToList();

        OrderTab.Current.GetRandomOrder();
    }

    public override void AddBody(IBody body)
    {
        if (body is IUnwalkable)
            Walls.Add(body as CollidableBody);

        if(body is Interactable)
            Interactables.Add(body as Interactable);
        
        if(body is Trash)
            Trashes.Add(body as Trash);

        if(body is Player)
            this.Player = body as Player;

        RenderStack.Add(body);
    }

    public override void Draw()
    {
        tileSet?.Draw(this.graphics);

        foreach (IBody body in RenderStack)
        {
            body.Draw(this.graphics);
        }
       
        var rect = new Rectangle(0, 0, 600, 40).AlignBottomLeft(this.Box);

        var font = new Font("Arial", 32, FontStyle.Bold, GraphicsUnit.Pixel);

        this.graphics.DrawString($"Points:{this.Points}", font, Brushes.Black, rect);

        rect.Y -= 40;

        var timeString = (GameTime - TimeDisplay).ToString();

        timeString = timeString.Substring(3, 5);
        this.graphics.DrawString($"Time Remaning:{timeString}", font, Brushes.Black, rect);
    }

    public override void Update()
    {
        TimeDisplay = DateTime.Now - GameStart;

        if(TimeDisplay > GameTime)
        {
            return;
        }    


        for(int i = 0; i < RenderStack.Count; i++)
        {
            RenderStack[i].Update();
        }
    }

    //public override void OnEnd()
    //{
        //var result =
        //        MessageBox.Show(
        //            $"Pontos: {this.Points}\nDeseja jogar novamente ?", 
        //            "Fim de Jogo", 
        //            MessageBoxButtons.YesNo, 
        //            MessageBoxIcon.Information);

        //if(result == DialogResult.Yes)
        //{
        //    this.ParentForm.Reload();
        //    this.Stop();
        //    this.Load();
        //    this.Start();
        //}

        //if(result == DialogResult.No)
        //{
        //    Application.Exit();
        //}
    //}
}