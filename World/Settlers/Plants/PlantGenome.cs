using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace World.Settlers.Plants
{
    public static class Gens 
    {
        public const int None = 0;
        public const int Photosynthesis = 1;

    }

    public class PlantGenome : IGenome
    {


        private readonly IEnumerator<int> Enumerable;

        public PlantGenome(int[] initial)
        {
            InternalGenome = initial;
            initial.AsEnumerable();
            Enumerable = InternalGenome.AsEnumerable().GetEnumerator();
        }

        public int[] InternalGenome { get; }

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