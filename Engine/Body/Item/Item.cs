using Engine.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine;

public abstract class Item : CollidableBody
{
    public Item(Rectangle box) : base(box)
    {
        var collMask = new Rectangle(0, 0,  box.Width * 4, box.Height * 4);
        collMask = collMask.AlignCenter(box);

        var relativePos = new Point(collMask.X - box.X, collMask.Y - box.Y);
        collMask = new Rectangle(relativePos, collMask.Size);

        this.SetColllisionMask(collMask);
    }

    private DateTime LastInteraction = DateTime.Now;
    private TimeSpan InteractionDelay = TimeSpan.FromSeconds(0.25);

    public bool Interactable => CanInteract();

    public Player Player { get; private set; } = null;
    public bool BeingHold => Player != null;


    public override void Draw(Graphics g)
    {
        g.DrawRectangle(Pen, Box);

        if (CollisionMask is not null)
            g.DrawRectangle(new Pen(Color.Black), this.CollisionMask.Box);
    }

    public override void Update()
    {
        if(BeingHold)        
            this.Box = this.Box.AlignCenter(Player.Box);
        
        this.CollisionMask.UpdatePoint(this.Box.Location);
    }

    public void GetHold(Player p)
    {
        if (this.Player is not null || !CanInteract())
            return;

        this.Player = p;
        p.holdingItem = this;
        LastInteraction = DateTime.Now;
    }

    public void GetReleased()
    {
        if (!CanInteract())
            return;


        this.Box = new Rectangle(this.Box.Left, Player.Bottom, this.Box.Width, this.Box.Height );

        this.Player = null;
        LastInteraction = DateTime.Now;

    }

    private bool CanInteract()
    {
        var diff = DateTime.Now - LastInteraction;

        return diff >= InteractionDelay;
    }
}



public class Tomato : Item
{
    public Tomato(Point p) : base(new Rectangle(p, new(20, 20)))
    {
        this.Pen = new Pen(Color.Red);
    }
}