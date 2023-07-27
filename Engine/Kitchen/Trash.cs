using System.Drawing;
using Engine.Sprites;


namespace Engine;

using Engine.Resource;
using Sprites;
using Extensions;

public class Trash : Interactable, IUnwalkable
{
    public Trash(Rectangle box) : base(new Rectangle(box.Location, new(48, 48)), 1.25f)
    {
        Loader = new TrashSpriteLoader();
        SpriteStream = Loader.GetAnimation(TrashTypes.Closed);

        var rect = new Rectangle(0, 0, Width * 2, Height * 2).AlignCenter(this.Box);

        NearMask = new CollisionMask(null,  rect);
    }

    Image trashImage = Resources.TrashImage;
    SpriteStream SpriteStream { get; set; }
    SpriteLoader<TrashTypes> Loader { get; set; }

    private CollisionMask NearMask { get; set; }



    public override void Draw(Graphics g)
    {
        var c = SpriteStream.Next();

        g.DrawImage(
           trashImage,
           this.Box,
           c.X,
           c.Y,
           c.Width,
           c.Height,
           GraphicsUnit.Pixel
           );
        
        //g.DrawRectangle(Pens.DarkRed, this.NearMask.Box);
    }

    public void Open()
    {
        SpriteStream = Loader.GetAnimation(TrashTypes.Open);
    }

    public void Close()
    {
        SpriteStream = Loader.GetAnimation(TrashTypes.Closed);
    }

    public override void Update()
    {
        if (this.NearMask.IsColling(BasicEngine.Current.Player.Box))
        {
            this.Open();
        }
        else
        {
            this.Close();
        }
    }

    public override void Interact(Player p)
    {
        if (!p.IsHolding)
            return;

        var holding = p.holdingItem as Food;

        if(holding is not null)
        {
            holding.Dispose();
            return;
        }

        var pan = p.holdingItem as Pan;

        if(pan is not null)
        {
            pan.ClearPan();
            return;
        }

        var plate = p.holdingItem as Plate;

        plate.ClearPlate();
        
    }

}