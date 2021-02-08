using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


// スーパーボール（派生クラス）
class SuperBall : Ball
{
    public int dx { get; set; }
    public int dy { get; set; }
    public double spdX { get; set; }
    public double spdY { get; set; }
    public double g { get; set; }   // 重力加速度

    public int Radius { get; set; }
    public double Degree { get; set; }

    private float[] fsin = new float[360];
    private float[] fcos = new float[360];

    // 反射４５°
    public Point Reflect(Point p, Size s)
    {
        if (p.X < 0 || p.X > s.Width - this.Width)
            dx = -dx;

        if (p.Y < 0 || p.Y > s.Height - this.Height)
            dy = -dy;

        p.X = p.X + dx;
        p.Y = p.Y + dy;

        return p;
    }

    // 円運動
    public Point Circle(Size s)
    {
        Point p = new Point();
        Degree += 1;
        double radian = Degree * Math.PI / 180;    // 度をラジアンに変換

        p.X = (int)(Radius * Math.Cos(radian)) + (s.Width - this.Width) / 2;
        p.Y = (int)(Radius * Math.Sin(radian)) + (s.Height - this.Height) / 2;

        return p;
    }


    // 楕円
    public Point Ellipse(Size s)
    {
        Point p = new Point();
        Degree -= 1;
        double radian = Degree * Math.PI / 180;

        p.X = (int)(200 * Math.Cos(radian)) + (s.Width - this.Width) / 2;
        p.Y = (int)(100 * Math.Sin(radian)) + (s.Height - this.Height) / 2;

        return p;
    }

    // ８の字
    public Point EightFigure(Size s)
    {
        Point p = new Point();
        Degree += 1;
        double radian = Degree * Math.PI / 180;

        p.X = (int)(200 * Math.Cos(radian)) + (s.Width - this.Width) / 2;
        p.Y = (int)(100 * Math.Sin(2 * radian)) + (s.Height - this.Height) / 2;

        return p;
    }

    // 放物線
    public Point Parabola(Size s)
    {
        Point p = new Point();
        
        spdY += g;   // 加速度の演算
        dx += (int)spdX;   // スピードの演算
        dy += (int)spdY;        
        p.X = dx;
        p.Y = dy;

        if (p.Y > s.Height)
        {
            // 開始座標
            dx = (s.Width - this.Width) / 4;
            dy = s.Height;
            // 初速
            spdX = 3;
            spdY = -13;
        }

        return p;
    }

    // Ｓ波
    public Point SWave(Size s)
    {
        Point p = new Point();
        Degree += 1;
        double radian = Degree * Math.PI / 180;
        dx += 2;
        p.X = dx;
        p.Y = (int)(100 * Math.Sin(4 * radian)) + (s.Height - this.Height) / 2;

        if (Degree == 270)
        {
            dx = (s.Width - this.Width) / 6;
            Degree = 90;
        }

        return p;
    }


    // 画面クリック誘導
    public Point ClickGuide()
    {
        Point p = new Point();
        int moveX = dx - base.Point.X;
        int moveY = dy - base.Point.Y;

        double radian = Math.Atan2(moveY, moveX);  //ラジアン
        // double degree = radian * (180 / Math.PI);  //角度
        // Console.WriteLine($"ラジアン{radian}");
        // int i = (i == 2) ? 1 : i;     // 三項演算子

        p = base.Point;

        if (moveX != 0)
        {
            p.X = base.Point.X + (int)(Math.Round(Math.Cos(radian)));
        }
        if (moveY != 0)
        {
            p.Y = base.Point.Y + (int)(Math.Round(Math.Sin(radian)));
        }

        return p;
    }

    //// 竜巻
    //public Point Tornado(Size s)
    //{
    //    Point p = new Point();
    //    Degree += 1;
    //    if (Degree % 5 == 1)
    //        Radius += 2;
    //    double radian = Degree * Math.PI / 180;

    //    p.X = (int)(Radius * Math.Cos(radian)) + (s.Width - this.Width) / 2;
    //    p.Y = (int)(Radius * Math.Sin(radian)) + (s.Height - this.Height) / 2;

    //    if (IsRangeOut(p, s))
    //    {
    //        Radius = 0;
    //        Degree = 0;
    //    }

    //    return p;
    //}

    //// ボールが範囲外か判定
    //private bool IsOutrange(Point p, Size s)
    //{
    //    if (p.X < 0 || p.X > s.Width + this.Width || p.Y < - this.Height || p.Y > s.Height)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //// ボールが範囲外か判定
    //private bool IsRangeOut(Point p, Size s)
    //{
    //    if (p.X <= 0 || p.X + this.Width >= s.Width  || p.Y <= 0 || p.Y + this.Height >= s.Height)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

}
