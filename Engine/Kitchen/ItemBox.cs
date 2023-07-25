using System;
using System.Drawing;
using Engine.Resource;
using Engine.Sprites;
using System.Linq;

namespace Engine;

using Extensions;

public class FoodBox<T> : Interactable, IUnwalkable
    where T : Item, new()
{

    public Image BenchImage { get; set; } = Resources.BenchImage;
    public Image FoodImage { get; set; } = Resources.FoodImage;

    Sprite BenchSprite { get; set; }

    Sprite ItemSprite { get; set; }

    Rectangle FoodRectangle { get; set;} = new Rectangle();


    readonly SpriteLoader<BenchTypes> SpriteLoader;

    readonly SpriteLoader<FoodTypes> FoodSpriteLoader;

    public FoodBox(Rectangle box) : base(new Rectangle(box.Location, new(96, 48)), 1, new Pen(Color.Crimson))
    {
        SpriteLoader = new BenchSpriteLoader();
        FoodSpriteLoader = new FoodSpriteLoader();
        BenchSprite = SpriteLoader.GetAnimation(BenchTypes.ItemBox).Next();

        switch (Activator.CreateInstance(typeof(T)))
        {
            case Tomato:
                ItemSprite = FoodSpriteLoader.GetAnimation(FoodTypes.Tomato).Sprites.Last();
                break;
            case Onion:
                ItemSprite = FoodSpriteLoader.GetAnimation(FoodTypes.Onion).Sprites.Last();
                break;
            case Meat:
                ItemSprite = FoodSpriteLoader.GetAnimation(FoodTypes.Meat).Sprites.Last();
                break;
            case Fish:
                ItemSprite = FoodSpriteLoader.GetAnimation(FoodTypes.Fish).Sprites.Last();
                break;
        }

        var tempRect = new Rectangle(0, 0, 25, 25).AlignCenter(Box);
 
        tempRect.Y -= 6;

        FoodRectangle = tempRect;

    }

    public override void Draw(Graphics g)
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

        g.DrawImage(
            FoodImage,
            FoodRectangle,
            ItemSprite.X,
            ItemSprite.Y,
            ItemSprite.Width,
            ItemSprite.Height,
            GraphicsUnit.Pixel
            );

        //newPos = new Point(wid - X - Box.Width, Y);
        //this.Box = new Rectangle(newPos, this.Box.Size);

        g.DrawRectangle(Pen, CollisionMask.Box);
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