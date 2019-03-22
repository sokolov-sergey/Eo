
using VideoSystem;

namespace World
{
    internal class WorldVideoSystem
    {
        private IViewPort VideoPort;

        public WorldVideoSystem()
        {
            VideoPort = new WorldVideoPort();
        }

        internal IViewPort GetViewPort()
        {
            return VideoPort;
        }
    }
}