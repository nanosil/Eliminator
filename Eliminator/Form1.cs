using System;
using System.Drawing;
using System.Windows.Forms;

namespace Eliminatetor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MaximumSize = Screen.FromControl(this).Bounds.Size - new Size(50, 50);
            this.numericUpDown2.Maximum = Colors.Length;
        }
        PictureBox[,] Squares;
        Point[,] Points;
        Color[] Colors = new Color[] { Color.Red, Color.Blue,Color.Green,Color.Yellow,
            Color.Purple,Color.DarkOrange,Color.Brown,Color.White,Color.Black,Color.Aqua };
        Random Rand = new Random();
        bool Touching = false, w = false;
        int Moves = 0,asize = 10;

        private void bGo_Click(object sender, EventArgs e)
        {
            this.AutoScroll = true;
            bGo.Visible = false;
            numericUpDown1.Visible = false;
            numericUpDown2.Visible = false;
            label3.Visible = false;
            label2.Visible = true;
            buttonRe.Visible = true;
            label1.Visible = true;
            label1.Text = "Ready!";
            asize = (int)numericUpDown1.Value;
            Squares = new PictureBox[asize, asize];
            Points = new Point[asize, asize];
            int n1, n2 = 10;
            for (int x = 0; x < asize; x++)
            {
                n1 = 10;
                for (int y = 0; y < asize; y++)
                {
                    Points[x, y] = new Point(n1, n2);
                    n1 += 30;
                }
                n2 += 30;
            }
            for (int x = 0; x < asize; x++)
            {
                for (int y = 0; y < asize; y++)
                {
                    Squares[x, y] = new PictureBox();
                    Squares[x, y].BackColor = Colors[Rand.Next(0,(int)numericUpDown2.Value)];
                    Squares[x, y].Location = Points[x, y];
                    Squares[x, y].Size = new Size(25, 25);
                    Squares[x, y].Click += new EventHandler(Form1_Click);
                    Squares[x, y].Margin = new Padding(1, 1, 10, 50);
                    this.Controls.Add(Squares[x, y]);
                }
            }
        }
        private void buttonRe_Click(object sender, EventArgs e)
        {
            GC.Collect();
            this.AutoScroll = false;
            this.AutoSize = false;
            this.Size = new System.Drawing.Size(100, 100);
            Moves = 0;
            this.Controls.Clear();
            Controls.Add(buttonRe);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(bGo);
            Controls.Add(label3);
            Controls.Add(label4);
            Controls.Add(numericUpDown1);
            Controls.Add(numericUpDown2);
            this.AutoSize = true;
            bGo.Visible = true;
            numericUpDown1.Visible = true;
            numericUpDown2.Visible = true;
            label3.Visible = true;
            label2.Visible = false;
            buttonRe.Visible = false;
            label1.Visible = false;
            label2.Text = "Moves: 0";
            w = false;
        }
        private void Form1_Click(object sender, EventArgs e)
        {
            Touching = false;
            for (int x = 0; x < asize; x++)
            {
                for (int y = 0; y < asize; y++)
                {
                    if (sender.Equals(Squares[x, y]))
                    {
                        checkA(x, y);
                        if (Touching)
                        {
                            label2.Text = "Moves: " + ++Moves;
                            Squares[x, y].Dispose();
                            label1.Text = "Loading";
                            label1.Refresh();
                            move();
                        }
                        x = asize;
                        break;
                    }
                }
            }
            if (haslost())
                label1.Text = "You Lost!";
            else
                label1.Text = "Ready!";
            if (w)
                label1.Text = "You WON!!!";
        }
        private void checkA(int x, int y)
        {
            try
            {
                //up
                if (Squares[x - 1, y].Visible && Squares[x, y].BackColor.ToArgb() == Squares[x - 1, y].BackColor.ToArgb())
                {
                    Touching = true;
                    Squares[x, y].Visible = false;
                    checkA(x - 1, y);
                    Squares[x - 1, y].Dispose();
                }
            }
            catch (IndexOutOfRangeException) { }
            try
            {
                //down
                if (Squares[x + 1, y].Visible && Squares[x, y].BackColor.ToArgb() == Squares[x + 1, y].BackColor.ToArgb())
                {
                    Touching = true;
                    Squares[x, y].Visible = false;
                    checkA(x + 1, y);
                    Squares[x + 1, y].Dispose();
                }
            }
            catch (IndexOutOfRangeException) { }
            try
            {
                //right
                if (Squares[x, y + 1].Visible && Squares[x, y].BackColor.ToArgb() == Squares[x, y + 1].BackColor.ToArgb())
                {
                    Touching = true;
                    Squares[x, y].Visible = false;
                    checkA(x, y + 1);
                    Squares[x, y + 1].Dispose();
                }
            }
            catch (IndexOutOfRangeException) { }
            try
            {
                //left
                if (Squares[x, y - 1].Visible && Squares[x, y].BackColor.ToArgb() == Squares[x, y - 1].BackColor.ToArgb())
                {
                    Touching = true;
                    Squares[x, y].Visible = false;
                    checkA(x, y - 1);
                    Squares[x, y - 1].Dispose();
                }
            }
            catch (IndexOutOfRangeException) { }
        }
        private void move()
        {
            int[] num = new int[asize];
            bool b = false, c = false;
            int numdis;
            for (int x = asize - 1; x >= 0; x--)
            {
                numdis = 0;
                if (!b)
                {
                    for (int y = 0; y < asize; y++)
                    {
                        if (Squares[y, x].IsDisposed)
                            num[x]++;
                    }
                    if (num[x] == 0)
                        continue;
                    else if (num[x] == asize)
                    {
                        c = true;
                        continue;
                    }
                }
                else
                    b = false;
                for (int y = 0; y < asize; y++)
                {
                    try
                    {
                        if (!(Squares[y, x].IsDisposed) && Squares[y + 1, x].IsDisposed)
                        {
                            Squares[y, x].Location = Points[y + 1, x];
                            PictureBox p = Squares[y, x];
                            Squares[y, x] = Squares[y + 1, x];
                            Squares[y + 1, x] = p;
                            b = true;
                            if (++numdis >= num[x])
                                y = asize;
                        }
                        else
                            continue;

                    }
                    catch (IndexOutOfRangeException) { }
                }
                if (b)
                    x++;
            }
            w = true;
            for (int x = 0; x < asize; x++)
            {
                if (!(num[x] == asize))
                {
                    w = false;
                    break;
                }
            }
            if (c)
            {
                for (int x = asize - 1; x > 0; x--)
                {
                    if (num[x] == asize)
                        right(x);
                }
            }
        }
        private void right(int x)
        {
            if (x == asize - 1)
                return;
            PictureBox[] p = new PictureBox[asize];
            for (int y = 0; y < asize; y++)
                p[y] = Squares[y, x];
            for (; x < asize - 1; x++)
            {
                for (int y = 0; y < asize; y++)
                {
                    Squares[y, x + 1].Location = Points[y, x];
                    Squares[y, x] = Squares[y, x + 1];
                }
            }
            x = asize - 1;
            for (int y = 0; y < asize; y++)
                Squares[y, x] = p[y];
        }
        private bool haslost()
        {
            for (int x = 0; x < asize; x++)
            {
                for (int y = 0; y < asize; y++)
                {
                    if (Squares[x, y].IsDisposed)
                        continue;
                    try
                    {
                        if (Squares[x - 1, y].Visible && Squares[x, y].BackColor.ToArgb() == Squares[x - 1, y].BackColor.ToArgb())
                            return false;
                    }
                    catch (IndexOutOfRangeException) { }
                    try
                    {
                        if (Squares[x + 1, y].Visible && Squares[x, y].BackColor.ToArgb() == Squares[x + 1, y].BackColor.ToArgb())
                            return false;
                    }
                    catch (IndexOutOfRangeException) { }
                    try
                    {
                        if (Squares[x, y + 1].Visible && Squares[x, y].BackColor.ToArgb() == Squares[x, y + 1].BackColor.ToArgb())
                            return false;
                    }
                    catch (IndexOutOfRangeException) { }
                    try
                    {
                        if (Squares[x, y - 1].Visible && Squares[x, y].BackColor.ToArgb() == Squares[x, y - 1].BackColor.ToArgb())
                            return false;
                    }
                    catch (IndexOutOfRangeException) { }
                }
            }
            return true;
        }
    }
}