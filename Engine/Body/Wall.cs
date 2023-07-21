using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine;

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
    public Trash(Rectangle box, float scale = 2, Pen pen = null) : base(box, scale, pen)
    {
        this.Filled = true;
    }

    public override void Draw(Graphics g)
    {
        g.FillRectangle(Pen.Brush, Box);
        g.DrawRectangle(Pens.Pink, CollisionMask.Box);
    }

    public override void Interact(Player p)
    {
        if (p.IsHolding)
            p.holdingItem.Dispose();
    }

}