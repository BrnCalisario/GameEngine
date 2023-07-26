using System.Drawing;
using Engine.Sprites;
using System.Linq;

namespace Engine;

using Engine.Extensions;
using Engine.Resource;
using Sprites;
using System.Drawing.Drawing2D;
using System.Net.Mail;

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

    public bool HasFood => Ingredients.Count >= 1;

    public override void Interact(Player p)
    {
        if (p.IsHolding)
        {
            if (p.holdingItem is not Meat && p.holdingItem is not Fish && p.holdingItem is not CookingTool)
                return;
        }

        base.Interact(p);


        var food = this.Ingredients[0];

        if (hasFood)
        {

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

        if (PlayerParent?.Invert ?? false)
        {
            container = this.InvertHorizontal(g);
            this.Box = this.CorrectHorizontal(this.Box);
            this.FoodRectangle = this.CorrectHorizontal(this.FoodRectangle);

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

        if (hasFood)
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

        if (PlayerParent?.Invert ?? false)
        {
            this.Box = this.CorrectHorizontal(this.Box);
            this.FoodRectangle = this.CorrectHorizontal(this.FoodRectangle);
        };


        if (container is not null)
            g.EndContainer(container);


    }

    public override void Update()
    {
        base.Update();

        if ((BeingHold || InBench) && hasFood)
        {
            if (BeingHold)
            {
                var tempRec = this.SetRectRelativePosition(this.FoodRectangle, PlayerParent.CurrentDirection);
                tempRec.X += PlayerParent?.Invert ?? false ? 30 : -15;
                this.FoodRectangle = tempRec;
                return;
            }

            var temp = FoodRectangle.AlignCenter(this.Box);
            this.FoodRectangle = temp;            
        }
    }

}