using System.Drawing;
using Engine.Sprites;

namespace Engine;

using Engine.Resource;
using Sprites;
using System.Collections.Generic;
using System.Windows.Forms;

public class CheckOut : Bench
{

    public CheckOut(Rectangle box, Direction dir = Direction.Bottom) : base(new Rectangle(box.Location, new(96, 48)), 1, null, dir)
    {
        Loader = new CheckOutSpriteLoader();
        CheckOutSprite = Loader.GetAnimation(CheckOutTypes.TransportingBelt).Next();
        SpriteStream = Loader.GetAnimation(CheckOutTypes.TransportingBelt);
    }

    SpriteLoader<CheckOutTypes> Loader { get; set; }
    public Plate plate { get; set; }
    
    Image checkOutImage = Resources.CheckOutImage;
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
        MessageBox.Show("Interagiu!");
        if (p.holdingItem is not Plate holding)
            return;

        this.plate = holding;

        MessageBox.Show(holding.Ingredients.Count.ToString());

        holding.ClearPlate();
    }

    public override void Update()
    {
        base.Update();
    }
}