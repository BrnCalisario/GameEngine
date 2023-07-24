using Engine.Extensions;
using Engine.Resource;
using Engine.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Kitchen;

public class Oven : Interactable, IUnwalkable
{
   
    public Oven(Rectangle box, float scale = 1, Pen pen = null) : base(new Rectangle(box.Location, new(96, 48)), scale, pen)
    {
        benchSpriteLoader = new BenchSpriteLoader();
        OvenSprite = benchSpriteLoader.GetAnimation(BenchTypes.Oven).Next();

    }

    Image ovenImage = Resources.BenchImage;
    readonly SpriteLoader<BenchTypes> benchSpriteLoader;
    
    
    Sprite OvenSprite;
    public Pan PlacedItem { get; set; }

    public override void Draw(Graphics g)
    {
        g.DrawImage(
            ovenImage,
            this.Box,
            OvenSprite.X,
            OvenSprite.Y,
            OvenSprite.Width,
            OvenSprite.Height,
            GraphicsUnit.Pixel
            );
    }



    public override void Interact(Player p)
    {
        var holding = p.holdingItem;

        if(holding as Pan is not null)
        {
            PlacedItem = holding as Pan;
            PlacedItem.Interact(p);
            var temp = PlacedItem.Box.AlignCenter(this.Box);
            PlacedItem.Box = new Rectangle(temp.X, temp.Y - 5, temp.Width, temp.Height);
            return;
        }
        
        if(holding as Food is not null)
        {
            PlacedItem.Interact(p);
            return;
        }

        if(PlacedItem is not null)
        {
            PlacedItem.Interact(p);
            PlacedItem = null;
        }
    }
}

