using System.Drawing;
using Engine.Sprites;
using System.Linq;

namespace Engine;

using Engine.Resource;
using Sprites;
using System.Collections.Generic;
using System.Windows.Forms;
using static ProjectPaths;


public class FryingPan : CookingTool
{
    public FryingPan(Rectangle r) : base(new Rectangle(r.Location, new(48, 48)))
    {
        SpriteController = new FryingPanSpriteController();
        SpriteController.StartAnimation(FryingPanTypes.Void);
    }


    readonly Image fryingPanImage = Resources.FryingPanImage;
    public FryingPanSpriteController SpriteController { get; set; }

    bool hasMeat = false;
    bool hasFish = false;

    public override void Interact(Player p)
    {
        base.Interact(p);

        foreach (var ingredient in Ingredients)
        {
            if (ingredient is Meat)
                hasMeat = true;

            if (ingredient is Fish)
                hasFish = true;
        }
    }

    public override void ClearPan()
    {
        base.ClearPan();
        SpriteController.StartAnimation(FryingPanTypes.Void);
    }


    public override void Draw(Graphics g)
    {
        var c = SpriteController.GetCurrentSprite(this.IsCooking);

        g.DrawImage(
            fryingPanImage,
            this.Box,
            c.X,
            c.Y,
            c.Width,
            c.Height,
            GraphicsUnit.Pixel
            );
    }

}