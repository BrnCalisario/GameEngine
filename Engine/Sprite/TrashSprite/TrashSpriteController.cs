using Engine.Extensions;
using Engine.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Sprites.TrashSprite
{

    public class TrashSpriteLoader : SpriteLoader<TrashTypes>
    {
        public TrashSpriteLoader() : base(scale:3) { }

        Size SpriteSize = new(16,16);

        protected override void Load()
        {
            var scaled = SpriteSize.Scale(this.Scale);

            int startX = 0;

            var trash = new SpriteStream();

            var rect = new Rectangle(startX, 0, scaled.Width, scaled.Height);
            Sprite sprite = new Sprite(rect.Location, rect.Size);
            trash.Add(sprite);
            this.Animations.Add(TrashTypes.Open, trash);
        }
    }


    public class TrashSpriteController
        : SpriteController<TrashSpriteLoader, TrashTypes>
    {
        public TrashSpriteController()
        { 
            this.SpriteLoader = new TrashSpriteLoader();
        }
    }

    public enum TrashTypes
    { 
        Open,
        Closed
    }

}
