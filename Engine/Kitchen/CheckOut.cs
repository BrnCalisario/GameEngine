using System.Drawing;
using Engine.Sprites;

namespace Engine;

using Engine.Resource;
using Sprites;
using System.Collections.Generic;
using System.Net.Mail;
using static Engine.Sprites.CuttingBoardSpriteLoader;
using static ProjectPaths;

public class CheckOut : Interactable, IUnwalkable
{

    public CheckOut(Rectangle box) : base(new Rectangle(box.Location, new(96, 48)), 1.25f)
    {
        Loader = new CheckOutSpriteLoader();
        CheckOutSprite = Loader.GetAnimation(CheckOutTypes.TransportingBelt).Next();
        SpriteStream = Loader.GetAnimation(CheckOutTypes.TransportingBelt);
    }

    SpriteLoader<CheckOutTypes> Loader { get; set; }
    public List<Food> Ingredients { get; set; } = new List<Food>();
    Image checkOutImage = Resources.CheckOutImage;
    Sprite CheckOutSprite;
    SpriteStream SpriteStream { get; set; }


    public override void Draw(Graphics g)
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
        throw new System.NotImplementedException();
    }
}