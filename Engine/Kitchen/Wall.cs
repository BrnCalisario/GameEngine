using System;
using System.Drawing;

namespace Engine;

using Sprites;


using static ProjectPaths;

public interface IUnwalkable
{

}

public class Wall : CollidableBody, IUnwalkable
{
    public Wall(Rectangle box) : base(box, new Pen(Color.SteelBlue))
    {
        this.Filled = true;
        this.SetColllisionMask(box);
    }
}