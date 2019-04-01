using System.Collections;
using System.Collections.Generic;

namespace World.Settlers
{
    public interface IGenome : IEnumerable<int>
    {
        int PopulationId { get; }
        int Length { get; }

        int DistanceBetween(IGenome genome);
    }

    public static class GenomExt
    {
        public static (int level, int cmd) SequenceGen(this int gen) => (gen >> 8, ((gen >> 8) << 8) ^ gen);
        public static int CodeGen(this (int lv, int cmd) sq) => (sq.lv << 8) + sq.cmd;

    }

}