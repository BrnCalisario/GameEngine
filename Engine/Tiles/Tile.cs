using Engine.Sprite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine;

public abstract class Tile : Body
{
    public abstract Image TileImage { get; protected set; }

    public Tile(Rectangle box, Image tileImage) : base(box, null)
    {
        this.TileImage = tileImage;
    }

    public Tile(Rectangle box) : base(box, null) {  }

    public override void Draw(Graphics g)
    {
        g.DrawImage(TileImage, Box);
    }


}

public class GridTile : Tile
{
    public override Image TileImage { get; protected set; } = Image.FromFile("../../../../assets/tileShadows.png");

    public GridTile(Rectangle box, Image tileImage) : base(box, tileImage) {  }
    public GridTile(Rectangle box) : base(box) { }

    public override void Update()
    {
        
    }
}