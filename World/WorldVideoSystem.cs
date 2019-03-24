
using VideoSystem;
using World.Maps;

namespace World
{
    internal class WorldVideoSystem
    {
        private IViewPort VideoPort;

        public WorldVideoSystem(IMap map)
        {
            VideoPort = new WorldVideoPort(map);
        }

        internal IViewPort GetViewPort()
        {
            return VideoPort;
        }
    }
}