

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

    protected Pen ProgressPen { get; set; }

    public int MaxValue { get; set; } = 100;
    public int MinValue { get; set; } = 0;

    public override void Draw(Graphics g)
    {
        g.FillRectangle(Brushes.Green, ProgressRectangle);
        g.DrawRectangle(Pen, Box);
    }

    public abstract void Restart();


    public EventHandler OnFinish;
}

public class TimeBar : ProgressBar
{
    public TimeBar(Rectangle box, TimeSpan time, bool descending = false) 
        : base(box)
    {

        Interval = time;
        this.Descending = descending;
    }

    private DateTime StartProgress { get; set; } = default;

    private TimeSpan Interval { get; set; }

    public bool Descending { get; set; }

    public bool Complete { get; set; }


    public override void Update()
    {
        if (Complete) return;

        if (StartProgress == default)
            StartProgress = DateTime.Now;

        CalculateWidth();
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


