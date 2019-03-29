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

    }

    public class PlantGenome : IGenome
    {
        public static readonly IGenome Defaul = InitDefault();

        private static int CodeGen(int val) => (0x0F << 8) + val;


        private static IGenome InitDefault()
        {
            var d = new int[] {CodeGen(Gens.Photosynthesis), CodeGen(Gens.Breed)  };

            return new PlantGenome(d);
        }

        private readonly IEnumerator<int> Enumerable;

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
    }
}