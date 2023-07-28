using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace Engine;

using Engine.Extensions;
using Resource;
using Sprites;
using System.Windows.Forms;

public class Plate : Item
{
    public Plate(Rectangle r) : base(new Rectangle(r.Location, new(45, 33)))
    {
        SpriteController = new PlateSpriteController();
        SpriteController.StartAnimation(PlateTypes.VoidPlate);
        FoodSpriteLoader = new FoodSpriteLoader();
    }

    readonly Image plateImage = Resources.PlateImage;
    readonly Image foodImage = Resources.FoodImage;

    private Sprite FoodSprite { get; set; }
    private Rectangle FoodRectangle { get; set; }
    public FoodSpriteLoader FoodSpriteLoader { get; set; }
    public PlateSpriteController SpriteController { get; set; }

    public OrderType Order { get; private set; } = OrderType.InvalidOrder;

    private bool hasProtein { get; set; } = false;

    private int lastPositionX { get; set; }
    private int lastPositionY { get; set; }



    public override void Interact(Player p)
    {
        if (!p.IsHolding || p.holdingItem == this)        
            base.Interact(p);
        
        if (p.holdingItem is not CookingTool holding)
            return;

        if (!holding.HasCookedFood)
            return;

        if (Order != OrderType.InvalidOrder)
            return;

        if(holding is Pan pan)
            HandlePan(pan);

        if(holding is FryingPan fryingPan)     
            HandleFryingPan(fryingPan);        
    }

    private void HandleFryingPan(FryingPan pan)
    {       
        this.FoodSprite = pan.FoodSprite;
        this.FoodRectangle = pan.FoodRectangle;
        this.Order = pan.OrderType;
        this.hasProtein = true;

        pan.ClearPan();
    }

    private void HandlePan(Pan pan)
    {
        this.Order = pan.OrderType;        

        pan.ClearPan();

        SetPlateSprite(this.Order);
    }

    private void SetPlateSprite(OrderType order)
    {
        PlateTypes type = order switch
        {
            OrderType.TomatoSoup => PlateTypes.TomatoPlate,
            OrderType.OnionSoup => PlateTypes.OnionPlate,
            OrderType.MixedSoup => PlateTypes.Tom_OnionPlate,
            _ => PlateTypes.VoidPlate
        };      

        SpriteController.StartAnimation(type);
    }


    public void ClearPlate()
    { 
        Order = OrderType.InvalidOrder;
        SpriteController.StartAnimation(PlateTypes.VoidPlate);
        this.hasProtein = false;

    }

    public void Deliver()
    {   
       

        Rectangle rect = new(700, 350, this.Box.Width, this.Box.Height);
        this.Box = rect;
        

        this.ClearPlate();

        // TODO: Colocar o prato numa bancada;
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

        if (!hasProtein) return;

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

        if(hasProtein)
        {
            var temp = FoodRectangle.AlignCenter(this.Box);
            this.FoodRectangle = temp;
        }
    }

}

