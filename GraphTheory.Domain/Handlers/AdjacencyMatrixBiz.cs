using GraphTheory.Domain.Models;
using System;
using System.IO;

namespace GraphTheory.Domain.Handlers
{
    /// <summary>
    /// Read matrix from the file, check sysmmetric and print to consonle app
    /// </summary>
    public class AdjacencyMatrixBiz
    {
        
        private const string SYMMETRIC_MATRIX = "Ma tran doi xung.";
        private const string ASYMMETRIC_MATRIX = "Ma tran khong doi xung.";
        public AdjacencyMatrix AdjacencyMatrix { get; set; }

        public void AdjacencyMatrixHandling(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("The file path cannot be empty.");
                Console.ReadLine();
                return;
            }

            AdjacencyMatrix = InitAdjacencyMatrix(filePath);

            if(AdjacencyMatrix == null)
            {
                Console.WriteLine("Cannot read content file successfully. Please check.");
                Console.ReadLine();
                return;
            }

            if(AdjacencyMatrix.Array.Length == 0)
            {
                Console.WriteLine("Nothing to show.");
                Console.ReadLine();
                return;
            }
           
            PrintToScreen();
            Console.WriteLine(IsSymmetric() ? SYMMETRIC_MATRIX : ASYMMETRIC_MATRIX);
        }

        public AdjacencyMatrix InitAdjacencyMatrix(string filePath)
        {
            try
            {
                var lines = File.ReadAllLines(filePath);

                int n = int.Parse(lines[0]);
                int[,] arr = new int[n, n];

                for (int i = 0; i < n; i++)
                {
                    string[] row = lines[i + 1].Split(" ");

                    for (int j = 0; j < n; j++)
                    {
                        arr[i, j] = int.Parse(row[j]);
                    }
                }

                return new AdjacencyMatrix(n, arr);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message.ToString());
                Console.ReadLine();
            }

            return null;
        }

        public bool IsSymmetric()
        {
            var result = true;

            for (int i = 0; i < AdjacencyMatrix.N; i++)
            {
                for (int j = 0; j < AdjacencyMatrix.N; j++)
                {
                    if (AdjacencyMatrix.Array[i, j] != AdjacencyMatrix.Array[j, i])
                        return false;
                }
            }

            return result;
        }

        public void PrintToScreen()
        {
            Console.WriteLine(AdjacencyMatrix.N);

            for (int i = 0; i < AdjacencyMatrix.N; i++)
            {
                for (int j = 0; j < AdjacencyMatrix.N; j++)
                {
                    Console.Write(AdjacencyMatrix.Array[i, j]);
                    Console.Write(" ");
                }

                Console.WriteLine("");
            }
            
            Console.WriteLine("\n");
        }

        public AdjacencyList ConvertToAdjacencyList(AdjacencyMatrix am)
        {
            var al = new AdjacencyList(am.N);

            for (int i = 0; i < am.N; i++)
            {
                for (int j = 0; j < am.N; j++)
                {
                    var value = am.Array[i, j];
                    if (value > 0)
                        al.AdjacentVertices[i].AddLast(j);
                }
            }

            return al;
        }

        public void WriteToFile(string filePath, AdjacencyList adjacencyList)
        {
            try
            {
                if (adjacencyList != null)
                {
                    string[] lines = new string[adjacencyList.N + 1];
                    lines[0] = adjacencyList.N.ToString();
                    string line = string.Empty;

                    for (int li = 0; li < adjacencyList.N; li++)
                    {
                        var item = adjacencyList.AdjacentVertices[li];
                        line = item.Count.ToString() + " ";

                        foreach (var value in item)
                        {
                            line += value.ToString() + " ";
                        }

                        lines[li + 1] = line;
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
