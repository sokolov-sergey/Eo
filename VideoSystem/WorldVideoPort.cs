using System;
using System.Drawing;
using System.Threading;
using World.Maps;

namespace VideoSystem
{
    public class WorldVideoPort : IViewPort
    {
        private Timer Timer;
        IDriver Driver;

        public WorldVideoPort(IMap map)
        {
            Driver = new Driver(map);

            Timer = new Timer(TickHadler, null, 100, 1000 / MaxFPS + 5);
        }

        private void TickHadler(object state)
        {
            var bitmap = new Bitmap(800, 600);
            var g = Graphics.FromImage(bitmap);

            Driver.DrawMap(g);


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