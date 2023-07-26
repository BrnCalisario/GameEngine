using System.Drawing;

namespace Engine.Sprites;
using Extensions;
using System.Linq;

public class PanSpriteLoader : SpriteLoader<PanTypes>
{
    public PanSpriteLoader() : base(scale:3) { }

    Size SpriteSize = new(16,16);
    protected override void Load()
    {
        var scaled = SpriteSize.Scale(this.Scale);

        int startY = 0;

        var voidPan = new SpriteStream();
        var tomatoPan = new SpriteStream();
        var onionPan = new SpriteStream();
        var tomOnionPan = new SpriteStream();

        var rectV = new Rectangle(0, startY, scaled.Width, scaled.Height);
        var spriteV = new Sprite(rectV.Location, rectV.Size);
        voidPan.Add(spriteV);
        
        this.Animations.Add(PanTypes.Void, voidPan);

        startY += scaled.Height + 6;        
        for (int i = 0; i< scaled.Width * 5; i += scaled.Width + 9)
        {
            var rect = new Rectangle(i, startY, scaled.Width, scaled.Height);
            var sprite = new Sprite(rect.Location, rect.Size);
            tomatoPan.Add(sprite);
        }
        this.Animations.Add(PanTypes.TomatoPan, tomatoPan);

        startY += scaled.Width + 6;

        for(int i = 0;i<scaled.Width * 5;i += scaled.Width + 9)
        {
            var rect = new Rectangle(i, startY, scaled.Width, scaled.Height);
            var sprite = new Sprite(rect.Location, rect.Size);
            onionPan.Add(sprite);
        }
        this.Animations.Add(PanTypes.OnionPan, onionPan);

        startY += scaled.Width + 6;

        for (int i = 0; i < scaled.Width * 5; i += scaled.Width + 9)
        {
            var rect = new Rectangle(i, startY, scaled.Width, scaled.Height);
            var sprite = new Sprite(rect.Location, rect.Size);
            tomOnionPan.Add(sprite);
        }
        this.Animations.Add(PanTypes.TomOnionPan, tomOnionPan);

    }
}


public class PanSpriteController
    : SpriteController<PanSpriteLoader, PanTypes>
{
    public PanSpriteController()
    {
        this.SpriteLoader = new PanSpriteLoader();
    }

    public Sprite GetCurrentSprite(bool isCooking)
    {
        if (isCooking)
            return this.CurrentAnimation.Next();

        return this.CurrentAnimation.Sprites.First();
    }
}

public enum PanTypes
{ 
    Void,
    TomatoPan,
    OnionPan,
    TomOnionPan,
    FryingPan
}




