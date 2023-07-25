using System.Drawing;
using Engine.Sprites;
using System.Linq;

namespace Engine;

using Engine.Resource;
using Sprites;
using System.Collections.Generic;
using System.Windows.Forms;
using static ProjectPaths;

public class Plate : Item
{
    public Plate(Rectangle r, PlateTypes type) : base(new Rectangle(r.Location, new(45, 33)))
    {
        Loader = new PlateSpriteLoader();
        SpriteStream = Loader.GetAnimation(type);

        SpriteController = new PlateSpriteController();
        SpriteController.StartAnimation(PlateTypes.VoidPlate);
    }

    Image plateImage = Resources.PlateImage;
    SpriteStream SpriteStream { get; set; }
    SpriteLoader<PlateTypes> Loader { get; set; }
    public List<Food> Ingredients { get; set; } = new List<Food>();
    public PlateSpriteController SpriteController { get; set; }

    bool hasTomato = false;
    bool hasOnion = false;

    public override void Interact(Player p)
    {
        if (!p.IsHolding || p.holdingItem == this)
            base.Interact(p);


        if (p.holdingItem is not Pan holding)
            return;


        if (holding.HasCookedFood)
        {
            this.Ingredients = holding.Ingredients;
            holding.ClearPan();
        }

        //foreach (var ingredient in this.Ingredients)
        //{
        //    if (ingredient is Tomato)
        //        hasTomato = true;

        //    if (ingredient is Onion)
        //        hasOnion = true;
        //}

        //if (hasOnion && !hasTomato)
        //    SpriteController.StartAnimation(PlateTypes.OnionPlate);
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

