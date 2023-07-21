using Engine.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Sprites.FoodSprites
{
    public class FoodSpriteLoader : SpriteLoader<FoodTypes>
    {
        public FoodSpriteLoader() : base(scale: 3) { }


        Size SpriteSize = new(10, 10);

        protected override void Load()
        {
            var scaled = SpriteSize.Scale(this.Scale);

            int startX = 0;
            int startY = 0;

            var tomatoes = new SpriteStream();
            tomatoes.Interval = TimeSpan.FromSeconds(1);

            var onions = new SpriteStream();
            var meats = new SpriteStream();
            var fishes = new SpriteStream();

            for (int i = startX; i < scaled.Width * 4; i+= scaled.Width + 3)
            {
                var rect = new Rectangle(i, startY, scaled.Width, scaled.Height);
                var sprite = new Sprite(rect.Location, rect.Size);
                tomatoes.Add(sprite);
            }
            this.Animations.Add(FoodTypes.Tomato, tomatoes);

            startY += scaled.Height + 6;
            
            for(int i = startX; i < scaled.Width * 4; i+= scaled.Width + 3)
            {
                var rect = new Rectangle(i, startY, scaled.Width, scaled.Height);
                var sprite = new Sprite(rect.Location, rect.Size);
                onions.Add(sprite);
            }
            this.Animations.Add(FoodTypes.Onion, onions);

            startY += scaled.Height + 6;
            
            for (int i = startX; i < scaled.Width * 4; i += scaled.Width + 3)
            {
                var rect = new Rectangle(i, startY, scaled.Width, scaled.Height);
                var sprite = new Sprite(rect.Location, rect.Size);
                meats.Add(sprite);
            }
            this.Animations.Add(FoodTypes.Meat, meats);

            startY += scaled.Height + 6;
            
            for(int i = startX; i < scaled.Width * 4; i += scaled.Width + 3)
            {
                var rect = new Rectangle(i, startY, scaled.Width, scaled.Height);
                var sprite = new Sprite(rect.Location, rect.Size);
                fishes.Add(sprite);
            }
            this.Animations.Add(FoodTypes.Fish, fishes);

        }
    }

    public class FoodSpriteController
        : SpriteController<FoodSpriteLoader, FoodTypes>
    {
        public FoodSpriteController()
        {
            this.SpriteLoader = new FoodSpriteLoader();

        }
    }



    public enum FoodTypes
    {
        Tomato,
        Onion,
        Meat,
        Fish
    }

   

}
