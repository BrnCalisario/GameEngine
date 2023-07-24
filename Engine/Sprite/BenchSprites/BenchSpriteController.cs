using Engine.Extensions;
using System.Drawing;


namespace Engine.Sprites;

public class BenchSpriteLoader : SpriteLoader<BenchTypes>
{
    public BenchSpriteLoader() : base(scale: 3) { }

    Size SpriteSize = new(32, 16);
    Size cornerSize = new(16, 16);

    protected override void Load()
    {
        var scaled = SpriteSize.Scale(this.Scale);

        var scaledCorner = cornerSize.Scale(this.Scale);

        int startY = 3;

        var corner = new SpriteStream();
        var bench = new SpriteStream();
        var itemBox = new SpriteStream();
        var oven = new SpriteStream();

        var rectCorner = new Rectangle(3, startY, scaledCorner.Width, scaledCorner.Height);
        Sprite spriteCorner = new Sprite(rectCorner.Location, rectCorner.Size);
        corner.Add(spriteCorner);
        this.Animations.Add(BenchTypes.Corner, corner);

        startY += scaled.Height + 9;

        var rectBench = new Rectangle(3, startY, scaled.Width, scaled.Height);
        Sprite spriteBench = new Sprite(rectBench.Location, rectBench.Size);
        bench.Add(spriteBench);
        this.Animations.Add(BenchTypes.Bench, bench);

        startY += scaled.Height + 9;

        var rectItemBox = new Rectangle(3, startY, scaled.Width, scaled.Height);
        Sprite spriteItemBox = new Sprite(rectItemBox.Location, rectItemBox.Size);
        itemBox.Add(spriteItemBox);
        this.Animations.Add(BenchTypes.ItemBox, itemBox);

        startY += scaled.Height + 9;

        var rectOven = new Rectangle(3, startY, scaled.Width, scaled.Height);
        Sprite spriteOven = new Sprite(rectOven.Location, rectOven.Size);
        oven.Add(spriteOven);
        this.Animations.Add(BenchTypes.Oven, oven);
    }
}




public class BenchSpriteController
    : SpriteController<BenchSpriteLoader, BenchTypes>
{
    public BenchSpriteController()
    {
        this.SpriteLoader = new BenchSpriteLoader();
    }
}

public enum BenchTypes
{
    Bench,
    ItemBox,
    Oven,
    Corner
}


