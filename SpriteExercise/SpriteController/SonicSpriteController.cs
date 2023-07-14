using SpriteExercise.Sprites;
using System;
using System.Windows.Forms;

namespace SpriteExercise.SpriteController;
public class SonicSpriteController : ISpriteController
{
    public SonicSpriteController()
    {
        SpriteLoader = new SonicSpriteLoader();
        animationArgsFactory = new SonicAnimationArgsFactory(SpriteLoader);

        //CurrentAnimation = SpriteLoader.GetAnimation(AnimationType.Jumping);
        //CurrentSprite = CurrentAnimation.Next();



        AnimationTimer.Tick += delegate
        {
            CurrentSprite = CurrentAnimation.Next();
        };

        this.ChangeAnimation(AnimationType.Idle);       
        //this.ChangeAnimation(AnimationType.Jumping);

        //EventHandler func = delegate
        //{
        //    AnimationTimer.Stop();
        //    MessageBox.Show("Teste");
        //};

        //this.SetOnStreamEnd(AnimationType.Jumping, func);

        AnimationTimer.Start();
    }
    public Sprite CurrentSprite { get; set; }
    Timer AnimationTimer { get; set; } = new Timer();
    SpriteStream CurrentAnimation { get; set; } = null;

    public SonicSpriteLoader SpriteLoader { get; set; }

    public AnimationArgsFactory animationArgsFactory { get; set; }

    public void StartAnimation(AnimationArgs args)
    {
        if (CurrentAnimation == args.Animation)
            return;

        CurrentAnimation = args.Animation;
        CurrentAnimation.Reset();
        CurrentSprite = CurrentAnimation.Next();

        AnimationTimer.Stop();
        AnimationTimer.Interval = args.Interval;
        AnimationTimer.Start();
    }

    public void ChangeAnimation(AnimationType type)
    {
        var args = animationArgsFactory.GetAnimationArgs(type);
        StartAnimation(args);
    }

    public void SetOnStreamEnd(AnimationType type, EventHandler action)
    {
        var args = animationArgsFactory.GetAnimationArgs(type);

        args.Animation.OnEndStream += action;
    }
}
