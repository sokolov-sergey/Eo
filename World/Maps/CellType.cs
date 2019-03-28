using System;

namespace World.Maps
{
    [Flags]
    public enum CellType : int
    {
        Empty = 0,

        Lifeless = 1,

        Wall = 2,
        Type4 = 4,
        Type8 = 8,
        Type10 = 0x10,
        Type20 = 0X20,
        Type40 = 0x40,
        Type80 = 0x80,

        Alive = 0x1000,
        SingleCell = 0x2000,
        MultyCell = 0x4000,

        Plant = 0x8000,
        Type10000 = 0x10000,
        Type20000 = 0x20000,
        Type40000 = 0x40000,
        Type80000 = 0x80000,



        Dead = 0x8000000
    }
}