using System.Drawing;

namespace Engine.Sprites;
using Extensions;







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

        for (int i = 0; i<scaled.Width * 5; i += scaled.Width)
        {
            var rect = new Rectangle(i, startY, scaled.Width, scaled.Height);
            var sprite = new Sprite(rect.Location, rect.Size);
            voidPan.Add(sprite);
        }
        this.Animations.Add(PanTypes.Void, voidPan);

        startY += scaled.Width + 9;
        
        for (int i = 0; i<scaled.Width * 5; i += scaled.Width)
        {
            var rect = new Rectangle(i, startY, scaled.Width, scaled.Height);
            var sprite = new Sprite(rect.Location, rect.Size);
            tomatoPan.Add(sprite);
        }
        this.Animations.Add(PanTypes.TomatoPan, tomatoPan);

        startY += scaled.Width + 9;

        for(int i = 0;i<scaled.Width * 5;i += scaled.Width)
        {
            var rect = new Rectangle(i, startY, scaled.Width, scaled.Height);
            var sprite = new Sprite(rect.Location, rect.Size);
            onionPan.Add(sprite);
        }
        this.Animations.Add(PanTypes.OnionPan, onionPan);

        startY += scaled.Width + 9;

        for (int i = 0; i < scaled.Width * 5; i += scaled.Width)
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
}

public enum PanTypes
{ 
    Void,
    TomatoPan,
    OnionPan,
    TomOnionPan
}




