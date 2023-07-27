using System;
using System.Collections.Generic;
using System.Drawing;

namespace Engine;

public class Order
{
    public Order()
    {
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public Rectangle Box { get; set; } 
    
    public OrderType Type { get; set; }
    public DateTime StartTime { get; set; } = DateTime.Now;
    public TimeSpan TimeLimit { get; set; } = TimeSpan.FromSeconds(12);
    
    public TimeBar TimeProgress { get; set; }

    public bool PassedOver => GetRemaingTime() < 0;

    public int GetRemaingTime()
    {
        return (int)(TimeLimit - (DateTime.Now - StartTime)).TotalSeconds;
    }

    public void SetTimeBar()
    {
        var rect = new Rectangle(Box.X, Box.Y + Box.Height,this.Box.Width, 10);
        TimeProgress = new TimeBar(rect, TimeLimit, true);
    }

    public void UpdateProgressPos()
    {
        var pos = new Point(Box.X, Box.Y + Box.Height);
        TimeProgress.UpdateLocation(pos);
    }
}

public class OrderTab : Body
{
    public OrderTab() 
        : base(new(), Pens.Black)
    {
        this.orders = new List<Order>();

        var gameBox = BasicEngine.Current.Box;

        var tileSetBox = BasicEngine.Current.tileSet.Box;

        var size = new Size(tileSetBox.Width, 160);

        var pos = new Point(tileSetBox.X, tileSetBox.Y - size.Height - 30);

        var rect = new Rectangle(pos, size);
        
        this.Box = rect;

        LoadPossibilities();
    }

    private void LoadPossibilities()
    {
        Possibilities = new List<Order>();

        Order o1 = new()
        { 
            Name = "Sopa de Tomate",
            Type = OrderType.TomatoSoup,    
        };
        Possibilities.Add(o1);

        Order o2 = new()
        {
            Name = "Sopa de Cebola",
            Type = OrderType.OnionSoup,
        };
        Possibilities.Add(o2);

        Order o3 = new()
        {
            Name = "Sopa Mista",
            Type = OrderType.MixedSoup,
        };
        Possibilities.Add(o3);

        Order o4 = new()
        {
            Name = "Bife Assado",
            Type = OrderType.Steak,
        };
        Possibilities.Add(o4);

        Order o5 = new()
        {
            Name = "Peixinho",
            Type = OrderType.Fish,
        };
        Possibilities.Add(o5);
    }

    private static OrderTab current = null;
    public static OrderTab Current
    {
        get
        {
            current ??= new OrderTab();
            return current;
        }
    }

    public static void New()
        => current = new OrderTab();


    private List<Order> orders;
    public IEnumerable<Order> Orders => orders;

    public List<Order> Possibilities;

    public TimeSpan OrderCoolDown { get; set; } = TimeSpan.FromSeconds(5);
    public DateTime LastOrderTime { get; set; } = DateTime.Now;

    public void CompleteOrder(Order order, bool success)
    {
        this.orders.Remove(order);
        int points = success ? 15 : -15;

        BasicEngine.Current.Points += points;
        var boxPos = Box.Location;

        for(int i = 0; i < orders.Count; i++)
        {
            var pos = new Point(boxPos.X + 15 + (130 * i + 15 * i), boxPos.Y + 15);

            orders[i].Box = new Rectangle(pos, new(130, 130));
            orders[i].UpdateProgressPos();
        }
    }

    public override void Draw(Graphics g)
    {
        var pos = this.Box.Location;

        g.DrawString("Order Tab", SystemFonts.MenuFont, Pen.Brush, pos.X, pos.Y);
        g.DrawRectangle(Pen, this.Box);

        for(int i = 0; i < orders.Count; i++)
        {
            if (orders[i].PassedOver)
            {
                CompleteOrder(orders[i], false);
                return;
            }
            
            DrawOrder(g, this.orders[i]);
        }
    }

    public override void Update()
    {
        if (orders.Count > 8) return;

        var diff = DateTime.Now - LastOrderTime;

        if (diff >= OrderCoolDown)
        {
            GetRandomOrder();
            this.LastOrderTime = DateTime.Now;
        }

        foreach (Order o in orders)
            o.TimeProgress.Update();
    }

    private void GetRandomOrder()
    {
        var randV = Random.Shared.Next(this.Possibilities.Count - 1);
        
        var pick = this.Possibilities[randV];

        var boxPos = this.Box.Location;
        var orderCount = this.orders.Count;

        var pos = new Point(boxPos.X + 15 + (130 * orderCount + 15 * orderCount), boxPos.Y + 15);

        var order = new Order()
        { 
            Name = pick.Name,
            StartTime = DateTime.Now,
            Type = pick.Type,
            Box = new Rectangle(pos, new(130, 130))
        };

        order.SetTimeBar();

        this.orders.Add(order);
    }

    private void DrawOrder(Graphics g, Order order)
    {
        var pos = order.Box.Location;

        g.DrawRectangle(Pens.Blue, order.Box);
        g.DrawString($"{order.Name}", SystemFonts.MenuFont, Pen.Brush, pos.X, pos.Y + 130 / 2 - 8);
        //g.DrawString($"{order.GetRemaingTime()}", SystemFonts.MenuFont, Pen.Brush, pos.X, pos.Y + 130 - 16);

        order.TimeProgress.Draw(g);
    }


}

public enum OrderType : byte
{
    TomatoSoup =1 ,
    OnionSoup = 2,
    MixedSoup = 4,
    Steak = 8,
    Fish = 16,
    InvalidOrder = 32,
}

public static class OrderExtensions
{
    public static bool IsProtein(this OrderType order)
        => order == OrderType.Steak || order == OrderType.Fish;
}