using Engine.Extensions;
using Engine.Sprites;
using Engine.Sprites.FoodSprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine;

using static ProjectPaths;

public abstract class Interactable : CollidableBody
{
    protected Interactable(Rectangle box, float scale = 2, Pen pen = null) : base(box, pen)
    {
        var scaledSize = new Size((int)(box.Width * scale),(int) (box.Height * scale));


        var collMask = new Rectangle(new(0, 0), scaledSize);
        collMask = collMask.AlignCenter(box);

        var relativePos = new Point(collMask.X - box.X, collMask.Y - box.Y);
        collMask = new Rectangle(relativePos, collMask.Size);

        SetColllisionMask(collMask);
    }

    public abstract void Interact(Player p);
}

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
        this.Player.holdingItem = null;
        this.Player = null;
    }
}


public class Food : Item
{
    public Food(Rectangle r, FoodTypes type) : base(new Rectangle(r.Location, new(30, 30)))
    {
        Loader = new FoodSpriteLoader();
        SpriteStream = Loader.GetAnimation(type);
    }

    public Food(Point point, FoodTypes type) : base(point)
    {
        Loader = new FoodSpriteLoader();
        SpriteStream = Loader.GetAnimation(type);
    }

    Image foodImage = Image.FromFile(AssetsPath + "food3x.png");
    SpriteLoader<FoodTypes> Loader { get; set; }
     
    SpriteStream SpriteStream { get; set; }

    public override void Draw(Graphics g)
    {
        var c = SpriteStream.Next();

        g.DrawImage(
            foodImage,
            this.Box,
            c.X,
            c.Y,
            c.Width,
            c.Height,
            GraphicsUnit.Pixel
            );
    }
}

public class Tomato : Food
{
    public Tomato(Rectangle r) : base(r, FoodTypes.Tomato)
    {
    }

    public Tomato(Point point) : base(point, FoodTypes.Tomato)
    {
    }
}

public class Onion : Food
{
    public Onion(Rectangle r) : base(r, FoodTypes.Onion)
    {
    }

    public Onion(Point point) : base(point, FoodTypes.Onion)
    {
    }
}


public class Meat : Food
{
    public Meat(Rectangle r) : base(r, FoodTypes.Meat)
    {
    }

    public Meat(Point point) : base(point, FoodTypes.Meat)
    {
    }
}

public class Fish : Food
{
    public Fish(Rectangle r) : base(r, FoodTypes.Fish)
    {
    }

    public Fish(Point point) : base(point, FoodTypes.Fish)
    {
    }
}



