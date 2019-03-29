using System;
using System.Drawing;
using System.Threading;
using World.Maps;

namespace VideoSystem.Implementation
{
    public class WorldVideoPort : IViewPort
    {
        private readonly Timer Timer;
        private IViewDriver Driver;

        Size DeviceSize = new Size(800, 600);
        private int _currFps = 25;

        public WorldVideoPort(IMap map)
        {
            Driver = new ViewDriver(map);

            Timer = new Timer(TickHadler, null, 1000, 1000 / MaxFPS );
        }



        private void TickHadler(object state)
        {
            var bitmap = new Bitmap(DeviceSize.Width, DeviceSize.Height);
            var g = Graphics.FromImage(bitmap);
            try
            {
                Driver.DrawMap(g);
                Driver.DrawEnvironment(g);
                Driver.DrawCells(g);
                Driver.DrawDebug(g);
                var delay = FrameHandler(new Frame(bitmap));
                if (delay > 0)
                {
                    Timer.Change(delay * 1000, 1000 / MaxFPS);
                }
            }
            catch { }
            
        }

        public int MaxFPS { get { return _currFps; } set { SetFps(value); } }

        private void SetFps(int value)
        {
            _currFps = value;
            Timer.Change(1000,1000/MaxFPS);
        }

        public Func<Frame,int> FrameHandler { get; private set; }

        public void ProvideFrames(Func<Frame,int> action)
        {
            FrameHandler = action;            
        }

        public void ZoomIn(int x = 0)
        {
            Driver.ZoomIn(x);
        }

        public void SetDeviceSize(int width, int height)
        {
            DeviceSize = new Size(width, height);
        }
    }
}