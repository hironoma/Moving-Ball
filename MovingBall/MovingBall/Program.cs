using System;
using System.Windows.Forms;
using System.Drawing;
using System.Windows;
using System.Collections.Generic;

// メインフォーム
class MainForm : Form
{
    private enum eMode
    {
        十字キー単発,
        十字キー連続,
        画面クリック誘導,
        反射45度,
        円,
        楕円,
        八の字,
        放物線,
        S波
    }

    private enum eDrive
    {
        Manual,
        Auto
    }

    private eMode mode;
    private eDrive drive;
    private PictureBox pb;
    private RadioButton[] rb = new ExRadioButton[9];
    private GroupBox gb;
    private SuperBall bl;
    private Keys key;


    // プログラム開始
    public static void Main()
    {
        Application.Run(new MainForm());
    }

    // コンストラクタ
    public MainForm()
    {

        // フォーム初期化
        this.Text = "Moving Ball";
        this.ClientSize = new Size(800, 400);
        this.BackColor = Color.DarkGray;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.KeyPreview = true;

        // ピクチャーボックス初期化
        pb = new PictureBox();
        pb.Size = new Size(600, 360);
        pb.BackColor = Color.Black;
        pb.BorderStyle = BorderStyle.Fixed3D;
        pb.Location = new Point(25, (this.ClientSize.Height - pb.Height) / 2);
        pb.Parent = this;

        // グループボックス初期化
        gb = new GroupBox();
        gb.Text = "種別";
        gb.Width = 130;
        gb.Height = 260;
        gb.Top = 25;
        gb.Left = 646;
        gb.Padding = new Padding(8, 6, 0, 0);
        gb.Parent = this;

        // ラジオボタン初期化
        for (int i = 0; i < rb.Length; i++)
        {
            rb[i] = new ExRadioButton();
            rb[i].Top = (i < 3) ? 18 + rb[i].Height * i : 34 + rb[i].Height * i;   // 三項演算子
            //rb[i].Top = 18 + rb[i].Height * i;
            rb[i].Left = 10;
            rb[i].TabIndex = i;
            rb[i].TabStop = false;
            rb[i].FlatStyle = FlatStyle.Flat;
        }

        rb[0].Text = "十字キー（単発）";
        rb[1].Text = "十字キー（連続）";
        rb[2].Text = "画面クリック誘導";
        rb[3].Text = "反射４５°";
        rb[4].Text = "円";
        rb[5].Text = "楕円";
        rb[6].Text = "８の字";
        rb[7].Text = "放物線";
        rb[8].Text = "Ｓ波";
        rb[0].Checked = true;

        for (int i = rb.Length - 1; i >= 0; i--)
        {
            gb.Controls.Add(rb[i]);
        }

        mode = eMode.十字キー単発;
        drive = eDrive.Manual;

        // セパレートライン
        Label lb = new Label();
        lb.Text = "---------------";
        lb.ForeColor = Color.Gray;
        lb.AutoSize = false;
        lb.Top = 92;
        lb.Left = 15;
        lb.Parent = gb;

        // ボール初期化
        bl = new SuperBall();
        bl.Color = Color.YellowGreen;
        bl.Width = 20;
        bl.Height = 20;
        bl.Brush = new SolidBrush(bl.Color);

        // 中心ポイント
        // bl.Point = new Point((pb.ClientSize.Width - bl.Width) / 2, (pb.ClientSize.Height - bl.Height) / 2);
        bl.Point = new Point((pb.ClientSize.Width - bl.Width) / 2 + bl.Width / 2, (pb.ClientSize.Height - bl.Height) / 2 + bl.Height / 2);

        // タイマー初期化
        Timer tm = new Timer();
        tm.Interval = 10;
        tm.Start();

        // イベントハンドラ
        pb.Paint += new PaintEventHandler(pb_Paint);
        tm.Tick += new EventHandler(tm_Tick);
        this.KeyDown += new KeyEventHandler(fm_KeyDown);
        pb.MouseClick += new MouseEventHandler(pb_MouseClick);
        for ( int i = 0; i < rb.Length; i++)
        rb[i].Click += rb_Click; // 省略形
    }


    // ピクチャーボックスペイントイベント
    private void pb_Paint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        if (drive == eDrive.Manual)
        {
            // 支点中心描画
            g.FillEllipse(bl.Brush, bl.Point.X - bl.Width / 2, bl.Point.Y - bl.Height / 2, bl.Width, bl.Height);
        } else if (drive == eDrive.Auto)
        {
            g.FillEllipse(bl.Brush, bl.Point.X, bl.Point.Y, bl.Width, bl.Height);
        }
    }

    // タイマーイベント
    private void tm_Tick(object sender, EventArgs e)
    {
        Point p = bl.Point; //移動前のポイント

        if (mode == eMode.十字キー連続)
        {
            p = bl.Move(key);
            bl.Point = bl.WallStop(p, pb.ClientSize);
        }
        else if (mode == eMode.画面クリック誘導)
        {
            bl.Point = bl.ClickGuide();
        }
        else if (mode == eMode.反射45度)
        {
            bl.Point = bl.Reflect(p, pb.ClientSize);
        }
        else if (mode == eMode.円)
        {
            bl.Point = bl.Circle(pb.ClientSize);
        }
        else if (mode == eMode.楕円)
        {
            bl.Point = bl.Ellipse(pb.ClientSize);
        }
        else if (mode == eMode.八の字)
        {
            bl.Point = bl.EightFigure(pb.ClientSize);
        }
        else if (mode == eMode.放物線)
        {
            bl.Point = bl.Parabola(pb.ClientSize);
        }
        else if (mode == eMode.S波)
        {
            bl.Point = bl.SWave(pb.ClientSize);
        }

        pb.Invalidate();
    }

    // キーダウンイベント
    private void fm_KeyDown(object sender, KeyEventArgs e)
    {
        key = e.KeyCode;

        if (mode == eMode.十字キー単発)
        {
            Point p = bl.Point;
            p = bl.Move(key);
            bl.Point = bl.WallStop(p, pb.ClientSize);
            pb.Invalidate();
        }

    }

    // ピクチャーボックスマウスクリックイベント
    private void pb_MouseClick(object sender, MouseEventArgs e)
    {
        // クリック座標取得
        bl.dx = e.X;
        bl.dy = e.Y;

        // MessageBox.Show(string.Format("{0}, {1}", e.X, e.Y));
        // double radian = Math.Atan2(moveY, moveX);  //ラジアン
        // double degree = radian * (180 / Math.PI);  //角度
        // Console.WriteLine($"ラジアン{radian}");
        // Console.WriteLine($"角度{degree}");
    }

    // ラジオボタンクリックイベント
    private void rb_Click(object sender, EventArgs e)
    {
        RadioButton r = (RadioButton)sender;

        switch (r.TabIndex)
        {
            case (int)eMode.十字キー単発:
                mode = eMode.十字キー単発;
                if (drive == eDrive.Auto)
                {
                    drive = eDrive.Manual;
                    bl.Point = new Point((pb.ClientSize.Width - bl.Width) / 2 + bl.Width / 2, (pb.ClientSize.Height - bl.Height) / 2 + bl.Height / 2);
                }
                break;
            case (int)eMode.十字キー連続:
                mode = eMode.十字キー連続;
                if (drive == eDrive.Auto)
                {
                    drive = eDrive.Manual;
                    bl.Point = new Point((pb.ClientSize.Width - bl.Width) / 2 + bl.Width / 2, (pb.ClientSize.Height - bl.Height) / 2 + bl.Height / 2);
                }
                key = Keys.None;
                break;
            case (int)eMode.画面クリック誘導:
                mode = eMode.画面クリック誘導;
                if (drive == eDrive.Auto)
                {
                    drive = eDrive.Manual;
                    bl.Point = new Point((pb.ClientSize.Width - bl.Width) / 2 + bl.Width / 2, (pb.ClientSize.Height - bl.Height) / 2 + bl.Height / 2);
                }
                // bl.Point = new Point((pb.ClientSize.Width - bl.Width) / 2 + bl.Width / 2, (pb.ClientSize.Height - bl.Height) / 2 + bl.Height / 2);
                // bl.dx = (pb.ClientSize.Width - bl.Width) / 2 + bl.Width / 2;
                // bl.dy = (pb.ClientSize.Height - bl.Height) / 2 + bl.Height / 2;
                bl.dx = bl.Point.X;
                bl.dy = bl.Point.Y;
                pb.Invalidate();
                break;
            case (int)eMode.反射45度:
                mode = eMode.反射45度;
                drive = eDrive.Auto;
                bl.Point = new Point((pb.ClientSize.Width - bl.Width) / 2 + bl.Width / 2, (pb.ClientSize.Height - bl.Height) / 2 + bl.Height / 2);
                bl.dx = 2;
                bl.dy = 2;
                break;
            case (int)eMode.円:
                mode = eMode.円;
                drive = eDrive.Auto;
                bl.Radius = 135;
                bl.Degree = 270;
                break;
            case (int)eMode.楕円:
                mode = eMode.楕円;
                drive = eDrive.Auto;
                bl.Degree = 270;
                break;
            case (int)eMode.八の字:
                mode = eMode.八の字;
                drive = eDrive.Auto;
                bl.Degree = 90;
                break;
            case (int)eMode.放物線:
                mode = eMode.放物線;
                drive = eDrive.Auto;
                // 開始座標
                bl.dx = (pb.ClientSize.Width - bl.Width) / 4;
                bl.dy = pb.ClientSize.Height;
                // 初速
                bl.spdX = 3;
                bl.spdY = -13;
                // 重力加速度
                bl.g = 0.25;
                break;
            case (int)eMode.S波:
                mode = eMode.S波;
                drive = eDrive.Auto;
                bl.dx = (pb.ClientSize.Width - bl.Width) / 6;
                bl.Degree = 90;
                break;
        }

    }


    // ラジオボタンボタンのキー操作無効（その１）
    //protected override bool ProcessDialogKey(Keys keyData)
    //{
    //    key = keyData;

    //    if (mode == eMode.十字キー単発)
    //    {
    //        Point p = bl.Point;
    //        p = bl.Move(keyData & Keys.KeyCode);
    //        bl.Point = bl.StopWall(p, pb.ClientSize);
    //        pb.Invalidate();
    //    }
    //    return true;
    //    //return base.ProcessDialogKey(keyData);
    //}
}

