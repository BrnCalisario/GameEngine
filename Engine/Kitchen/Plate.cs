using System.Drawing;
using Engine.Sprites;
using System.Linq;

namespace Engine;

using Engine.Extensions;
using Engine.Resource;
using Sprites;
using System.Collections.Generic;
using System.Windows.Forms;
using static ProjectPaths;

public class Plate : Item
{
    public Plate(Rectangle r) : base(new Rectangle(r.Location, new(45, 33)))
    {
        SpriteController = new PlateSpriteController();
        SpriteController.StartAnimation(PlateTypes.VoidPlate);
    }

    readonly Image plateImage = Resources.PlateImage;
    readonly Image foodImage = Resources.FoodImage;

    private Sprite FoodSprite { get; set; }
    private Rectangle FoodRectangle { get; set; }
    public FoodSpriteLoader FoodSpriteLoader { get; set; }


    public List<Food> Ingredients { get; set; } = new List<Food>();

    public PlateSpriteController SpriteController { get; set; }

    bool hasTomato = false;
    bool hasOnion = false;

    bool hasMeat = false;
    bool hasFish = false;

    bool hasProtein = false;

    public override void Interact(Player p)
    {
        if (!p.IsHolding || p.holdingItem == this)
        {

            base.Interact(p);
        }


        if (p.holdingItem is not CookingTool holding)
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

                if (ingredient is Meat)
                    hasMeat = true;

                if (ingredient is Fish)
                    hasFish = true;                         
            }

            holding.ClearPan();

            if (hasTomato)
                SpriteController.StartAnimation(PlateTypes.TomatoPlate);

            if (hasOnion)
                SpriteController.StartAnimation(PlateTypes.OnionPlate);

            if (hasTomato && hasOnion)
                SpriteController.StartAnimation(PlateTypes.Tom_OnionPlate);

            if(!hasTomato && !hasOnion)
                SpriteController.StartAnimation(PlateTypes.VoidPlate);

            if (hasMeat)
                FoodSpriteLoader.GetAnimation(FoodTypes.Meat).Sprites.Last();

            if (hasFish)
                FoodSpriteLoader.GetAnimation(FoodTypes.Fish).Sprites.Last();

            var tempRect = new Rectangle(0, 0, 25, 25).AlignCenter(Box);
            FoodRectangle = tempRect;

        }
    }

    public void ClearPlate()
    {
        hasFish = false;
        hasMeat = false;
        hasOnion = false;
        hasTomato = false;
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

        if(hasProtein)
        {
            g.DrawImage(
             foodImage,
             FoodRectangle,
             FoodSprite.X,
             FoodSprite.Y,
             FoodSprite.Width,
             FoodSprite.Height,
             GraphicsUnit.Pixel
           );
        }
    }

}

