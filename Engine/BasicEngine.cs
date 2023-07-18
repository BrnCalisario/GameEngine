using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine;

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

    public readonly List<CollidableBody> Walls = new List<CollidableBody>();

    public override void AddBody(IBody body)
    {
        if (body is Wall)
            Walls.Add(body as Wall);

        RenderStack.Add(body);
    }

    public override void Draw()
    {

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