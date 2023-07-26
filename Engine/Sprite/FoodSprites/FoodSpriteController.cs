
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

        for( int i = startX; i < scaled.Width * 2; i += scaled.Width )
        {
            var rect = new Rectangle(i, startY, scaled.Width, scaled.Height);
            var sprite = new Sprite(rect.Location, rect.Size);
            tomatoes.Add(sprite);
        }
        this.Animations.Add(FoodTypes.Tomato, tomatoes);

        startY += scaled.Height;
        startX = 0;


        for(int i = startX; i < scaled.Width * 2; i += scaled.Width )
        {
            var rect = new Rectangle(i, startY, scaled.Width, scaled.Height);
            var sprite = new Sprite(rect.Location, rect.Size);
            onions.Add(sprite);
        }
        this.Animations.Add(FoodTypes.Onion, onions);

        startY += scaled.Height;
        startX = 0;

        for(int i = startX;i < scaled.Width * 3; i += scaled.Width )
        {
            var rect = new Rectangle(i, startY, scaled.Width, scaled.Height);
            var sprite = new Sprite(rect.Location, rect.Size);
            meats.Add(sprite);
        }
        this.Animations.Add(FoodTypes.Meat, meats);

        startY += scaled.Height;
        startX = 0;

        for (int i = startX; i < scaled.Width * 3; i += scaled.Width)
        {
            var rect = new Rectangle(i, startY, scaled.Width, scaled.Height);
            var sprite = new Sprite(rect.Location, rect.Size);
            fishes.Add(sprite);
        }

        this.Animations.Add(FoodTypes.Fish, fishes);

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
    Onion,
    Meat,
    Fish
}



