using System.Drawing;
using Engine.Sprites;
using System.Linq;

namespace Engine;

using Resource;
using Sprites;
using System.Collections.Generic;

public abstract class CookingTool : Item
{
    public CookingTool(Rectangle r) : base(new Rectangle(r.Location, new(48, 48)))
    {

    }

    public bool IsCooking { get; set; } = false;
    public List<Food> Ingredients { get; set; } = new List<Food>();

    public abstract bool HasCookedFood { get; }

    public OrderType OrderType { get; set; }

    protected abstract void SetOrder();

    public void Remove()
    {
        if (this.BenchParent is not null)
            this.BenchParent = null;
    }

    public override void Interact(Player p)
    {
        if (!p.IsHolding || p.holdingItem == this)
        {
            base.Interact(p);
            this.IsCooking = false;
        }

        if (p.holdingItem is not Food food)
            return;

        if (!food.Cutted)
            return;

        if (Ingredients.Count >= 3)
            return;

        food.Interact(p);
        food.Dispose();
        Ingredients.Add(food);

        this.SetOrder();
    }

    public virtual void ClearPan()
    {
        Ingredients.Clear();
        //SpriteController.StartAnimation(PanTypes.Void);
    }
}


public class Pan : CookingTool
{
    public Pan(Rectangle r) : base(new Rectangle(r.Location, new(48, 48)))
    {
        SpriteController = new PanSpriteController();
        SpriteController.StartAnimation(PanTypes.Void);
    }

    readonly Image panImage = Resources.PanImage;
    public PanSpriteController SpriteController { get; set; }

    public override bool HasCookedFood => Ingredients.Count >= 3;

    public override void Interact(Player p)
    {
        base.Interact(p);
    }



    protected override void SetOrder()
    {
        bool hasTomato = Ingredients.Any(i => i is Tomato);
        bool hasOnion = Ingredients.Any(i => i is Onion);

        (bool, bool) ingredients = (hasTomato, hasOnion);

        SetAnimation(ingredients);

        if (Ingredients.Count < 3) return;

        this.OrderType = ingredients switch
        { 
            (true, false) => OrderType.TomatoSoup,
            (false, true) => OrderType.OnionSoup,
            (true, true) => OrderType.MixedSoup,
            _ => OrderType.InvalidOrder
        };;
    }

    private void SetAnimation((bool tomato, bool onion) ingredients)
    {
        PanTypes type =   
            ingredients switch
            {
                (true, false) => PanTypes.TomatoPan,
                (false, true) => PanTypes.OnionPan,
                (true, true) => PanTypes.TomOnionPan,
                _ => PanTypes.Void            
            };
        SpriteController.StartAnimation(type);
    }

    public override void ClearPan()
    {
        base.ClearPan();
        SpriteController.StartAnimation(PanTypes.Void);
    }

    public override void Draw(Graphics g)
    {
        var c = SpriteController.GetCurrentSprite(this.IsCooking);

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

