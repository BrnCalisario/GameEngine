using System;
using System.Drawing;
using Engine.Resource;
using Engine.Sprites;
using System.Linq;

namespace Engine;

using Extensions;

public class FoodBox<T> : Bench
    where T : Item, new()
{
    public FoodBox(Rectangle box, Direction dir = Direction.Bottom) : base(new Rectangle(box.Location, new(96, 48)), 1, null, dir)
    {
        BenchSprite = benchSpriteLoader.GetAnimation(BenchTypes.ItemBox).Next();
        FoodSpriteLoader = new FoodSpriteLoader();

        switch (Activator.CreateInstance(typeof(T)))
        {
            case Tomato:
                ItemSprite = FoodSpriteLoader.GetAnimation(FoodTypes.Tomato).Sprites.First();
                break;
            case Onion:
                ItemSprite = FoodSpriteLoader.GetAnimation(FoodTypes.Onion).Sprites.First();
                break;
            case Meat:
                ItemSprite = FoodSpriteLoader.GetAnimation(FoodTypes.Meat).Sprites.First();
                break;
            case Fish:
                ItemSprite = FoodSpriteLoader.GetAnimation(FoodTypes.Fish).Sprites.First();
                break;
        }

        var tempRect = new Rectangle(0, 0, 25, 25).AlignCenter(Box);

        
        tempRect.X += Direction == Direction.Right ? Width / 5 : Direction == Direction.Left ? 28 : 0;
        tempRect.Y += Direction.IsHorizontal() ? Height / 2 : Direction == Direction.Top ? 6 : -3;
        

        FoodRectangle = tempRect;
    }

    public Image FoodImage { get; set; } = Resources.FoodImage;

    Sprite ItemSprite { get; set; }
    Rectangle FoodRectangle { get; set; } = new Rectangle();

    readonly SpriteLoader<FoodTypes> FoodSpriteLoader;

    protected override void DrawBench(Graphics g)
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
        //newPos = new Point(wid - X - Box.Width, Y);
        //this.Box = new Rectangle(newPos, this.Box.Size);

        //g.DrawRectangle(Pen, CollisionMask.Box);
    }

    public override void Draw(Graphics g)
    {
        base.Draw(g);
        g.DrawImage(
            FoodImage,
            FoodRectangle,
            ItemSprite.X,
            ItemSprite.Y,
            ItemSprite.Width,
            ItemSprite.Height,
            GraphicsUnit.Pixel
        );
    }

    public override void Interact(Player p)
    {
        if (p.IsHolding)
            return;

        Item item = (T)Activator.CreateInstance(typeof(T), new object[] { Box });
        BasicEngine.Current.AddBody(item);
        item.Interact(p);
    }
}