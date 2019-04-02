using System;
using System.Collections.Generic;

namespace World
{
    
    public interface IStatistics
    {
        IReadOnlyDictionary<string,int> Aggregations { get; }
        void Agg(string key, int val);
    }
}