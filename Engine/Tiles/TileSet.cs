using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Tiles;

public class TileSet
{
    public Tile[,] Set {  get; set; }

    public Point Location { get; set; }

    public TileSet(Tile[,] set)
    {
        this.Set = set;
    }

    public int Width => Set.GetLength(0);
    public int Height => Set.GetLength(1);

    private int TileSize { get; set; } = 96;
    public int BoxWidth => TileSize * Width;
    public int BoxHeight => TileSize * Height;


    public TileSet(int width, int height, Point location)
    {
        this.Location = location;
        Set = new Tile[width, height];

        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                var loc = new Point(Location.X + i * TileSize, Location.Y + j * TileSize);
                var rectTile = new Rectangle(loc, new(TileSize, TileSize));
                var t = new GridTile(rectTile);
                Set[i, j] = t;
            }
        }
    }

    public void Draw(Graphics g)
    {

        for(int i = 0; i < this.Width; i++)
        {
            for(int j = 0; j < this.Height; j++)
            {
                Set[i, j].Draw(g);
            }
        }
        g.DrawRectangle(new Pen(Color.Black), new Rectangle(Location, new(BoxWidth, BoxHeight)));
    }
}

