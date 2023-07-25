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
        //Loader = new PlateSpriteLoader();
        //SpriteStream = Loader.GetAnimation(type);

        SpriteController = new PlateSpriteController();
        SpriteController.StartAnimation(type);
    }

    Image plateImage = Resources.PlateImage;

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
            foreach (var ingredient in holding.Ingredients)
            {
                Ingredients.Add(ingredient);

                if (ingredient is Tomato)
                    hasTomato = true;

                if (ingredient is Onion)
                    hasOnion = true;
            }

            holding.ClearPan();

            //MessageBox.Show("Onion: " + hasOnion + "\nTomato: " + hasTomato);


            if (hasTomato)
                SpriteController.StartAnimation(PlateTypes.TomatoPlate);

            if (hasOnion)
                SpriteController.StartAnimation(PlateTypes.OnionPlate);

            if (hasTomato && hasOnion)
                SpriteController.StartAnimation(PlateTypes.Tom_OnionPlate);

            if(!hasTomato && !hasOnion)
                SpriteController.StartAnimation(PlateTypes.VoidPlate);
        }
    }

    public void ClearPlate()
    {
        Ingredients.Clear();
        SpriteController.StartAnimation(PlateTypes.VoidPlate);
    }

    public override void Draw(Graphics g)
    {
        var c = SpriteController.CurrentAnimation.Next();

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

