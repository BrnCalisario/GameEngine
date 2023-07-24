using Engine.Extensions;
using System.Drawing;
using static Engine.Sprites.CuttingBoardSpriteLoader;
using static System.Windows.Forms.AxHost;

namespace Engine.Sprites;

public class CuttingBoardSpriteLoader : SpriteLoader<BoardTypes>
{
    public CuttingBoardSpriteLoader() : base(scale: 3) { }

    Size SpriteSize = new(22, 10);

    protected override void Load()
    {
        var scaled = SpriteSize.Scale(this.Scale);

        int startX = 0;

        var board = new SpriteStream();
        var boardKnife = new SpriteStream();

        var rectWith = new Rectangle(startX, 0, scaled.Width, scaled.Height);
        Sprite spriteWithKnife = new Sprite(rectWith.Location, rectWith.Size);
        boardKnife.Add(spriteWithKnife);
        this.Animations.Add(BoardTypes.WithKnife, boardKnife);

        startX += scaled.Width + 6;

        var rectWithout = new Rectangle(startX, 0, scaled.Width, scaled.Height);
        Sprite spriteWithoutKnife = new Sprite(rectWithout.Location, rectWithout.Size);
        board.Add(spriteWithoutKnife);
        //this.Animations.Add(BoardTypes.WithoutKnife, bor)
    }

    public enum BoardTypes
    {
        WithKnife,
        WithoutKnife
    }

}