using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Engine;

public class Player : CollidableBody
{
    private readonly int Speed = 10;

    public Player(Rectangle box, Pen pen = null) : base(box, pen)
    {
    }

    public override void Update()
    {
        this.Move();
    }

    private void Move()
    {
        var keyMap = GameEngine.KeyMap;

        float velX = keyMap[Keys.A] ? Speed * -1 : keyMap[Keys.D] ? Speed : 0;
        float velY = keyMap[Keys.W] ? Speed * -1 : keyMap[Keys.S] ? Speed : 0;


        if(velX != 0 && velY != 0)
        { 
            velX *= 0.707f;
            velY *= 0.707f;
        }
        
        var newPos = this.IncrementPoint(velX, velY);

        this.Box = new Rectangle(newPos, this.Box.Size);
    }

    private Point IncrementPoint(float velx, float vely)
    {
        int gap = 15;

        float incX = velx;
        float incY = vely;

        if ((X + Width >= GameEngine.Width - gap && velx > 0) || (X <= 0 + gap && velx < 0))
            incX = 0;

        if ((Y + Height >= GameEngine.Height - gap && vely > 0) || (Y <= 0 + gap && vely < 0))
            incY = 0;


        return new Point((int)(X + incX), (int)(Y + incY));   
    }
}