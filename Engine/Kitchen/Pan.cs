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
    public Pan(Rectangle r, PanTypes type) : base(new Rectangle(r.Location, new(48, 48)))
    {
        Loader = new PanSpriteLoader();
        SpriteStream = Loader.GetAnimation(type);
    }

    Image panImage = Resources.PanImage;
    SpriteStream SpriteStream { get; set; }
    SpriteLoader<PanTypes> Loader { get; set; }
    public List<Food> Ingredients { get; set; } = new List<Food>();

    public override void Interact(Player p)
    {
        if(!p.IsHolding || p.holdingItem == this)
            base.Interact(p);

        var holding = p.holdingItem as Food;

        if (holding is null)
            return;

        holding.Interact(p);

        holding.Dispose();
        Ingredients.Add(holding);
       
    }

    public void ClearPan()
        => Ingredients.Clear();

    public override void Draw(Graphics g)
    {
        var c = SpriteStream.Sprites.First();

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

