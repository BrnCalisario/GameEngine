using System.Drawing;

namespace Engine.Sprites;
using Extensions;
using System.Linq;
using static System.Windows.Forms.AxHost;

public class FryingPanSpriteLoader : SpriteLoader<FryingPanTypes>
{
    public FryingPanSpriteLoader() : base(scale: 3) { }

    Size SpriteSize = new(16, 16);

    protected override void Load()
    {
        var scaled = SpriteSize.Scale(this.Scale);

        int startX = 0;

        var FryingPan = new SpriteStream();

        for (int i = 0; i <= scaled.Width * 5; i += scaled.Width)
        {
            var rect = new Rectangle(i, 0, scaled.Width, scaled.Height);
            var sprite = new Sprite(rect.Location, rect.Size);
            FryingPan.Add(sprite);
        }
        this.Animations.Add(FryingPanTypes.Void, FryingPan);
    }
}



public class FryingPanSpriteController
    : SpriteController<FryingPanSpriteLoader, FryingPanTypes>
{
    public FryingPanSpriteController()
    {
        this.SpriteLoader = new FryingPanSpriteLoader();
    }

    public Sprite GetCurrentSprite(bool isCooking)
    {
        if (isCooking)
            return this.CurrentAnimation.Next();

        return this.CurrentAnimation.Sprites.First();
    }
}


public enum FryingPanTypes
{
    Void,
    Frying
}
