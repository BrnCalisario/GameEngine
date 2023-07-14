using SpriteExercise;
using SpriteExercise.SpriteController;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class Player
{
    Image spriteImg = Image.FromFile("C:\\Users\\bruno\\Desktop\\Winforms\\Studies\\GameEngine\\SpriteExercise\\assets\\images\\sonic_sprite.png");

    int speed = 20;

    SonicSpriteController spriteController { get; set; }
    Size Size { get; set; } = new Size(34 * 4, 39 * 4);
    Point Position { get; set; } = new Point(200, 200);
    Rectangle RectangleBody { get; set; }



    bool direction = true;

    bool jumping = false;

    public Player()
    {
        RectangleBody = new Rectangle(Position, Size);
        
        spriteController = new SonicSpriteController();

        EventHandler afterJump = delegate
        {
            this.jumping = false;
            this.spriteController.ChangeAnimation(AnimationType.Idle);
        };

        spriteController.SetOnStreamEnd(AnimationType.Jumping, afterJump);
    }

    public void Update()
    {
        this.Walk();
    }

    public void Draw(Graphics g)
    {
        if(!this.direction && !this.jumping)
            this.invertDraw(g);

        this.draw(g);

        if (!this.direction && !this.jumping)
            this.invertDraw(g);
    }

    private void draw(Graphics g)
    {
        var currentSpr = this.spriteController.CurrentSprite;

        g.DrawImage(
           this.spriteImg,
           this.RectangleBody,
           currentSpr.X,
           currentSpr.Y,
           currentSpr.Width,
           currentSpr.Height,
           GraphicsUnit.Pixel
       );
    }

    private void invertDraw(Graphics g)
    {
        g.TranslateTransform(Form1.FormWidth / 2, 0);
        g.ScaleTransform(-1, 1);
        g.TranslateTransform(-Form1.FormWidth / 2, 0);

        this.Position = new Point(Form1.FormWidth - RectangleBody.X - RectangleBody.Width, this.Position.Y);

        this.RectangleBody = new Rectangle(this.Position, this.Size);
    }

    public void Walk()
    {
        var keyMap = Form1.KeyMap;

        if (keyMap is null)
            return;

        //if (this.jumping)
        //    return;

        if(keyMap[Keys.W])
        {
            this.spriteController.ChangeAnimation(AnimationType.Jumping);
            this.jumping = true;
            return;
        }

        if (keyMap[Keys.D] || keyMap[Keys.A])
            this.direction = keyMap[Keys.D] || !keyMap[Keys.A];

        //float Vy = keyMap[Keys.W] ? speed * -1 : keyMap[Keys.S] ? speed : 0;
        float Vy = 0;
        float Vx = keyMap[Keys.A] ? speed * -1 : keyMap[Keys.D] ? speed : 0;

        if ((Vy == 0 && Vx == 0) && !jumping)
        {
            this.spriteController.ChangeAnimation(AnimationType.Idle);
            return;
        }

        if (Vy != 0 && Vx != 0)
        {
            Vy *= 0.75f;
            Vx *= 0.75f;
        }

        if ((Vy != 0 || Vx != 0) && !jumping)
            this.spriteController.ChangeAnimation(AnimationType.Running);

        float oldX = this.Position.X;
        float oldY = this.Position.Y;

        this.Position = Point.Round(new PointF(oldX + Vx, oldY + Vy));

        this.RectangleBody = new Rectangle(Point.Round(this.Position), this.Size);
    }
}

