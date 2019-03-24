using System.Drawing;
using World.Maps;

namespace VideoSystem
{
    internal class Driver : IDriver
    {
        private IMap map;
        private Size PhysicalSize;
        private int Scale=3;

        public Driver(IMap map)
        {
            this.map = map;
            PhysicalSize = new Size(map.Width*Scale, map.Height*Scale);
        }

        public void DrawMap(Graphics graphics)
        {
             
        }
    }
}