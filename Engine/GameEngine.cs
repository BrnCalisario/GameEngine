using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Engine;

public interface IGameEngine
{
    void Load();
    void Update();
    void Draw();
    void Start();
    void Stop();
}

public abstract class GameEngine : IGameEngine
{
    protected Graphics graphics { get; set; }
    protected PictureBox pb { get; set; }
    protected Bitmap bmp { get; set; }   

    public static int Width { get; set; }
    public static int Height { get; set; }

    public readonly static Dictionary<Keys, bool> KeyMap = new Dictionary<Keys, bool>()
    {
        { Keys.W, false},
        { Keys.S, false},
        { Keys.A, false},
        { Keys.D, false}
    };

    Timer tm = new();

    public int Interval
    {
        get => tm.Interval;
        set
        {
            tm.Stop();
            tm.Interval = value;
            tm.Start();
        }
    }

    private bool loaded = false;
    private bool running = false;

    public GameEngine(Form form)
    {
        pb = new PictureBox
        {
            Dock = DockStyle.Fill
        };
        form.Controls.Add(pb);

        GameEngine.Width = pb.Width;
        GameEngine.Height = pb.Height;
    }

    private void update()
    {
        if (!running)
            return;

        this.Update();
        this.draw();
    }

    public abstract void Update();

    private void draw()
    {
        graphics.Clear(Color.White);

        this.Draw();

        pb.Refresh();
    }

    public abstract void Draw();

    public virtual void Start()
    {

        if (!loaded)
            this.Load();

        running = true;
        tm.Start();
    }

    public virtual void Stop()
    {
        running = false;
        tm.Stop();
    }

    public void Load()
    {
        bmp = new Bitmap(pb.Width, pb.Height);
        graphics = Graphics.FromImage(bmp);
        pb.Image = bmp;

        tm.Tick += delegate
        {
            this.update();
        };


        this.loaded = true;
    }

    public void HandleKey(KeyEventArgs e, bool isDown)
    {
        if (KeyMap.ContainsKey(e.KeyCode))
            KeyMap[e.KeyCode] = isDown;
    }

    public abstract void AddBody(IBody body);
}

public class BasicEngine : GameEngine
{
    public BasicEngine(Form form) : base(form)
    {
        RenderStack.Add(player);
    }

    readonly List<IBody> RenderStack = new();
    public readonly List<Wall> walls = new List<Wall>();

    public Player player = new Player(new Rectangle(40, 40, 50, 50), new Pen(Color.Red, 1));

    public override void AddBody(IBody body)
    {
        if (body is Wall)
            walls.Add(body as Wall);

        RenderStack.Add(body);
    }

    public override void Draw()
    {
        foreach(IBody body in RenderStack)
        {
            body.Draw(this.graphics);
        }
    }

    public override void Update()
    {       
        foreach(IBody body in RenderStack)
        {
            body.Update();
        }
    }
}