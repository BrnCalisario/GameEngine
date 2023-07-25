using System.Drawing;

namespace Engine.Sprites;
using Extensions;
using static System.Windows.Forms.AxHost;


public class PlateSpriteLoader : SpriteLoader<PlateTypes>
{
    public PlateSpriteLoader() : base(scale: 3) { }
    Size SpriteSize = new(15, 11);

    protected override void Load()
    {
        var scaled = SpriteSize.Scale(this.Scale);

        int startX = 0;

        var voidPlate = new SpriteStream();
        var tomatoPlate = new SpriteStream();
        var onionPlate = new SpriteStream();
        var tomOnionPlate = new SpriteStream();

        var rectVoid = new Rectangle(startX, 0, scaled.Width, scaled.Height);
        Sprite spriteVoid = new Sprite(rectVoid.Location, rectVoid.Size);
        voidPlate.Add(spriteVoid);
        this.Animations.Add(PlateTypes.VoidPlate, voidPlate);

        startX += scaled.Width;

        var rectTomato = new Rectangle(startX, 0, scaled.Width, scaled.Height);
        Sprite spriteTomato = new Sprite(rectTomato.Location, rectTomato.Size);
        tomatoPlate.Add(spriteTomato);
        this.Animations.Add(PlateTypes.TomatoPlate, tomatoPlate);

        startX += scaled.Width;

        var rectOnion = new Rectangle(startX, 0, scaled.Width, scaled.Height);
        Sprite spriteOnion = new Sprite(rectOnion.Location, rectOnion.Size);
        onionPlate.Add(spriteOnion);
        this.Animations.Add(PlateTypes.OnionPlate, onionPlate);

        startX += scaled.Width;

        var rectTomOnion = new Rectangle(startX, 0, scaled.Width, scaled.Height);
        Sprite spriteTomOnion = new Sprite(rectTomOnion.Location, rectTomOnion.Size);
        tomOnionPlate.Add(spriteTomOnion);
        this.Animations.Add(PlateTypes.Tom_OnionPlate, tomOnionPlate);
    }
}



public class PlateSpriteController
    : SpriteController<PlateSpriteLoader, PlateTypes>
{
    public PlateSpriteController()
    {
        this.SpriteLoader = new PlateSpriteLoader();

    }
}


public enum PlateTypes
{
    VoidPlate,
    TomatoPlate,
    OnionPlate,
    Tom_OnionPlate
}

