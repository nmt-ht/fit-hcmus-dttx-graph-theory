using GraphTheory.Domain.Models;
using System;
using System.IO;

namespace GraphTheory.Domain.Handlers
{
    /// <summary>
    /// Read matrix from the file, check sysmmetric and print to consonle app
    /// Reference: https://theoryofprogramming.wordpress.com/2014/12/24/graph-theory-basics/
    /// https://theoryofprogramming.wordpress.com/adjacency-list-in-c-sharp/
    /// </summary>
    public class AdjacencyListBiz
    {
        private const string TWO_DIMENSIONAL_GRAPH = "Danh sach ke bieu dien do thi hai chieu.";
        private const string ONE_DIMENSIONAL_GRAPH = "Danh sach ke bieu dien do thi mot chieu.";
        public AdjacencyList AdjacencyList { get; set; }

        public void AdjacencyListHandling(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("The file path cannot be empty.");
                Console.ReadLine();
                return;
            }

            AdjacencyList = InitAdjacencyList(filePath);

            if(AdjacencyList == null)
            {
                Console.WriteLine("Cannot read content file successfully. Please check.");
                Console.ReadLine();
                return;
            }

            if(AdjacencyList.AdjacentVertices.Length == 0)
            {
                Console.WriteLine("Nothing to show.");
                Console.ReadLine();
                return;
            }

            PrintToScreen();
            Console.WriteLine(IsUndirectedGraph() ? TWO_DIMENSIONAL_GRAPH : ONE_DIMENSIONAL_GRAPH);
        }

        public AdjacencyList InitAdjacencyList(string filePath)
        {
            try
            {
                var lines = File.ReadAllLines(filePath);

                int n = int.Parse(lines[0]);
                var al = new AdjacencyList(n);

                for (int i = 0; i < n; i++)
                {
                    string[] items = lines[i + 1].Split(" ");
                    int adjacentVertexCount = int.Parse(items[0]);

                    for (int j = 0; j < adjacentVertexCount; j++)
                    {
                        al.AdjacentVertices[i].AddLast(int.Parse(items[j + 1]));
                    }
                }

                return al;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message.ToString());
                Console.ReadLine();
            }

            return null;
        }

        public AdjacencyMatrix ConvertToAdjacencyMatrix(AdjacencyList al)
        {
            int n = al.N;
            int[,] a = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int value = 0;
                    var node = al.AdjacentVertices[i].Find(j);

                    if (node != null)
                    {
                        value = 1;
                    }

                    a[i, j] = value;
                }
            }

            return new AdjacencyMatrix(n, a);
        }

        public void PrintToScreen()
        {
            Console.WriteLine(AdjacencyList.N);

            for (int i = 0; i < AdjacencyList.AdjacentVertices.Length; i++)
            {
                Console.Write(AdjacencyList.AdjacentVertices[i].Count);
                Console.Write(" ");

                foreach (var item in AdjacencyList.AdjacentVertices[i])
                {
                    Console.Write(item + " ");
                }

                Console.WriteLine();
            }
        }

        public bool IsUndirectedGraph()
        {
            bool result = true;

            for (int i = 0; i < AdjacencyList.AdjacentVertices.Length; i++)
            {
                var list = AdjacencyList.AdjacentVertices[i];

                if (list.Count == 0)
                    return false;

                foreach (var item in list)
                {
                    if (!(AdjacencyList.AdjacentVertices[item].Count > 0 && AdjacencyList.AdjacentVertices[item].Find(i) != null))
                        return false;
                }
            }

            return result;
        }

        public void WriteToFile(string filePath, AdjacencyMatrix adjacencyMatrix)
        {
            try
            {
                if (adjacencyMatrix != null)
                {
                    string[] lines = new string[adjacencyMatrix.N + 1];
                    lines[0] = adjacencyMatrix.N.ToString();
                    string line = string.Empty;

                    for (int i = 0; i < adjacencyMatrix.N; i++)
                    {
                        line = string.Empty;

                        for (int j = 0; j < adjacencyMatrix.N; j++)
                        {
                            var value = adjacencyMatrix.Array[i, j];
                            line += value.ToString() + " ";
                        }

                        lines[i + 1] = line;
                    }

                    File.WriteAllLines(filePath, lines);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message.ToString());
                Console.ReadLine();
            }
        }
    }
}
