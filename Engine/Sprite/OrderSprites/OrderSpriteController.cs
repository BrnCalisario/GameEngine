using System.Drawing;

namespace Engine.Sprites;

using Extensions;
using static System.Windows.Forms.AxHost;


public class OrderSpriteLoader : SpriteLoader<OrderType>
{
    public OrderSpriteLoader() : base(scale: 3) { }

    Size SpriteSize = new(130, 130);

    protected override void Load()
    {
        var scaled = SpriteSize.Scale(this.Scale);
        int startX = 0;

        var tomatoSoup = new SpriteStream();
        var onionSoup = new SpriteStream();
        var mixedSoup = new SpriteStream();
        var steak = new SpriteStream();
        var fish = new SpriteStream();

        var rect = new Rectangle(startX, 0, scaled.Width, scaled.Height);
        var sprite = new Sprite(rect.Location, rect.Size);
        tomatoSoup.Add(sprite);
        this.Animations.Add(OrderType.TomatoSoup, tomatoSoup);

        startX += scaled.Width;

        rect = new Rectangle(startX, 0, scaled.Width, scaled.Height);
        sprite = new Sprite(rect.Location, rect.Size);
        mixedSoup.Add(sprite);
        this.Animations.Add(OrderType.MixedSoup, mixedSoup);

        startX += scaled.Width;

        rect = new Rectangle(startX, 0, scaled.Width, scaled.Height);
        sprite = new Sprite(rect.Location, rect.Size);
        onionSoup.Add(sprite);
        this.Animations.Add(OrderType.OnionSoup, onionSoup);

        startX += scaled.Width;

        rect = new Rectangle(startX, 0, scaled.Width, scaled.Height);
        sprite = new Sprite(rect.Location, rect.Size);
        steak.Add(sprite);
        this.Animations.Add(OrderType.Steak, steak);

        startX += scaled.Width;

        rect = new Rectangle(startX, 0, scaled.Width, scaled.Height);
        sprite = new Sprite(rect.Location, rect.Size);
        fish.Add(sprite);
        this.Animations.Add(OrderType.Fish, fish);

    }
}


public class OrderSpriteController
    : SpriteController<OrderSpriteLoader, OrderType>
{
    public OrderSpriteController()
    {
        this.SpriteLoader = new OrderSpriteLoader();
    }
}
