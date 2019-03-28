namespace World.Maps
{
    public interface IMap
    {
        int Height { get;  }
        int Width { get; }
        Cell[,] Cells { get; }

        Cell this[int x, int y ] { get; set; }
    }
}