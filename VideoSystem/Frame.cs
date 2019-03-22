

using System.Drawing;

namespace VideoSystem
{
    public class Frame
    {
        public Image Image { get; }
        public static Frame Empty;

        static Frame()
        {
            Bitmap image = new Bitmap(100, 100);
            Graphics.FromImage(image).FillRectangle(Brushes.AntiqueWhite, 0, 0, image.Width, image.Height);
            Empty = new Frame(image);
        }

        public Frame(Image image)
        {
            Image = image;
        }
    }
}