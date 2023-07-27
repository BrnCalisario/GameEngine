using System.Drawing;

namespace Engine;

using Resource;
using Sprites;

public class CheckOut : Bench
{

    public CheckOut(Rectangle box, Direction dir = Direction.Bottom) : base(new Rectangle(box.Location, new(96, 48)), 1, null, dir)
    {
        Loader = new CheckOutSpriteLoader();
        CheckOutSprite = Loader.GetAnimation(CheckOutTypes.TransportingBelt).Next();
        SpriteStream = Loader.GetAnimation(CheckOutTypes.TransportingBelt);
    }

    readonly Image checkOutImage = Resources.CheckOutImage;
    SpriteLoader<CheckOutTypes> Loader { get; set; }
    Sprite CheckOutSprite;
    SpriteStream SpriteStream { get; set; }



    protected override void DrawBench(Graphics g)
    {
        var c = SpriteStream.Next();

        g.DrawImage(
           checkOutImage,
           this.Box,
           c.X,
           c.Y,
           c.Width,
           c.Height,
           GraphicsUnit.Pixel
           );

        g.DrawRectangle(Pens.DarkRed, this.CollisionMask.Box);
    }



   public override void Interact(Player p)
    {
        if (p.holdingItem is not Plate plate)
            return;

        if (plate.Order == OrderType.InvalidOrder)
            return;

        plate.Interact(p);
        plate.Deliver();
    }

    public override void Update()
    {
        base.Update();
    }
}