
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

    protected readonly Image BenchImage = Resources.BenchImage;

    protected SpriteLoader<BenchTypes> benchSpriteLoader;
    protected Sprite BenchSprite { get; set; }
    public Direction Direction { get; set; }

    protected void SetDirection(Direction dir)
    {
        this.Direction = dir;

        if (Direction == Direction.Left || Direction == Direction.Right)
        {
            var tempLoc = new Point(X - Width / 2, Y);
            this.Box = new(tempLoc, Box.Size);

            var tempSize = new Size(CollisionMask.Height, CollisionMask.Width);
            this.CollisionMask.Box = new(CollisionMask.Box.Location, tempSize);

        }
    }

    public GraphicsContainer RotateLeft(Graphics g, GraphicsContainer container = null)
    {
        container ??= g.BeginContainer();

        var rect = this.Box;

        g.TranslateTransform(rect.X + rect.Width / 2 + Width / 4, rect.Y + rect.Height / 2 - Height);
        g.RotateTransform(90f);
        g.TranslateTransform(-rect.X + rect.Width / 2 - Width / 4, -rect.Y + rect.Height / 2 - Height);

        return container;
    }

    public override void Draw(Graphics g)
    {
        var container = Direction switch
        {
            Direction.Left or Direction.Right => RotateLeft(g),
            _ => null
        };

        if (Direction == Direction.Right || Direction == Direction.Top)
        {
            container = InvertVertical(g, container);
            var newPos = new Point(Box.X, BasicEngine.Current.Height - Box.Y - Box.Height);
            this.Box = new Rectangle(newPos, this.Box.Size);
        }




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
        {
            g.EndContainer(container);


            if (Direction == Direction.Right || Direction == Direction.Top)
            {
                var newPos = new Point(Box.X, BasicEngine.Current.Height - Box.Y - Box.Height);
                this.Box = new Rectangle(newPos, this.Box.Size);
            }
        }


        //CollisionMask?.Draw(g);
    }

    protected virtual Point GetRelativeItemPoint(Item item)
    {
        var temp = item.Box.AlignCenter(this.Box);
        Point p = Direction switch
        {
            Direction.Top or Direction.Bottom => new Point(temp.X, temp.Y - 10),
            Direction.Left or Direction.Right => new Point(temp.X + 20, temp.Y + 15),
            _ => temp.Location
        };

        return p;
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

            var relativePoint = GetRelativeItemPoint(PlacedItem);

            PlacedItem.Box = new Rectangle(relativePoint, PlacedItem.Box.Size);

            return;
        }

        if (PlacedItem is not null)
        {
            PlacedItem.Interact(p);
            PlacedItem = null;
        }
    }

}

public class CornerBench : Bench
{
    public CornerBench(Rectangle box, Direction dir = Direction.Bottom)
        : base(new Rectangle(box.Location, new(48, 48)), 1, null, dir)
    {
        BenchSprite = this.benchSpriteLoader.GetAnimation(BenchTypes.Corner).Next();
    }

    public override void Interact(Player p)
    {

    }
}