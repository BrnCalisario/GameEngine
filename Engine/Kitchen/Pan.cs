using System.Drawing;
using Engine.Sprites;
using System.Linq;

namespace Engine;

using Engine.Resource;
using Sprites;
using System.Collections.Generic;
using static ProjectPaths;

public class Pan : Item
{
    public Pan(Rectangle r) : base(new Rectangle(r.Location, new(48, 48)))
    {
        SpriteController = new PanSpriteController();
        SpriteController.StartAnimation(PanTypes.Void);
    }

    readonly Image panImage = Resources.PanImage;
    public PanSpriteController SpriteController { get; set; }
    public List<Food> Ingredients { get; set; } = new List<Food>();

    public bool IsCooking { get; set; } = false;

    public override void Interact(Player p)
    {
        if(!p.IsHolding || p.holdingItem == this)
        {
            base.Interact(p);
            this.IsCooking = false;
        }

        if (p.holdingItem is not Food holding)
            return;

        holding.Interact(p);

        holding.Dispose();
        Ingredients.Add(holding);

        if (holding is Tomato)
            SpriteController.StartAnimation(PanTypes.TomatoPan);

        if (holding is Onion)
            SpriteController.StartAnimation(PanTypes.OnionPan);

        //SpriteController.IsCooking = true;
    }

    public void ClearPan()
    {
        Ingredients.Clear();
        SpriteController.StartAnimation(PanTypes.Void);
    }
      

    public override void Draw(Graphics g)
    {
        var c = SpriteController.GetCurrentSprite(this.IsCooking);

        g.DrawImage(
            panImage,
            this.Box,
            c.X,
            c.Y,
            c.Width,
            c.Height,
            GraphicsUnit.Pixel
            );
    }
}

