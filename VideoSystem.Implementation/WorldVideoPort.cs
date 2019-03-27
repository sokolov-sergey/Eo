using System;
using System.Drawing;
using System.Threading;
using World.Maps;

namespace VideoSystem.Implementation
{
    public class WorldVideoPort : IViewPort
    {
        private Timer Timer;
        private IViewDriver Driver;

        Size DeviceSize = new Size(800,600);


        public WorldVideoPort(IMap map)
        {
            Driver = new ViewDriver(map);

            Timer = new Timer(TickHadler, null, 1000, 1000 / MaxFPS + 5);
        }

        

        private void TickHadler(object state)
        {
            var bitmap = new Bitmap(DeviceSize.Width, DeviceSize.Height);
            var g = Graphics.FromImage(bitmap);

            Driver.DrawMap(g);
            Driver.DrawEnvironment(g);

            Driver.DrawCells(g);

            Driver.DrawDebug(g);

            FrameHandler(new Frame(bitmap));
        }

        public int MaxFPS => 25;

        public Action<Frame> FrameHandler { get; private set; }

        public void ProvideFrames(Action<Frame> action)
        {
            FrameHandler = action;
        }

        public void ZoomIn(int x=0)
        {
            Driver.ZoomIn(x);
        }

        public void SetDeviceSize(int width, int height)
        {
            DeviceSize = new Size(width, height);
        }
    }
}