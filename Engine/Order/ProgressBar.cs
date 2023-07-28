

using System;
using System.Drawing;

namespace Engine;

public abstract class ProgressBar : Body
{
    public ProgressBar(Rectangle box) : base(box, Pens.Black)
    {
        ProgressRectangle = new Rectangle(box.Location, new(1, box.Height));
    }

    protected Rectangle ProgressRectangle { get; set; }

    protected Pen ProgressPen { get; set; } = Pens.Green;

    public int MaxValue { get; set; } = 100;
    public int MinValue { get; set; } = 0;

    public override void Draw(Graphics g)
    {
        g.FillRectangle(ProgressPen.Brush, ProgressRectangle);
        g.DrawRectangle(Pen, Box);
    }

    public abstract void Restart();


    public EventHandler OnFinish;
}

public class TimeBar : ProgressBar
{
    public TimeBar(Rectangle box, TimeSpan time, bool descending = false, bool start = false) 
        : base(box)
    {

        Interval = time;
        this.Descending = descending;

        if (start) this.Start();
    }

    private DateTime StartProgress { get; set; } = default;

    private TimeSpan Interval { get; set; }

    public bool Descending { get; set; }

    public bool Complete { get; set; } = true;

    public bool Stopped { get; set; } = false;  

    public bool ChangeColor { get; set; } = true;

    public void Start()
    {
        if (!Complete || Stopped) return;

        StartProgress = DateTime.Now;
        Complete = false;
    }

    public void Reset()
    {
        StartProgress = default;
        Complete = false;
        Stopped = false;
    }

    public void Stop()
    {
        Complete = true;
        Stopped = true;
    }

    public override void Update()
    {
        if (Complete) return;

        if (StartProgress == default)
            StartProgress = DateTime.Now;

        CalculateWidth();
    }

    public void UpdateLocation(Point point)
    {
        this.Box = new Rectangle(point, Box.Size);
        this.ProgressRectangle = new Rectangle(this.Box.Location, ProgressRectangle.Size);
    }


    private void CalculateWidth()
    {
        var diff = DateTime.Now - StartProgress;

        if (diff >= Interval)
        {
            this.Complete = true;

            var temp = Descending ? 0 : Box.Width;
            this.ProgressRectangle = new Rectangle(ProgressRectangle.Location, new(temp, ProgressRectangle.Height));
            OnFinish?.Invoke(this, EventArgs.Empty);

            return;
        }

        var percentange = diff / Interval;

        if(ChangeColor)
        {
            if (percentange > 0.5)
                this.ProgressPen = Pens.Orange;

            if (percentange > 0.75)
                this.ProgressPen = Pens.Red;

            if (percentange < 0.5)
                this.ProgressPen = Pens.Green;

        }      
        
        int wid = (int)(Box.Width * percentange);
        wid = Descending ? Box.Width - wid : wid;

        this.ProgressRectangle = new Rectangle(ProgressRectangle.Location, new(wid, ProgressRectangle.Height));
    }

    public override void Restart()
    {
        this.Complete = false;
        StartProgress = DateTime.Now;
    }
}


