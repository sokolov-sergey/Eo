using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using VideoSystem;
using Ww.GrapfComponints;


namespace Ww
{
    internal class VideoSystem
    {
        public static IViewPort GetViewPort()
        {
            return new MouseCaptureVideoPort();
            
        }
    }

   
}