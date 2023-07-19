using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Engine;

using Tiles;

public class BasicEngine : GameEngine
{
    private BasicEngine(Form form) : base(form) { }
    private BasicEngine() { }

    private static BasicEngine current = null;
    public static BasicEngine Current { 
        get 
        {
            current ??= new BasicEngine();
            return current;
        } 
    }
    
    public static void New(Form form)
        => current = new BasicEngine(form);

    public static void New()
        => current = new BasicEngine();

    readonly List<IBody> RenderStack = new();

    public readonly List<Wall> Walls = new();
    public readonly List<Item> Items = new();

    public TileSet tileSet;

    public override void AddBody(IBody body)
    {
        if (body is Wall)
            Walls.Add(body as Wall);

        if(body is Item)
            Items.Add(body as Item);

        RenderStack.Add(body);
    }

    public override void Draw()
    {
        tileSet?.Draw(this.graphics);

        foreach (IBody body in RenderStack)
        {
            body.Draw(this.graphics);
        }

        this.graphics.DrawString($"FPS:{this.Fps}", SystemFonts.MenuFont, Brushes.Black, new Point(0, 0));
    }

    public override void Update()
    {
        foreach (IBody body in RenderStack)
        {
            body.Update();
        }
    }
}