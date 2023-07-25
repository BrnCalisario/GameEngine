using System.Drawing;
using Engine.Sprites;
using System.Linq;

namespace Engine;

using Engine.Resource;
using Sprites;
using System.Collections.Generic;
using static ProjectPaths;

public class Plate : Item
{
    public Plate(Rectangle r, PlateTypes type) : base(new Rectangle(r.Location, new(45, 33)))
    {
        Loader = new PlateSpriteLoader();
        SpriteStream = Loader.GetAnimation(type);
    }

    Image plateImage = Resources.PlateImage;
    SpriteStream SpriteStream { get; set; }
    SpriteLoader<PlateTypes> Loader { get; set; }
    public List<Food> Ingredients { get; set; } = new List<Food>();


    public override void Interact(Player p)
    {
        if (!p.IsHolding || p.holdingItem == this)
            base.Interact(p);

        var holding = p.holdingItem as Pan;

        if (holding is null)
            return;

        holding.Interact(p);

        holding.Dispose();
    }

    public override void Draw(Graphics g)
    {
        var c = SpriteStream.Sprites.First();

        g.DrawImage(
            plateImage,
            this.Box,
            c.X,
            c.Y,
            c.Width,
            c.Height,
            GraphicsUnit.Pixel
            );
    }

}

