using System.Drawing;
using Engine.Sprites;


namespace Engine;

using Sprites;

using static ProjectPaths;

public class Pan : Item
{
    public Pan(Rectangle r, PanTypes type) : base(new Rectangle(r.Location, new(48, 48)))
    {
        Loader = new PanSpriteLoader();
        SpriteStream = Loader.GetAnimation(type);
    }

    public Pan(Point point, PanTypes type) : base(point)
    {
        Loader = new PanSpriteLoader();
        SpriteStream = Loader.GetAnimation(type);
    }

    Image panImage = Image.FromFile(AssetsPath + "pan3x.png");
    SpriteStream SpriteStream { get; set; }
    SpriteLoader<PanTypes> Loader { get; set; }


    public override void Draw(Graphics g)
    {
        var c = SpriteStream.Sprites.Last();

        g.DrawImage(
            panImage,
            this.Box,
            c.X,
            c.Y,
            c.Width,
            c.Height,
            GraphicsUnit.Pixel
            );
    }
}

