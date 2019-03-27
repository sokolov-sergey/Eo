using System;
using System.Collections.Generic;
using System.Text;
using World.Maps;

namespace World
{
    public interface IGod
    {
        void SettleCell(Cell cell, object p);
    }
}
