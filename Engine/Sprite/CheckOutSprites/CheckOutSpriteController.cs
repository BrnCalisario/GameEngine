using Engine.Extensions;
using System.Drawing;
using static System.Windows.Forms.AxHost;


namespace Engine.Sprites;

public class CheckOutSpriteLoader : SpriteLoader<CheckOutTypes>
{
    public CheckOutSpriteLoader() : base(scale: 3) { }

    Size SpriteSize = new(32, 16);

    protected override void Load()
    {
        var scaled = SpriteSize.Scale(this.Scale);
        var belt = new SpriteStream();

        int startX = 3;

        for(int i = startX; i < scaled.Width * 5; i += scaled.Width + 6)
        {
            var rect = new Rectangle(i, 3, scaled.Width, scaled.Height);
            Sprite sprite = new Sprite(rect.Location, rect.Size);
            belt.Add(sprite);
        }
        this.Animations.Add(CheckOutTypes.TransportingBelt, belt);
    }
}


public class CheckOutSpriteController
    : SpriteController<CheckOutSpriteLoader, CheckOutTypes>
{
    public CheckOutSpriteController()
    {
        this.SpriteLoader = new CheckOutSpriteLoader();
    }
}


public enum CheckOutTypes
{ 
    TransportingBelt
}

