using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace caja {
    public partial class contenedor : Form{

        public contenedor(String textLabel)
        {
            InitializeComponent();
            InitializeCustomStyles(textLabel);
        }

        private int borderSize = 2;
        private int borderRadius = 20;
        private Color borderColor = ColorTranslator.FromHtml("#0061AB");

        //Properties
        [Category("RJ Code Advance")]
        public int BorderSize
        {
            get { return borderSize; }
            set
            {
                borderSize = value;
                this.Invalidate();
            }
        }
        [Category("RJ Code Advance")]
        public int BorderRadius
        {
            get { return borderRadius; }
            set
            {
                borderRadius = value;
                this.Invalidate();
            }
        }
        [Category("RJ Code Advance")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                this.Invalidate();
            }
        }
        [Category("RJ Code Advance")]
        public Color BackgroundColor
        {
            get { return this.BackColor; }
            set { this.BackColor = value; }
        }
        [Category("RJ Code Advance")]
        public Color TextColor
        {
            get { return this.ForeColor; }
            set { this.ForeColor = value; }
        }
        private void InitializeCustomStyles(String textLabel) {
            StartPosition = FormStartPosition.CenterScreen;
            rjButton1.BackColor = ColorTranslator.FromHtml("#0061AB");
            label1.Text = textLabel;
            this.BackColor = ColorTranslator.FromHtml("#F8F8F9");
            pictureBox1.Left = (ClientSize.Width - pictureBox1.Width) / 2;
        }
            private GraphicsPath GetFigurePath(Rectangle rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 175, 85);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 85);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }
        private void contenedor_Load(object sender, EventArgs e)
        {
        }
        private void rjButton1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(ColorTranslator.FromHtml("#0061AB"));     
            System.Windows.Forms.Panel panel = sender as System.Windows.Forms.Panel;
            System.Drawing.Rectangle rect = panel.ClientRectangle;
            rect.Width--;
            rect.Height--;

            Rectangle rectBorder = Rectangle.Inflate(rect, -borderSize, -borderSize);

            using (GraphicsPath pathSurface = GetFigurePath(rect, borderRadius))
            using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - borderSize))
            //using (Pen penSurface = new Pen(this.Parent.BackColor, smoothSize))
            using (Pen penBorder = new Pen(borderColor, borderSize))

            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                //Draw control border
                e.Graphics.DrawPath(penBorder, pathBorder);
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }
    }
}
