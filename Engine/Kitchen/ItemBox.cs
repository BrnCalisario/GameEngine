using System;
using System.Drawing;

namespace Engine;

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