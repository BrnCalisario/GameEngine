using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace Engine;

using Engine.Extensions;
using Resource;
using Sprites;

public class Plate : Item
{
    public Plate(Rectangle r) : base(new Rectangle(r.Location, new(45, 33)))
    {
        SpriteController = new PlateSpriteController();
        SpriteController.StartAnimation(PlateTypes.VoidPlate);
        FoodSpriteLoader = new FoodSpriteLoader();
    }

    Image plateImage = Resources.PlateImage;
    Image foodImage = Resources.FoodImage;

    private Sprite FoodSprite { get; set; }
    private Rectangle FoodRectangle { get; set; }
    public FoodSpriteLoader FoodSpriteLoader { get; set; }


    public List<Food> Ingredients { get; set; } = new List<Food>();

    public PlateSpriteController SpriteController { get; set; }

    bool hasMeat { get; set; } = false;

    bool hasTomato = false;
    bool hasOnion = false;

    public override void Interact(Player p)
    {
        if (!p.IsHolding || p.holdingItem == this)
        {

            base.Interact(p);
        }


        if (p.holdingItem is not CookingTool holding)
            return;

        if (!holding.HasCookedFood)
            return;

        if(holding is Pan pan)
        {
            HandlePan(pan);
            return;
        }

        if(holding is FryingPan fryingPan)
        {
            HandleFryingPan(fryingPan);
            return;
        }
    }

    private void HandleFryingPan(FryingPan pan)
    {
        Ingredients.Add(pan.Ingredients.First());

        this.FoodSprite = pan.FoodSprite;
        this.FoodRectangle = pan.FoodRectangle;

        hasMeat = true;

        pan.ClearPan();
    }

    private void HandlePan(Pan pan)
    {
        foreach (var ingredient in pan.Ingredients)
        {
            Ingredients.Add(ingredient);

            if (ingredient is Tomato)
                hasTomato = true;

            if (ingredient is Onion)
                hasOnion = true;
        }

        pan.ClearPan();

        if (hasTomato)
            SpriteController.StartAnimation(PlateTypes.TomatoPlate);

        if (hasOnion)
            SpriteController.StartAnimation(PlateTypes.OnionPlate);

        if (hasTomato && hasOnion)
            SpriteController.StartAnimation(PlateTypes.Tom_OnionPlate);
    }


    public void ClearPlate()
    {
        hasMeat = false;
        hasOnion = false;
        hasTomato = false;
        Ingredients.Clear();
        SpriteController.StartAnimation(PlateTypes.VoidPlate);
    }

    public void Deliver()
    {
        Rectangle rect = new Rectangle(700, 350, this.Box.Width, this.Box.Height);
        this.Box = rect;
        //this.BenchParent.SetItem(this);
        //fb11.setItem(this);
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

        if (!hasMeat) return;

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

    public override void Update()
    {
        base.Update();

        if(hasMeat)
        {
            var temp = FoodRectangle.AlignCenter(this.Box);
            this.FoodRectangle = temp;
        }
    }

}

