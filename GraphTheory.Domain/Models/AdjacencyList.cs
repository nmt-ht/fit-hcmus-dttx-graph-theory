using System.Collections.Generic;

namespace GraphTheory.Domain.Models
{
    /// <summary>
    /// Reference: https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.linkedlist-1?view=net-5.0
    /// We can use List/Array instead. However, LinkedList may get more convenience in the future for this exercise.
    /// </summary>
    public class AdjacencyList
    {
        public AdjacencyList(int n)
        {
            N = n;
            AdjacentVertices = new LinkedList<int>[n];

            for (int i = 0; i < AdjacentVertices.Length; i++)
            {
                AdjacentVertices[i] = new LinkedList<int>();
            }
        }

        public int N { get; set; }
        public LinkedList<int>[] AdjacentVertices { get; set; }
    }
}
