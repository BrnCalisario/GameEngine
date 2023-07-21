
using System.Drawing;


namespace Engine;

using System.Windows.Forms.VisualStyles;
using Extensions;

public class Bench : Interactable, IUnwalkable
{
    public Bench(Rectangle box, float scale = 1.15f, Pen pen = null) : base(box, scale, pen)
    {
        this.Pen = new Pen(Color.Purple);
    }

    public Item PlacedItem { get; private set; } = null;

    public override void Draw(Graphics g)
    {
        g.FillRectangle(Pen.Brush, Box);        
    }

    public override void Interact(Player p)
    {
        if(p.IsHolding && PlacedItem is not null)
            return;

        if(PlacedItem is null)
        {
            PlacedItem = p.holdingItem;
            PlacedItem.Interact(p);
            PlacedItem.Box = PlacedItem.Box.AlignCenter(this.Box);
            return;
        }

        PlacedItem.Interact(p);
        PlacedItem = null;        
    }
}

