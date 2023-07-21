using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    Image trashImage = Image.FromFile(AssetsPath + "trash3x.png");


    public Trash(Rectangle box, float scale = 3, Pen pen = null) : base(box, scale, pen)
    {
        this.Filled = true;
    }

    public override void Draw(Graphics g)
    {
        g.DrawImage(trashImage, Box);
    }

    public override void Interact(Player p)
    {
        if (p.IsHolding)
            p.holdingItem.Dispose();
    }

}