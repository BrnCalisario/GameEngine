using System;
using System.Drawing;


namespace Engine;

using Extensions;

public abstract class Item : Interactable, IDisposable
{
    public Item(Rectangle box) : base(box)
    {

    }

    public Item(Point p) : base(new Rectangle(p, new Size(20, 20)))
    {

    }

    public Item() : base(new Rectangle(1, 1, 1, 1))
    {

    }

    private DateTime LastInteraction = DateTime.Now;
    private readonly TimeSpan InteractionDelay = TimeSpan.FromSeconds(0.25);

    private bool CanInteract()
    {
        var diff = DateTime.Now - LastInteraction;
        return diff >= InteractionDelay;
    }

    public bool Interactable => CanInteract();

    public Player Player { get; private set; } = null;
    public bool BeingHold => Player != null;

    public Direction Direction { get; set; } = Direction.Bottom;

    public override void Draw(Graphics g)
    {
        g.FillEllipse(Pen.Brush, Box);

        if (CollisionMask is not null)
            g.DrawEllipse(new Pen(Color.Black), this.CollisionMask.Box);
    }

    public override void Update()
    {
        if(BeingHold)        
        {
            this.Box = this.Box.AlignCenter(Player.Box);
            SetRelativePosition(Player.CurrentDirection);
        }
        
        this.CollisionMask.UpdatePoint(this.Box.Location);
    }

    private void GetHold(Player p)
    {
        if (Player is not null)
            return;

        if(p.IsHolding)
            return;

        this.Player = p;
        p.holdingItem = this;
        LastInteraction = DateTime.Now;
    }

    private void GetReleased()
    {
        Point newPos = Direction switch 
        {
            Direction.Top => new Point(Left , Player.Top),
            Direction.Bottom => new Point(Left, Player.Bottom),
            Direction.Right => new Point(Player.CollisionMask.Right, Y + Player.CollisionMask.Width / 2),
            Direction.Left => new Point(Player.CollisionMask.Left - Width, Y + Player.CollisionMask.Width / 2),
            _ => new Point(Left, Player.Bottom)
        };

        this.Box = new Rectangle(newPos, Box.Size);
        this.Player.holdingItem = null;
        this.Player = null;
        LastInteraction = DateTime.Now;
    }

    public void SetRelativePosition(Direction direction, int offset = 30, int offsetY = 30)
    {        
        Direction = direction;

        if(Player is null) return;

        Point relativePos = direction switch
        {
            Direction.Top => new Point(X, Y - offset),
            Direction.Bottom => new Point(X, Y + offset),
            Direction.Left => new Point(X - offset, Y + offset / 2),
            Direction.Right => new Point(X + offset, Y + offset / 2),
            _ => new Point(X, Y)
        };

        this.Box = new Rectangle(relativePos, Box.Size);
    }

    public Rectangle SetRectRelativePosition(Rectangle rect, Direction direction, int offset = 30, int offsetY = 30)
    {
        Direction = direction;

        //if (Player is null) return;

        Point relativePos = direction switch
        {
            Direction.Top => new Point(X, Y - offset),
            Direction.Bottom => new Point(X, Y + offset),
            Direction.Left => new Point(X - offset, Y + offset / 2),
            Direction.Right => new Point(X + offset, Y + offset / 2),
            _ => new Point(X, Y)
        };

        rect = new Rectangle(relativePos, rect.Size);
        return rect;
    }

    public override void Interact(Player p)
    {
        if(Player is null)
            this.GetHold(p);
        else
            this.GetReleased();
    }

    public void Dispose()
    {
        BasicEngine.Current.Interactables.Remove(this);
        BasicEngine.Current.RenderStack.Remove(this);

        if(Player is not null)
        {
            this.Player.holdingItem = null;
            this.Player = null;
        }
    }
}
