using System.Drawing;
using Engine.Sprites;
using System.Linq;

namespace Engine;

using Engine.Extensions;
using Engine.Resource;
using Sprites;
using System.Drawing.Drawing2D;

public class FryingPan : CookingTool
{
    public FryingPan(Rectangle r) : base(new Rectangle(r.Location, new(48, 48)))
    {
        SpriteController = new FryingPanSpriteController();
        FoodSpriteLoader = new FoodSpriteLoader();
        SpriteController.StartAnimation(FryingPanTypes.Void);
    }

    readonly Image fryingPanImage = Resources.FryingPanImage;
    readonly Image foodImage = Resources.FoodImage;

    private Sprite FoodSprite { get; set; }
    private Rectangle FoodRectangle { get; set; } 

    public FryingPanSpriteController SpriteController { get; set; }
    public FoodSpriteLoader FoodSpriteLoader { get; set; }

    bool hasFood => this.Ingredients.Count > 0;

    bool hasMeat = false;
    bool hasFish = false;

    public bool HasFood => Ingredients.Count >= 1;

    public override void Interact(Player p)
    {
        if(p.IsHolding)
        {
            if (p.holdingItem is not Meat && p.holdingItem is not Fish)
                return;
        }
        base.Interact(p);

        if (hasFood)
        {
            var food = this.Ingredients[0];
            FoodSprite = food switch
            {
                Meat => FoodSpriteLoader.GetAnimation(FoodTypes.Meat).Sprites.Last(),
                Fish => FoodSpriteLoader.GetAnimation(FoodTypes.Fish).Sprites.Last(),
                _ => throw new System.Exception()
            };

            var tempRect = new Rectangle(0, 0, 25, 25).AlignCenter(Box);
            //tempRect.X += Direction == Direction.Right ? Width / 5 : Direction == Direction.Left ? 28 : 0;
            //tempRect.Y += Direction.IsHorizontal() ? Height / 2 : Direction == Direction.Top ? 6 : -3;

            tempRect.X += 5;
            tempRect.Y += 3;
            FoodRectangle = tempRect;
        }

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
        //SpriteController.CurrentAnimation.Sprites.First();
        SpriteController.StartAnimation(FryingPanTypes.Void);
    }


    public override void Draw(Graphics g)
    {
        this.IsCooking = HasFood;
       

        var c = SpriteController.GetCurrentSprite(this.IsCooking);

        GraphicsContainer container = null;

        if (Player?.Invert ?? false)
        {
            container = this.InvertHorizontal(g);
            this.CorrectHorizontal();
        }

        g.DrawImage(
            fryingPanImage,
            this.Box,
            c.X,
            c.Y,
            c.Width,
            c.Height,
            GraphicsUnit.Pixel
            );

        if(hasFood)
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

        if(Player?.Invert ?? false)
        {            
            this.CorrectHorizontal();
        };


        if (container is not null)
            g.EndContainer(container);

        
    }

    public override void Update()
    {
        base.Update();
        
        if(this.BeingHold && hasFood)
        {
            this.FoodRectangle = this.SetRectRelativePosition(this.FoodRectangle, Player.CurrentDirection);
        }
    }

}