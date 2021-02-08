using System;
using System.Windows.Forms;
using System.Drawing;

// ボール（基本クラス）
class Ball
{
    public Point Point { get; set; } = new Point(0, 0);
    public Color Color { get; set; } = Color.White;
    public int Width = 10;
    public int Height = 10;  
    public Brush Brush;
    public int Speed = 1;

    // コンストラクタ
    public Ball()
    {
        Brush = new SolidBrush(this.Color);
    }

    // 矢印キーで移動
    public Point Move(Keys k)
    {
        Point p = Point;
        switch (k)
        {
            case Keys.Up:
                p.Y -= this.Speed;
                break;

            case Keys.Down:
                p.Y += this.Speed;
                break;

            case Keys.Right:
                p.X += this.Speed;
                break;

            case Keys.Left:
                p.X -= this.Speed;
                break;
        }

        return p;
    }

    // 壁に当たったら止まる
    public Point WallStop(Point p, Size s)
    {
        // 支点右上の場合
        //if (p.X < 0 || p.X > s.Width - this.Width)
        //    p.X = this.Point.X;
        //if (p.Y < 0 || p.Y > s.Height - this.Height)
        //    p.Y = this.Point.Y;

        if (p.X < this.Width / 2 || p.X > s.Width - this.Width / 2)
            p.X = this.Point.X;
        if (p.Y < this.Height / 2 || p.Y > s.Height - this.Height / 2)
            p.Y = this.Point.Y;
        return p;
    }


}
