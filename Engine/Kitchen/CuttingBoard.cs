﻿using System.Drawing;
using Engine.Sprites;

namespace Engine;

using Engine.Extensions;
using Engine.Resource;
using Sprites;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using static Engine.Sprites.CuttingBoardSpriteLoader;
using static ProjectPaths;

public class CuttingBoard : Bench
{
    public CuttingBoard(Rectangle box, Direction dir = Direction.Bottom) : base(new Rectangle(box.Location, new(96, 48)), 1, null, dir)
    {
        CbSpriteLoader = new CuttingBoardSpriteLoader();

        CbSprite = CbSpriteLoader.GetAnimation(BoardTypes.WithKnife).Next();        
        BenchSprite = this.benchSpriteLoader.GetAnimation(BenchTypes.Bench).Next();

        var tempRect = new Rectangle(0, 0, 60, 30).AlignCenter(this.Box);
        tempRect.Y -= 5;

        this.CbRectangle = tempRect;
    }

    CuttingBoardSpriteLoader CbSpriteLoader { get; set; }
    
    Image CbImage = Resources.CuttingBoardImage;
    Rectangle CbRectangle { get; set; }
    Sprite CbSprite { get; set; }

    public TimeSpan ActionTime { get; set; } = TimeSpan.FromSeconds(1.25);

    private bool inAction { get; set; } = false;

    private DateTime? LastInteraction = null;

    private Player Interactor { get; set; }

    protected override void DrawBench(Graphics g)
    {
        base.DrawBench(g);

        CbSprite = CbSpriteLoader.GetAnimation(BoardTypes.WithKnife).Next();

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
            this.inAction = false;
            this.Interactor.canWalk = true;
            this.Interactor = null;

            this.LastInteraction = null;
        }
    }

    public override void Interact(Player p)
    {
        if (this.inAction) return;

        this.inAction = true;
        this.LastInteraction = DateTime.Now;

        Interactor = p;
        Interactor.canWalk = false;        
    }
}