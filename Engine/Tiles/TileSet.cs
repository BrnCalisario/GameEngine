using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Tiles;

public class TileSet
{
    public Tile[,] Set { get; set; }


    private Point location { get; set; }
    public Point Location
    {
        get => this.location;        
        set
        {            
            this.location = value;
            UpdateLocation();
        }
    }
    


    public TileSet(Tile[,] set)
    {
        this.Set = set;
    }

    public int Width => Set.GetLength(0);
    public int Height => Set.GetLength(1);

    private int TileSize { get; set; } = 96;
    public int BoxWidth => TileSize * Width;
    public int BoxHeight => TileSize * Height;

    public Rectangle Box 
    {
        set => this.Location = new Point(value.X, value.Y);
        get => new(location, new(BoxWidth, BoxHeight));
    }

    private void UpdateLocation()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                var loc = new Point(location.X + i * TileSize, location.Y + j * TileSize);
                var rectTile = new Rectangle(loc, new(TileSize, TileSize));
                var t = new GridTile(rectTile);
                Set[i, j] = t;
            }
        }
    }

    public TileSet(int width, int height, Point location)
    {
        Set = new Tile[width, height];        
        this.location = location;

        UpdateLocation();
    }

    public void Draw(Graphics g)
    {

        for (int i = 0; i < this.Width; i++)
        {
            for (int j = 0; j < this.Height; j++)
            {
                Set[i, j].Draw(g);
            }
        }
        g.DrawRectangle(new Pen(Color.Black), new Rectangle(location, new(BoxWidth, BoxHeight)));
    }
}

