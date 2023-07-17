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

public class MyEngine : GameEngine
{
    public MyEngine(Form form) : base(form) 
    {

    }

    CollidableBody bot1 = new Bot(new Rectangle(20, 20, 50, 50), new Pen(Color.Red));
    CollidableBody bot2 = new Bot(new Rectangle(300, 300, 50, 50), new Pen(Color.Red));

    bool colliding = false;

    public override void Draw()
    {
        //graphics.DrawString($"{colliding}", SystemFonts.MenuFont, Brushes.Black, new Point(20, 20));
        bot1.Draw(this.graphics);
        bot2.Draw(this.graphics);
    }

    public override void Update()
    {
        bot1.Update();
        bot2.Update();

        if (bot1.IsColling(bot2))
        {
            bot1.Pen = new Pen(Color.Green, 3);
            bot2.Pen = new Pen(Color.Green, 3);
        }
        else
        {
            bot1.Pen = new Pen(Color.Black, 1);
            bot2.Pen = new Pen(Color.Black, 1);
        }

    }
}