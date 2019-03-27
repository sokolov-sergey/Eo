using World.Maps;

namespace World
{
    public class TheGod : IGod
    {
        private readonly IMap map;

        public TheGod(IMap map)
        {
            this.map = map;
        }

        public void SettleCell(Cell cell, object p)
        {
            
        }
    }
}