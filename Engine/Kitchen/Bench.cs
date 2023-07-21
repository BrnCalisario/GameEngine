
using System.Runtime.Versioning;
using System.Drawing;


namespace Engine;

using static ProjectPaths;
using Extensions;
using Sprites;
using Engine.Resource;

public class Bench : Interactable, IUnwalkable
{
    public Bench(Rectangle box, float scale = 1, Pen pen = null) : base(new Rectangle(box.Location, new(96, 48)), scale, pen)
    {
        this.Pen = new Pen(Color.Purple);

        benchSpriteLoader = new BenchSpriteLoader();
        BenchSprite = benchSpriteLoader.GetAnimation(BenchTypes.Bench).Next();
    }

    Image tableImage = Resources.BenchImage;

    readonly SpriteLoader<BenchTypes> benchSpriteLoader;
    Sprite BenchSprite { get; set;}   

    public Item PlacedItem { get; private set; } = null;

    public override void Draw(Graphics g)
    {
        g.DrawImage(
            tableImage,
            this.Box,
            BenchSprite.X,
            BenchSprite.Y,
            BenchSprite.Width,
            BenchSprite.Height,
            GraphicsUnit.Pixel
            );

    }

    public override void Interact(Player p)
    {
        if(p.IsHolding && PlacedItem is not null)
            return;

        if(PlacedItem is null && p.IsHolding)
        {
            PlacedItem = p.holdingItem;
            PlacedItem.Interact(p);
            var temp = PlacedItem.Box.AlignCenter(this.Box);
            PlacedItem.Box = new Rectangle(temp.X, temp.Y - 10, temp.Width, temp.Height);
            return;
        }

        if(PlacedItem is not null)
        {
            PlacedItem.Interact(p);
            PlacedItem = null;        
        }
    }
}

