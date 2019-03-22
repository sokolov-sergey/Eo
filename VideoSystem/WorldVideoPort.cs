using System;
using System.Drawing;
using System.Threading;

namespace VideoSystem
{
    public class WorldVideoPort : IViewPort
    {
        private Timer Timer;

        public WorldVideoPort()
        {
            Timer = new Timer(TickHadler, null,100, MaxFPS+5);
        }

        private void TickHadler(object state)
        {
            var bitmap = new Bitmap(300,300);
            var g =  Graphics.FromImage(bitmap);
            var rnd = new Random();            
            g.DrawEllipse(Pens.BlueViolet, rnd.Next(20, 300), rnd.Next(20, 300), rnd.Next(5, 20), rnd.Next(5, 20));
            FrameHandler(new Frame(bitmap));
        }

        public int MaxFPS => 25;

        public Action<Frame> FrameHandler { get; private set; }

        public void ProvideFrames(Action<Frame> action)
        {
            FrameHandler = action;
        }
    }
}