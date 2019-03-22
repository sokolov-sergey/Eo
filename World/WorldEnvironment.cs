using System;
using VideoSystem;

namespace World
{
    public class WorldEnvironment
    {
        private WorldVideoSystem CurrentVideoSystem;

        public WorldEnvironment()
        {
            CurrentVideoSystem = new WorldVideoSystem();
        }

        public IViewPort GetViewPort()
        {
            return CurrentVideoSystem.GetViewPort();
        }
    }
}
