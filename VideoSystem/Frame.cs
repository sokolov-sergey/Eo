

using System.Drawing;

namespace VideoSystem
{
    public struct Frame
    {
        public Image Image { get; }
        public readonly static Frame Empty;
        public bool IsEmpty;

        static Frame()
        {
            Bitmap image = new Bitmap(100, 100);
            Graphics.FromImage(image).FillRectangle(Brushes.AntiqueWhite, 0, 0, image.Width, image.Height);
            Empty = new Frame(image);
            Empty.IsEmpty = true;
        }

        public Frame(Image image)
        {
            Image = image;
            IsEmpty = false;
        }
    }
}