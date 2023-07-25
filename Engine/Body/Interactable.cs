using System.Drawing;
namespace Engine;

using Extensions;
public abstract class Interactable : CollidableBody
{
    protected Interactable(Rectangle box, float scale = 2, Pen pen = null) : base(box, pen)
    {
        var scaledSize = new Size((int)(box.Width * scale),(int) (box.Height * scale));

        var collMask = new Rectangle(new(0, 0), scaledSize);
        collMask = collMask.AlignCenter(box);

        var relativePos = new Point(collMask.X - box.X, collMask.Y - box.Y);
        collMask = new Rectangle(relativePos, collMask.Size);

        SetColllisionMask(collMask);
    }

    public abstract void Interact(Player p);
}