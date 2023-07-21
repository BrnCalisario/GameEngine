using Engine.Sprites.FoodSprites;
using Engine.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Sprites.TrashSprite;
using Engine.Extensions;

namespace Engine;

using static ProjectPaths;

public interface IUnwalkable
{

}

public class Wall : CollidableBody, IUnwalkable
{
    public Wall(Rectangle box) : base(box, new Pen(Color.SteelBlue))
    {
        this.Filled = true;
    }
}


public class ItemBox<T> : Interactable, IUnwalkable
    where T : Item
{
    public ItemBox(Rectangle box) : base(box, 1.25f, new Pen(Color.Crimson))
    {
    }

    public override void Draw(Graphics g)
    {
        g.FillRectangle(Pen.Brush, Box);
        g.DrawRectangle(Pens.Black, CollisionMask.Box);
    }

    public override void Interact(Player p)
    {
        if(p.IsHolding)
            return;

        Item item = (T)Activator.CreateInstance(typeof(T), new object[] { Box });
        BasicEngine.Current.AddBody(item);
        item.Interact(p);
    }
}

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