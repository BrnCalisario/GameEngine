using SpriteExercise.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteExercise.SpriteController;

public class SonicSpriteLoader : SpriteLoader
{
    public SonicSpriteLoader() : base() { }

    public override void LoadSprites()
    {

        int jumpHeight = 548;
        int runningHeight = 235;
        int idleHeight = 40;

        int runGap = 13;
        int idleGap = 11;

        int jumpLimit = 405;
        int idleLimit = 380;

        Size spriteSize = new Size(34, 39);

        var runningAnimation = new SpriteStream();
        for (int i = 15; i <= idleLimit; i += spriteSize.Width + runGap)
        {
            runningAnimation.Add(new Sprite()
            {
                Position = new Point(i, runningHeight),
                Size = spriteSize
            });
        }
        Animations.Add(AnimationType.Running, runningAnimation);

        var idleAnimation = new SpriteStream();
        for (int i = 15; i <= idleLimit; i += spriteSize.Width + idleGap)
        {
            idleAnimation.Add(new Sprite()
            {
                Position = new Point(i, idleHeight),
                Size = spriteSize
            });
        }
        Animations.Add(AnimationType.Idle, idleAnimation);

        var jumpingAnimation = new SpriteStream();
        for(int i = 10; i <= jumpLimit; i += 32 + 8)
        {
            jumpingAnimation.Add(new Sprite()
            {
                Position = new Point(i, jumpHeight),
                Size = new Size(32, 46)
            });
        }
        Animations.Add(AnimationType.Jumping, jumpingAnimation);
    }
}

