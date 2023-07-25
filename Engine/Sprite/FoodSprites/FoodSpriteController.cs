
using System.Drawing;


namespace Engine.Sprites;

using Extensions;

public class FoodSpriteLoader : SpriteLoader<FoodTypes>
{
    public FoodSpriteLoader() : base(scale: 3) { }


    Size SpriteSize = new(12, 12);

    protected override void Load()
    {
        var scaled = SpriteSize.Scale(this.Scale);

        int startX = 0;
        int startY = 0;

        var tomatoes = new SpriteStream();
        var onions = new SpriteStream();
        var meats = new SpriteStream();
        var fishes = new SpriteStream();

        var tomatoesCutted = new SpriteStream();
        var onionsCutted = new SpriteStream();
        var meatsCutted = new SpriteStream();
        var fishesCutted = new SpriteStream();

       
        var rect = new Rectangle(startX, startY, scaled.Width, scaled.Height);
        var sprite = new Sprite(rect.Location, rect.Size);
        tomatoes.Add(sprite);
        this.Animations.Add(FoodTypes.Tomato, tomatoes);

        startX += scaled.Width;

        rect = new Rectangle(startX, startY, scaled.Width, scaled.Height);
        sprite = new Sprite(rect.Location, rect.Size);
        tomatoesCutted.Add(sprite);
        this.Animations.Add(FoodTypes.SlicedTomato, tomatoesCutted);

        startY += scaled.Height;
        startX = 0;

        rect = new Rectangle(startX, startY, scaled.Width, scaled.Height);
        sprite = new Sprite(rect.Location, rect.Size);
        onions.Add(sprite);
        this.Animations.Add(FoodTypes.Onion, onions);

        startX += scaled.Width;

        rect = new Rectangle(startX, startY, scaled.Width, scaled.Height);
        sprite = new Sprite(rect.Location, rect.Size);
        onionsCutted.Add(sprite);
        this.Animations.Add(FoodTypes.SlicedOnion, onionsCutted);

        startY += scaled.Height;
        startX = 0;

        rect = new Rectangle(startX, startY, scaled.Width, scaled.Height);
        sprite = new Sprite(rect.Location, rect.Size);
        meats.Add(sprite);
        this.Animations.Add(FoodTypes.Meat, meats);

        startX = startX + scaled.Width;

        rect = new Rectangle(startX, startY, scaled.Width, scaled.Height);
        sprite = new Sprite(rect.Location, rect.Size);
        meatsCutted.Add(sprite);
        this.Animations.Add(FoodTypes.SlicedMeat, meatsCutted);


        startY += scaled.Height ;
        startX = 0;

        rect = new Rectangle(startX, startY, scaled.Width, scaled.Height);
        sprite = new Sprite(rect.Location, rect.Size);
        fishes.Add(sprite);
        this.Animations.Add(FoodTypes.Fish, fishes);

        startX += scaled.Width;

        rect = new Rectangle(startX, startY, scaled.Width, scaled.Height);
        sprite = new Sprite(rect.Location, rect.Size);
        fishesCutted.Add(sprite);
        this.Animations.Add(FoodTypes.SlicedFish, fishesCutted);

    }
}

public class FoodSpriteController
    : SpriteController<FoodSpriteLoader, FoodTypes>
{
    public FoodSpriteController()
    {
        this.SpriteLoader = new FoodSpriteLoader();

    }
}



public enum FoodTypes
{
    Tomato,
    SlicedTomato,
    Onion,
    SlicedOnion,
    Meat,
    SlicedMeat,
    Fish,
    SlicedFish
}



