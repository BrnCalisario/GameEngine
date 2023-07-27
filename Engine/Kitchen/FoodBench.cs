
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Engine;

using Extensions;
using Sprites;
using Engine.Resource;

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

    protected virtual void SetDirection(Direction dir)
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

    protected virtual void DrawBench(Graphics g)
    {
        g.DrawImage(
            BenchImage,
            this.Box,
            BenchSprite.X,
            BenchSprite.Y,
            BenchSprite.Width,
            BenchSprite.Height,
            GraphicsUnit.Pixel
        );
    }

    protected virtual void CorrectVertical()
    {
        this.Box = CorrectPosition(Box);
    }

    protected virtual void CorrectHorizontal() 
    {
        this.Box = this.CollisionMask.Box;
        var newPos = new Point(BasicEngine.Current.Width - Box.X - Box.Width, Box.Y);
        this.Box = new Rectangle(newPos, this.Box.Size);
    }

    protected Rectangle CorrectPosition(Rectangle box)
    {
        var newPos = new Point(box.X, BasicEngine.Current.Height - box.Y - box.Height);
        return new Rectangle(newPos, box.Size);
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
            CorrectVertical();
        }

        this.DrawBench(g);

        if (container is not null)
        {
            g.EndContainer(container);


            if (Direction == Direction.Right || Direction == Direction.Top)
            {
                CorrectVertical();
            }
        }       
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

    public bool HasItem => PlacedItem is not null;

    public void SetItem(Item item)
    {
        this.PlacedItem = item;
        var temp = PlacedItem.Box.AlignCenter(this.Box);

        var relativePoint = GetRelativeItemPoint(PlacedItem);
        PlacedItem.Box = new Rectangle(relativePoint, temp.Size);
    }

    public void UnassignItem()
    {
        if (PlacedItem is null) return;

        var temp = PlacedItem;
        
        this.PlacedItem = null;
        temp.UnassignBench();
    }

    public void AssignItem(Item i)
    {
        if (PlacedItem is not null) return;

        this.PlacedItem = i;
        PlacedItem.AssignBench(this);        
    }


    public override void Interact(Player p)
    {
        if (p.IsHolding && this.HasItem)
            return;

        if (!HasItem && p.IsHolding)
        {
            this.AssignItem(p.holdingItem);            
            PlacedItem.Interact(p);

            var relativePoint = GetRelativeItemPoint(PlacedItem);
            PlacedItem.Box = new Rectangle(relativePoint, PlacedItem.Box.Size);

            return;
        }

        if (HasItem)
        {
            PlacedItem.Interact(p);
            this.UnassignItem();
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

    protected override void SetDirection(Direction dir)
    {
        this.Direction = dir;
        
        if (Direction == Direction.Left || Direction == Direction.Right)
        {
            var tempLoc = new Point(X - Width / 4, Y + Height / 4);
            this.Box = new(tempLoc, Box.Size);

            var tempSize = new Size(CollisionMask.Height, CollisionMask.Width);
            this.CollisionMask.Box = new(CollisionMask.Box.Location, tempSize);
        }
    }

    public override void Draw(Graphics g)
    {
        GraphicsContainer container = null;

        if (Direction == Direction.Left || Direction == Direction.Right)
        {
            container = InvertHorizontal(g, container);
            CorrectHorizontal();
        }

        if (Direction == Direction.Right || Direction == Direction.Top)
        {
            //container = RotateLeft(g);
            container = InvertVertical(g, container);
            CorrectVertical();
        }

        this.DrawBench(g);

        if (container is not null)
        {
            g.EndContainer(container);

            if (Direction == Direction.Top || Direction == Direction.Right)
            {
                CorrectVertical();
            }

            if (Direction == Direction.Left || Direction == Direction.Right)
            {
                CorrectHorizontal();
            }
        }
    }

    public override void Interact(Player p)
    {

    }
}



public class SmallBench : Bench
{
    public SmallBench(Rectangle box, Direction dir = Direction.Bottom)
        : base(new Rectangle(box.Location, new(48, 48)), 1, null, dir)
    {
        BenchSprite = this.benchSpriteLoader.GetAnimation(BenchTypes.Small).Next();
    }

    public override void Draw(Graphics g)
    {
        GraphicsContainer container = null;

        if (Direction == Direction.Left || Direction == Direction.Right)
        {
            container = InvertHorizontal(g, container);
            CorrectHorizontal();
        }

        if (Direction == Direction.Right || Direction == Direction.Top)
        {
            //container = RotateLeft(g);
            container = InvertVertical(g, container);
            CorrectVertical();
        }

        this.DrawBench(g);

        if (container is not null)
        {
            g.EndContainer(container);

            if (Direction == Direction.Top || Direction == Direction.Right)
            {
                CorrectVertical();
            }

            if (Direction == Direction.Left || Direction == Direction.Right)
            {
                CorrectHorizontal();
            }
        }
    }

    public override void Interact(Player p)
    {

    }
}