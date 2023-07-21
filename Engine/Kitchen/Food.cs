using System.Drawing;
using System.Linq;

namespace Engine;

using Engine.Resource;
using Sprites;
using static ProjectPaths;

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

    public Food() : base(new Point(0, 0))
    {
        Loader = new FoodSpriteLoader();
        SpriteStream = Loader.GetAnimation(FoodTypes.Tomato);
    }

    Image FoodImage = Resources.FoodImage;
    SpriteLoader<FoodTypes> Loader { get; set; }
     
    SpriteStream SpriteStream { get; set; }

    public override void Draw(Graphics g)
    {
        var c = SpriteStream.Sprites.Last();

        g.DrawImage(
            FoodImage,
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

    public Tomato() { }
}

public class Onion : Food
{
    public Onion(Rectangle r) : base(r, FoodTypes.Onion)
    {
    }

    public Onion(Point point) : base(point, FoodTypes.Onion)
    {
    }

    public Onion() { }
}


public class Meat : Food
{
    public Meat(Rectangle r) : base(r, FoodTypes.Meat)
    {
    }

    public Meat(Point point) : base(point, FoodTypes.Meat)
    {
    }

    public Meat() { }
}

public class Fish : Food
{
    public Fish(Rectangle r) : base(r, FoodTypes.Fish)
    {
    }

    public Fish(Point point) : base(point, FoodTypes.Fish)
    {
    }

    public Fish() { }
}



