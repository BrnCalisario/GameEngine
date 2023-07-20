using Engine.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine;

public abstract class Item : CollidableBody
{
    public Item(Rectangle box) : base(box)
    {
        var collMask = new Rectangle(0, 0,  box.Width * 2, box.Height * 2);
        collMask = collMask.AlignCenter(box);

        var relativePos = new Point(collMask.X - box.X, collMask.Y - box.Y);
        collMask = new Rectangle(relativePos, collMask.Size);

        this.SetColllisionMask(collMask);
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

    public void GetHold(Player p)
    {
        if (Player is not null)
            return;

        this.Player = p;
        p.holdingItem = this;
        LastInteraction = DateTime.Now;
    }

    public void GetReleased()
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
        this.Player = null;
        LastInteraction = DateTime.Now;
    }

    public void SetRelativePosition(Direction direction, int offset = 30, int offsetY = 30)
    {        
        // if(Direction == direction)
        //     return;

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
}



public class Tomato : Item
{
    public Tomato(Point p) : base(new Rectangle(p, new(20, 20)))
    {
        this.Pen = new Pen(Color.Red);
    }
}