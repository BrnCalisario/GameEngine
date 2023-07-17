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

    protected Dictionary<Keys, bool> KeyMap = new Dictionary<Keys, bool>()
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
}