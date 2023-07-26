using System;
using System.Drawing;

namespace Engine;

using Sprites;
using Extensions;
using Resource;


public class CuttingBoard : Bench
{
    public CuttingBoard(Rectangle box, Direction dir = Direction.Bottom) : base(new Rectangle(box.Location, new(96, 48)), 1, null, dir)
    {
        CbSpriteLoader = new CuttingBoardSpriteLoader();

        CbSprite = CbSpriteLoader.GetAnimation(BoardTypes.WithKnife).Next();
        BenchSprite = this.benchSpriteLoader.GetAnimation(BenchTypes.Bench).Next();

        var tempRect = new Rectangle(0, 0, 60, 30).AlignCenter(this.Box);
        
        if(dir != Direction.Right)
        {
            tempRect.Y -= 5;
        }
        this.CbRectangle = tempRect;
    }

    CuttingBoardSpriteLoader CbSpriteLoader { get; set; }
    
    Image CbImage = Resources.CuttingBoardImage;
    Rectangle CbRectangle { get; set; }
    Sprite CbSprite { get; set; }

    public TimeSpan ActionTime { get; set; } = TimeSpan.FromSeconds(3);

    private bool inAction { get; set; } = false;

    private DateTime? LastInteraction = null;

    private Player Interactor { get; set; }

    protected override void DrawBench(Graphics g)
    {
        base.DrawBench(g);

        g.DrawImage(
            CbImage,
            CbRectangle,
            CbSprite.X,
            CbSprite.Y,
            CbSprite.Width,
            CbSprite.Height,
            GraphicsUnit.Pixel
        );
    }

    protected override void CorrectVertical()
    {
        base.CorrectVertical();
        this.CbRectangle = CorrectPosition(CbRectangle);
    }

    public override void Update()
    {
        if (LastInteraction is null)
            return;

        var diff = DateTime.Now - LastInteraction;

        if (diff >= ActionTime)
        {
            var food = this.Interactor.holdingItem as Food;
            food.Cutted = true;

            this.Interactor.holdingItem = food;

            this.inAction = false;
            this.Interactor.Cutting = false;
            
            this.Interactor = null;
            this.LastInteraction = null;

            CbSprite = CbSpriteLoader.GetAnimation(BoardTypes.WithKnife).Next();
        }
    }

    public override void Interact(Player p)
    {
        if (this.inAction) return;

        if (p.holdingItem is not Food food)
            return;

        if (food.Cutted)
            return;

        this.inAction = true;
        this.LastInteraction = DateTime.Now;
        CbSprite = CbSpriteLoader.GetAnimation(BoardTypes.WithoutKnife).Next();
        
        Interactor = p;
        Interactor.Cutting = true;
    }
}