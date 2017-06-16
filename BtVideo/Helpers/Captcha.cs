using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace BtVideo.Helpers
{
    public class Captcha
    {
        private string text;
        private int width;
        private int height;
        private int length;
        private float fontSize = 45f;
        private Bitmap image;
        private Random random = new Random();

        public string Text
        {
            get
            {
                return this.text;
            }
        }

        public int Width
        {
            get
            {
                return this.width;
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }
        }

        public byte[] ImageData
        {
            get
            {
                byte[] result = null;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    this.image.Save(memoryStream, ImageFormat.Jpeg);
                    result = memoryStream.GetBuffer();
                }
                return result;
            }
        }

        public Captcha(int width, int height, int length = 5, float fontsize = 45f)
        {
            this.fontSize = fontsize;
            this.length = length;
            this.text = this.GenerateRandomCode();
            this.SetDimensions(width, height);
            this.GenerateImage();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.image.Dispose();
            }
        }

        private void SetDimensions(int width, int height)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException("width", width, "Argument out of range, must be greater than zero.");
            }
            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException("height", height, "Argument out of range, must be greater than zero.");
            }
            this.width = width;
            this.height = height;
        }

        private void GenerateImage()
        {
            Bitmap bitmap = new Bitmap(this.width, this.height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rectangle = new Rectangle(0, 0, this.width, this.height);
            HatchBrush hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
            graphics.FillRectangle(hatchBrush, rectangle);
            float num = (float)(rectangle.Height + 1);
            Font font;
            do
            {
                num -= 1f;
                font = new Font(FontFamily.GenericSansSerif, num, FontStyle.Bold);
            }
            while (graphics.MeasureString(this.text, font).Width > (float)rectangle.Width);
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddString(this.text, font.FontFamily, (int)font.Style, this.fontSize, rectangle, stringFormat);
            float num2 = 4f;
            PointF[] destPoints = new PointF[]
            {
                new PointF((float)this.random.Next(rectangle.Width) / num2, (float)this.random.Next(rectangle.Height) / num2),
                new PointF((float)rectangle.Width - (float)this.random.Next(rectangle.Width) / num2, (float)this.random.Next(rectangle.Height) / num2),
                new PointF((float)this.random.Next(rectangle.Width) / num2, (float)rectangle.Height - (float)this.random.Next(rectangle.Height) / num2),
                new PointF((float)rectangle.Width - (float)this.random.Next(rectangle.Width) / num2, (float)rectangle.Height - (float)this.random.Next(rectangle.Height) / num2)
            };
            Matrix matrix = new Matrix();
            matrix.Translate(0f, 0f);
            graphicsPath.Warp(destPoints, rectangle, matrix, WarpMode.Perspective, 0f);
            hatchBrush = new HatchBrush(HatchStyle.Percent10, Color.Black, Color.SkyBlue);
            graphics.FillPath(hatchBrush, graphicsPath);
            int num3 = Math.Max(rectangle.Width, rectangle.Height);
            for (int i = 0; i < (int)((float)(rectangle.Width * rectangle.Height) / 30f); i++)
            {
                int x = this.random.Next(rectangle.Width);
                int y = this.random.Next(rectangle.Height);
                int num4 = this.random.Next(num3 / 50);
                int num5 = this.random.Next(num3 / 50);
                graphics.FillEllipse(hatchBrush, x, y, num4, num5);
            }
            font.Dispose();
            hatchBrush.Dispose();
            graphics.Dispose();
            this.image = bitmap;
        }

        private string GenerateRandomCode()
        {
            Random random = new Random();
            char[] array = "1234567890".ToCharArray();
            int maxValue = array.Length - 1;
            string text = "";
            for (int i = 0; i < this.length; i++)
            {
                text += array[random.Next(0, maxValue)].ToString();
                random.NextDouble();
                random.Next(100, 1999);
            }
            return text;
        }
    }
}