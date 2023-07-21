using System.Drawing;
using Engine.Sprites;


namespace Engine;

using Sprites;

using static ProjectPaths;

public class Trash : Interactable, IUnwalkable
{
    public Trash(Rectangle box) : base(new Rectangle(box.Location, new(48, 48)))
    {
        Loader = new TrashSpriteLoader();
        SpriteStream = Loader.GetAnimation(TrashTypes.Closed);
    }

    Image trashImage = Image.FromFile(AssetsPath + "trash3x.png");
    SpriteStream SpriteStream { get; set; }
    SpriteLoader<TrashTypes> Loader { get; set; }

    public bool IsNear { get; set; } = false;

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
        
        g.DrawRectangle(Pens.DarkRed, this.CollisionMask.Box);
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
        if (this.CollisionMask.IsColling(BasicEngine.Current.Player.Box))
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
        if (p.IsHolding)
            p.holdingItem.Dispose();
    }

}