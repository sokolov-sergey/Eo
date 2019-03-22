using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoSystem
{

    public class MouseCaptureVideoPort : IViewPort
    {
        private Point prevPos;
        private Point point;
        private readonly System.Threading.Timer Timer;

        public Action<Frame> FrameHandler = f => { };

        public int MaxFPS => 30;

        public MouseCaptureVideoPort()
        {
            Timer = new System.Threading.Timer(onTimerTick, null, 1000, MaxFPS + 5);
        }

        private void onTimerTick(object state)
        {
            point = Control.MousePosition;
            if (point.X - prevPos.X == 0 && point.Y - prevPos.Y == 0)
                return;
            prevPos = point;

            FrameHandler(new Frame(CaptureCursor(640, 480)));
        }

        private Image CaptureCursor(int width, int height)
        {
            int colourDepth = Screen.PrimaryScreen.BitsPerPixel;
            PixelFormat format;
            switch (colourDepth)
            {
                case 8:
                case 16:
                    format = PixelFormat.Format16bppRgb565;
                    break;

                case 24:
                    format = PixelFormat.Format24bppRgb;
                    break;

                case 32:
                    format = PixelFormat.Format32bppArgb;
                    break;

                default:
                    format = PixelFormat.Format32bppArgb;
                    break;
            }
            var captured = new Bitmap(width, height, format);
            Graphics gdi = Graphics.FromImage(captured);
            gdi.CopyFromScreen(point.X - (width / 2), point.Y - (height / 2), 0, 0, new Size(width, height));
            return captured;
        }

        public void ProvideFrames(Action<Frame> action)
        {
            FrameHandler = action;
        }
    }
}
