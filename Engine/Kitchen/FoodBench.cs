
using System.Runtime.Versioning;
using System.Drawing;


namespace Engine;

using static ProjectPaths;
using Extensions;
using Sprites;
using Engine.Resource;
using System.ComponentModel;
using System.Drawing.Drawing2D;

public abstract class Bench : Interactable, IUnwalkable
{
    protected Bench(Rectangle box, float scale = 1, Pen pen = null, Direction dir = Direction.Bottom)
        : base(box, scale, pen)
    {
        this.benchSpriteLoader = new BenchSpriteLoader();
        SetDirection(dir);
    }


    Image BenchImage = Resources.BenchImage;

    protected readonly SpriteLoader<BenchTypes> benchSpriteLoader;
    protected Sprite BenchSprite { get; set; }


    public Direction Direction { get; set; }


    private void SetDirection(Direction dir)
    {
        this.Direction = dir;

        if (dir == Direction.Right)
        {
            var tempRect = new Size(CollisionMask.Height, CollisionMask.Width);
            this.CollisionMask.Box = new(CollisionMask.Box.Location, tempRect);
        }

        if(dir == Direction.Left)
        {
            var tempLoc = new Point(X + this.Width / 2, Y);
            var tempRect = new Size(CollisionMask.Height, CollisionMask.Width);
            this.CollisionMask.Box = new(tempLoc, tempRect);

        }
    }


    public GraphicsContainer RotateRight(Graphics g)
    {
        var rect = this.Box;
        var container = g.BeginContainer();

        g.TranslateTransform(rect.X + rect.Width / 2 - this.Width / 4, rect.Y + rect.Height / 2);
        g.RotateTransform(-90f);
        g.TranslateTransform(-rect.X - rect.Width / 2 - this.Width / 4, -rect.Y - rect.Height / 2);

        return container;
    }

    public GraphicsContainer RotateLeft(Graphics g)
    {
        var rect = this.Box;
        var container = g.BeginContainer();

        g.TranslateTransform(rect.X + rect.Width / 2 + Width / 4, rect.Y + rect.Height / 2);
        g.RotateTransform(90f);
        g.TranslateTransform(-rect.X - rect.Width / 2 + Width / 4, -rect.Y - rect.Height / 2);

        return container;
    }


    public override void Draw(Graphics g)
    {

        var container = Direction switch
        {
            Direction.Right => RotateRight(g),
            Direction.Left => RotateLeft(g),
            _ => null
        };


        g.DrawImage(
           BenchImage,
           this.Box,
           BenchSprite.X,
           BenchSprite.Y,
           BenchSprite.Width,
           BenchSprite.Height,
           GraphicsUnit.Pixel
           );



        if (container is not null)
            g.EndContainer(container);


        g.DrawRectangle(Pen, CollisionMask.Box);
    }
}



public class FoodBench : Bench
{
    public FoodBench(Rectangle box, Direction dir = Direction.Bottom)
        : base(new Rectangle(box.Location, new(96, 48)), 1, null, dir)
    {
        BenchSprite = this.benchSpriteLoader.GetAnimation(BenchTypes.Bench).Next();


    }

    public Item PlacedItem { get; private set; } = null;

    public override void Interact(Player p)
    {
        if (p.IsHolding && PlacedItem is not null)
            return;

        if (PlacedItem is null && p.IsHolding)
        {
            PlacedItem = p.holdingItem;
            PlacedItem.Interact(p);
            var temp = PlacedItem.Box.AlignCenter(this.Box);
            PlacedItem.Box = new Rectangle(temp.X, temp.Y - 10, temp.Width, temp.Height);
            return;
        }

        if (PlacedItem is not null)
        {
            PlacedItem.Interact(p);
            PlacedItem = null;
        }
    }
}

