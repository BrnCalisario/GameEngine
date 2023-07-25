using System.Drawing;
using Engine.Sprites;

namespace Engine;

using Engine.Resource;
using Sprites;
using System.Collections.Generic;
using System.Net.Mail;
using static Engine.Sprites.CuttingBoardSpriteLoader;
using static ProjectPaths;

public class CuttingBoard : Interactable, IUnwalkable
{
    public CuttingBoard(Rectangle box) : base(new Rectangle(box.Location, new(48, 48)), 1.25f)
    {
        Loader = new CuttingBoardSpriteLoader();
        SpriteStream = Loader.GetAnimation(BoardTypes.WithKnife);
    }

    Image cuttingBoardImage = Resources.CuttingBoardImage;
    SpriteStream SpriteStream { get; set; }
    SpriteLoader<BoardTypes> Loader { get; set; }
    public List<Food> Ingredients { get; set; } = new List<Food>();


    public override void Draw(Graphics g)
    {
        var c = SpriteStream.Next();

        g.DrawImage(
           cuttingBoardImage,
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