using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine;


public class Wall : CollidableBody
{
    public Wall(Rectangle box) : base(box, new Pen(Color.SteelBlue))
    {
        this.Filled = true;
    }

    public override void Update()
    {
        
    }
}
