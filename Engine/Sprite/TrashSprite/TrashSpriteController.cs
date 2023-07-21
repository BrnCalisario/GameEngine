using Engine.Extensions;
using System.Drawing;

namespace Engine.Sprites;


public class TrashSpriteLoader : SpriteLoader<TrashTypes>
{
    public TrashSpriteLoader() : base(scale: 3) { }

    Size SpriteSize = new(16, 16);

    protected override void Load()
    {
        var scaled = SpriteSize.Scale(this.Scale);

        int startY = 0;

        var trashClosed = new SpriteStream();

        var rectClosed = new Rectangle(0, startY, scaled.Width, scaled.Height);
        Sprite spriteClosed = new Sprite(rectClosed.Location, rectClosed.Size);
        trashClosed.Add(spriteClosed);
        this.Animations.Add(TrashTypes.Closed, trashClosed);


        startY += scaled.Height;
        var trashOpen = new SpriteStream();

        var rectOpen = new Rectangle(0, startY, scaled.Width, scaled.Height);
        Sprite spriteOpen = new Sprite(rectOpen.Location, rectOpen.Size);
        trashOpen.Add(spriteOpen);
        this.Animations.Add(TrashTypes.Open, trashOpen);

    }
}


public class TrashSpriteController
    : SpriteController<TrashSpriteLoader, TrashTypes>
{
    public TrashSpriteController()
    {
        this.SpriteLoader = new TrashSpriteLoader();
    }
}

public enum TrashTypes
{
    Open,
    Closed
}

