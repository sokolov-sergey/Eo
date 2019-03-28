using World.Maps;

namespace World.Settlers
{
    public interface ISettler
    {
        Cell Cell { get; set; }
        IMap Map { get; set; }
    }
}