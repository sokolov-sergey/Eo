using System;

namespace World.Maps
{
    [Flags]
    public enum CellType : int
    {
        Empty = 0,

        Lifeless = 1,

        Type1 = 2,
        Type4 = 4,
        Type8 = 8,
        Type10 = 0x10,
        Type20 = 0X20,
        Type40 = 0x40,
        Type80 = 0x80,

        Alive = 0x100,        
        SingleCell = 0x200,
        MultyCell = 0x400,

        Type800 =  0x800,
        Type1000 = 0x1000,
        Type2000 = 0x2000,
        Type4000 = 0x4000,
        Type8000 = 0x8000,



        Dead = 0x8000000
    }
}