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

        var voidFryingPan = new SpriteStream();
        var cookingFryingPan = new SpriteStream();


        var rectVoid = new Rectangle(startX, 0, scaled.Width, scaled.Height);
        var spriteVoid = new Sprite(rectVoid.Location, rectVoid.Size);
        voidFryingPan.Add(spriteVoid);
        this.Animations.Add(FryingPanTypes.Void, voidFryingPan);

        for(int i = scaled.Width; i< scaled.Width * 4; i+= scaled.Width)
        {
            var rect = new Rectangle(i, 0, scaled.Width, scaled.Height);
            var sprite = new Sprite(rect.Location, rect.Size);
            cookingFryingPan.Add(sprite);
        }
        this.Animations.Add(FryingPanTypes.Frying,cookingFryingPan);

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
