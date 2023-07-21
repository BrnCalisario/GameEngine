using System.Drawing;
using Engine.Resource;

namespace Engine;

using static ProjectPaths;

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
    public override Image TileImage { get; protected set; } = Resources.TileImage;

    public GridTile(Rectangle box, Image tileImage) : base(box, tileImage) {  }
    public GridTile(Rectangle box) : base(box) { }

    public override void Update()
    {
        
    }
}