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

    public Player PlayerParent = null;

    public FoodBench BenchParent = null;

    public bool BeingHold => PlayerParent != null;
    public bool InBench => BenchParent != null;

    public Direction Direction { get; set; } = Direction.Bottom;

    public override void Update()
    {
        if (BeingHold )
        {
            this.Box = this.Box.AlignCenter(PlayerParent.Box);
            SetRelativePosition(PlayerParent.CurrentDirection);
        }

        this.CollisionMask.UpdatePoint(this.Box.Location);
    }

    private void GetHold(Player p)
    {
        if (PlayerParent is not null)
            return;

        if (p.IsHolding)
            return;

        this.AssignPlayer(p);

        LastInteraction = DateTime.Now;
    }

    private void GetReleased()
    {
        Point newPos = Direction switch
        {
            Direction.Top => new Point(Left, PlayerParent.Top),
            Direction.Bottom => new Point(Left, PlayerParent.Bottom),
            Direction.Right => new Point(PlayerParent.CollisionMask.Right, Y + PlayerParent.CollisionMask.Width / 2),
            Direction.Left => new Point(PlayerParent.CollisionMask.Left - Width, Y + PlayerParent.CollisionMask.Width / 2),
            _ => new Point(Left, PlayerParent.Bottom)
        };

        this.Box = new Rectangle(newPos, Box.Size);
        
        UnassignPlayer();
        LastInteraction = DateTime.Now;
    }

    public void SetRelativePosition(Direction direction, int offset = 30, int offsetY = 30)
    {
        Direction = direction;

        if (PlayerParent is null) return;

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
        if (PlayerParent is null)
            this.GetHold(p);
        else
            this.GetReleased();
    }

    public void Dispose()
    {
        BasicEngine.Current.Interactables.Remove(this);
        BasicEngine.Current.RenderStack.Remove(this);

        if (PlayerParent is not null)
        {
            UnassignPlayer();
        }
    }

    public void UnassignPlayer()
    {
        if (this.PlayerParent is null) return;

        this.PlayerParent.holdingItem = null;
        this.PlayerParent = null;
    }

    public void UnassignBench()
    {
        if (this.BenchParent is null) return;

        this.BenchParent.UnassignItem();
        this.BenchParent = null;
    }

    public void AssignPlayer(Player p)
    {
        if (this.PlayerParent is not null) return;

        this.PlayerParent = p;
        this.PlayerParent.AssignItem(this);
    }

    public void AssignBench(FoodBench fb)
    {
        if (this.BenchParent is not null) return;

        this.BenchParent = fb;
        fb.AssignItem(this);
    }
}
