

using VideoSystem;
using World.Maps;

namespace VideoSystem.Implementation
{
    public class WorldVideoSystem
    {
        private IViewPort VideoPort;

        public WorldVideoSystem(IMap map)
        {
            VideoPort = new WorldVideoPort(map);
        }

        public IViewPort GetViewPort()
        {
            return VideoPort;
        }
    }
}