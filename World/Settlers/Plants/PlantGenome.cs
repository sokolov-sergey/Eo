using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace World.Settlers.Plants
{

    public static class Gens
    {
        public const int None = 0;
        public const int Photosynthesis = 1;
        public const int Breed = 2;
        public const int PopulationId = 3;

    }

    public class PlantGenome : IGenome
    {
        private static Random Rnd = new Random();
        public static IGenome Defaul => InitDefault();


        private static int CodeGen(int val) => (Rnd.Next(1, 100), val).CodeGen();
        private static int CodeGen(int val, int rnd) => (rnd, val).CodeGen();

        private static IGenome InitDefault()
        {
            var d = new int[] { CodeGen(Gens.PopulationId), CodeGen(Gens.Photosynthesis, Rnd.Next(0, +15)), CodeGen(Gens.Breed) };

            return new PlantGenome(d);
        }

        private readonly IEnumerator<int> Enumerable;

        public int PopulationId { get { return InternalGenome[0].SequenceGen().level; } }
        public PlantGenome(int[] initial)
        {
            InternalGenome = initial;
            initial.AsEnumerable();
            Enumerable = InternalGenome.AsEnumerable().GetEnumerator();
        }

        public int[] InternalGenome { get; }

        public int Length => InternalGenome.Length;

        public IEnumerator GetEnumerator()
        {
            return InternalGenome.GetEnumerator();
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            return InternalGenome.AsEnumerable().GetEnumerator();
        }

        public int DistanceBetween(IGenome genome)
        {
            var d = this.PopulationId - genome.PopulationId;
            return d > 0 ? d : -d;
        }
    }
}