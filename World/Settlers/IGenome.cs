using System.Collections;
using System.Collections.Generic;

namespace World.Settlers
{
    public interface IGenome : IEnumerable<int>
    {
        int Length { get; }
    }
}