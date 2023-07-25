using System.Drawing;
using Engine.Sprites;
using System.Linq;

namespace Engine;

using Engine.Resource;
using Sprites;
using System.Collections.Generic;
using System.Windows.Forms;
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

    public bool HasCookedFood => Ingredients.Count() >= 3;

    public bool IsCooking { get; set; } = false;

    bool hasTomato = false;
    bool hasOnion = false;

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

        foreach (var ingredient in Ingredients)
        {
            if (ingredient is Tomato)
                hasTomato = true;

            if (ingredient is Onion)
                hasOnion = true;
        }

        if(hasTomato && !hasOnion)
            SpriteController.StartAnimation(PanTypes.TomatoPan);

        if(!hasTomato && hasOnion)
            SpriteController.StartAnimation(PanTypes.OnionPan);

        if (hasTomato && hasOnion)
            SpriteController.StartAnimation(PanTypes.TomOnionPan);
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

