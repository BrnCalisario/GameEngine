using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SpriteExercise.Sprites;

namespace SpriteExercise.SpriteController;

public interface ISpriteController
{
    void StartAnimation(AnimationArgs args);
    void SetOnStreamEnd(AnimationType type, EventHandler action);
}