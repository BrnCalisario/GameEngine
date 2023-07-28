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
    protected GameForm ParentForm { get; set; } = null;
    protected Graphics graphics { get; set; }
    protected PictureBox pb { get; set; }
    protected Bitmap bmp { get; set; }

    private Queue<DateTime> queue = new();

    public Rectangle Box => new Rectangle(0, 0, pb.Width, pb.Height); 

    public int Fps { get; private set; } 

    public int Width { get; set; }
    public int Height { get; set; }

    public readonly Dictionary<Keys, bool> KeyMap = new()
    {
        { Keys.W, false},
        { Keys.S, false},
        { Keys.A, false},
        { Keys.D, false},
        { Keys.E, false},
        { Keys.F, false},
        { Keys.V, false},
        { Keys.C, false},
    };

    readonly Timer tm = new();
    public int Interval
    {
        get => tm.Interval;
        set
        {
            tm.Interval = value;
        }
    }

    private bool loaded = false;
    private bool running = false;

    public GameEngine() { }
    public GameEngine(GameForm form)
    {
        this.ParentForm = form;
        pb = new PictureBox
        {
            Dock = DockStyle.Fill
        };
        ParentForm.Controls.Add(pb);

        Width = pb.Width;
        Height = pb.Height;

        this.Interval = 20;
    }

    public void SetParentForm(GameForm form)
    {
        this.ParentForm = form;
        pb = new PictureBox
        {
            Dock = DockStyle.Fill
        };
        form.Controls.Add(pb);

        Width = pb.Width;
        Height = pb.Height;
    }

    public bool IsParentSet()
        => ParentForm is not null;

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
        if (!IsParentSet())
            throw new Exception("Parent form not set");

        if (!loaded)
            this.Load();

        running = true;
        tm.Start();
    }

    public virtual void Stop()
    {
        running = false;
        loaded = false;
    }

    public virtual void Load()
    {
        bmp = new Bitmap(pb.Width, pb.Height);
        graphics = Graphics.FromImage(bmp);
        pb.Image = bmp;

        Application.Idle += delegate
        {
            while (running)
            {
                this.update();
                Application.DoEvents();
            }
            //this.OnEnd();
            
        };
        this.loaded = true;
    }

    public void HandleKey(KeyEventArgs e, bool isDown)
    {
        if (KeyMap.ContainsKey(e.KeyCode))
            KeyMap[e.KeyCode] = isDown;
    }

    public abstract void AddBody(IBody body);

    public abstract void OnEnd();
}