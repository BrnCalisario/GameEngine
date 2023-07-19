using System.Collections.Generic;
using System.Drawing;

namespace Engine.Extensions;
public static class RectangleExtension
{
    public static bool AnyPlaceMeeting<T>(this Rectangle rec, List<T> collList, out T collider)
        where T : CollidableBody
    {
        foreach (var coll in collList)
        {
            if (coll.IsColling(rec))
            {
                collider = coll;
                return true;
            }
        }

        collider = null;
        return false;
    }

    public static Rectangle AlignCenter(this Rectangle rec, Rectangle parent)
    {
        var centerPoint = new Point(parent.X + parent.Width / 2, parent.Y + parent.Height / 2);
        var relativePoint = new Point(centerPoint.X - rec.Width / 2, centerPoint.Y - rec.Height / 2);

        rec = new Rectangle(relativePoint, rec.Size);

        return rec;
    }
}


