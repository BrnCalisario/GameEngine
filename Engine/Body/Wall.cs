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

    public override void Update()
    {
        
    }
}


public class ItemBox<T> : Interactable, IUnwalkable
    where T : Item
{
    public ItemBox(Rectangle box) : base(box, new Pen(Color.LightBlue))
    {
    }

    public override void Draw(Graphics g)
    {
        g.DrawRectangle(Pen, Box);
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

    public override void Update()
    {
        
    }
}