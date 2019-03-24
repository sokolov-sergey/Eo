using System;
using VideoSystem;
using World.Maps;

namespace World
{
    public class WorldEnvironment
    {
        private WorldVideoSystem CurrentVideoSystem;
        private IMap Map;

        public WorldEnvironment()
        {
            Map = new Map();


            CurrentVideoSystem = new WorldVideoSystem(Map);
        }

        public IViewPort GetViewPort()
        {
            return CurrentVideoSystem.GetViewPort();
        }
    }
}
