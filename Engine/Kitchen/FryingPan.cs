using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Engine;

using Extensions;
using Resource;
using Sprites;
using System;

public class FryingPan : CookingTool
{
    public FryingPan(Rectangle r) : base(new Rectangle(r.Location, new(48, 48)))
    {
        SpriteController = new FryingPanSpriteController();
        FoodSpriteLoader = new FoodSpriteLoader();
        SpriteController.StartAnimation(FryingPanTypes.Void);


        var timeRect = new Rectangle(X, Y + Height, Width, 5);
        TimeBar = new(timeRect, TimeSpan.FromSeconds(5))
        {
            ChangeColor = false
        };

        TimeBar.OnFinish += delegate
        {
            this.Cooked = true;
            SetCookedFood(Ingredients[0]);
            TimeBar.Stop();
        };
    }


    readonly Image fryingPanImage = Resources.FryingPanImage;
    readonly Image foodImage = Resources.FoodImage;

    public Sprite FoodSprite { get; private set; }
    public Rectangle FoodRectangle { get; private set; }

    public FryingPanSpriteController SpriteController { get; set; }
    public FoodSpriteLoader FoodSpriteLoader { get; set; }
    public bool HasFood => Ingredients.Count == 1;
    public override bool HasCookedFood => Cooked;  
    private bool Cooked { get; set; }

    TimeBar? TimeBar { get; set; } = null;


    private void SetTimeBar()
    {
        var timeRect = new Rectangle(X, Y + Height, Width, 5);
        TimeBar = new(timeRect, TimeSpan.FromSeconds(5))
        {
            ChangeColor = false
        };

        TimeBar.OnFinish += delegate
        {
            this.Cooked = true;
            SetCookedFood(Ingredients[0]);
            TimeBar?.Stop();
        };
    }

    public override void Interact(Player p)
    {
       
        if (p.IsHolding)
        {
            if (p.holdingItem is not Meat && p.holdingItem is not Fish && p.holdingItem is not CookingTool)
                return;
        }

    
        base.Interact(p);

        SetTimeBar();
    }

    private void AnchorFood()
    {
        var food = this.Ingredients[0];

        if (HasFood)
        {

            FoodSprite = food switch
            {
                Meat => FoodSpriteLoader.GetAnimation(FoodTypes.Meat).Sprites.Skip(1).First(),
                Fish => FoodSpriteLoader.GetAnimation(FoodTypes.Fish).Sprites.Skip(1).First(),
            _ => throw new System.Exception()
            };
        };

        if(HasCookedFood)
            SetCookedFood(food);

        var tempRect = new Rectangle(0, 0, 25, 25).AlignCenter(Box);

        tempRect.X += 5;
        tempRect.Y += 3;
        FoodRectangle = tempRect;        
    }

    private void SetCookedFood(Food food)
    {
        FoodSprite = food switch
        {
            Meat => FoodSpriteLoader.GetAnimation(FoodTypes.Meat).Sprites.Last(),
            Fish => FoodSpriteLoader.GetAnimation(FoodTypes.Fish).Sprites.Last(),
            _ => throw new System.Exception()
        };
    }

    protected override void SetOrder()
    {
        if (!HasFood) return;

        AnchorFood();

        var isMeat = Ingredients.First() is Meat;

        this.OrderType = isMeat ? OrderType.Steak : OrderType.Fish;
    }

    public override void ClearPan()
    {
        base.ClearPan();        
        SpriteController.StartAnimation(FryingPanTypes.Void);
        TimeBar = null;
        Cooked = false;
    }


    public override void Draw(Graphics g)
    {
        bool changeSprite = this.IsCooking;
        var c = SpriteController.GetCurrentSprite(changeSprite);

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

        if (HasFood)
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

        TimeBar?.Draw(g);
        if (HasFood && !BeingHold )
        {
            TimeBar?.Start();
        }

    }

    public override void Update()
    {
        base.Update();

        if ((BeingHold || InBench) && HasFood)
        {
            var temp = FoodRectangle.AlignCenter(this.Box);
            
                temp.X += this.PlayerParent?.CurrentDirection == Direction.Left ? -12 : 6;
            this.FoodRectangle = temp;            
        }

        var pos = Box.Location;
        pos.Y += Height;

        
        TimeBar?.UpdateLocation(pos);
        
        if(!BeingHold)
            TimeBar?.Update();
    }

}