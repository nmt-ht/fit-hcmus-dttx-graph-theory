using GraphTheory.Domain.Models;
using System;
using System.ComponentModel;
using System.IO;
using GraphTheory.Domain.Helper;

namespace GraphTheory.Domain.Handlers
{
    /// <summary>
    /// Read matrix from the file, check sysmmetric and print to consonle app
    /// </summary>
    public class AdjacencyMatrixBiz
    {

        private const string SYMMETRIC_MATRIX = "Ma tran doi xung.";
        private const string ASYMMETRIC_MATRIX = "Ma tran khong doi xung.";
        private const string DIRECTED_GRAPH = "Do thi co huong.";
        private const string UNDIRECTED_GRAPH = "Do thi vo huong.";

        public AdjacencyMatrix AdjacencyMatrix { get; set; }
        private int LoopEdges { get; set; } = 0;
        private int TotalOfEdges { get; set; } = 0;

        public void AdjacencyMatrixHandling(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("The file path cannot be empty.");
                Console.ReadLine();
                return;
            }

            AdjacencyMatrix = InitAdjacencyMatrix(filePath);

            if (AdjacencyMatrix == null)
            {
                Console.WriteLine("Cannot read content file successfully. Please check.");
                Console.ReadLine();
                return;
            }

            if (AdjacencyMatrix.Array.Length == 0)
            {
                Console.WriteLine("Nothing to show.");
                Console.ReadLine();
                return;
            }

            PrintToScreen();
            Console.WriteLine(IsUndirectedGraph() ? UNDIRECTED_GRAPH : DIRECTED_GRAPH);
            
            if(!this.IsUndirectedGraph())
                PrintInfo4DirecredGraph();
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

        public bool IsUndirectedGraph()
        {
            var isSymmetric = true;

            for (int i = 0; i < AdjacencyMatrix.N; i++)
            {
                for (int j = 0; j < AdjacencyMatrix.N; j++)
                {
                    if (AdjacencyMatrix.Array[i, j] != AdjacencyMatrix.Array[j, i])
                        return false;
                }
            }

            return isSymmetric;
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

        private int[] CountDegrees()
        {
            int[] degrees = new int[this.AdjacencyMatrix.N]; // Mảng chứa bậc của các đỉnh
            for (int i = 0; i < AdjacencyMatrix.N; i++)
            {
                int count = 0;
                for (int j = 0; j < AdjacencyMatrix.N; j++)
                    if (AdjacencyMatrix.Array[i, j] != 0)
                    {
                        count += AdjacencyMatrix.Array[i, j];
                        if (i == j) // xet truong hop canh khuyen
                            count += AdjacencyMatrix.Array[i, i];
                    }
                degrees[i] = count;
            }
            return degrees;
        }

        public void PrintInfo4DirecredGraph()
        {
            int n = this.AdjacencyMatrix.N;

            Console.WriteLine($"So dinh cua do thi: {n}");

            int[] inDeg = new int[n];
            int[] outDeg = new int[n];

            for (int i = 0; i < n; i++)
            {
                int count = 0;
                for (int j = 0; j < n; j++)
                {
                    if (this.AdjacencyMatrix.Array[i, j] != 0)
                    {
                        count++;
                        if (i == j) //Canh khuyen
                        { 
                            count++;
                            LoopEdges++;
                        }
                    }
                }

                outDeg[i] = count;
            }

            for (int i = 0; i < n; i++)
            {
                int count = 0;
                for (int j = 0; j < n; j++)
                {
                    if (this.AdjacencyMatrix.Array[j, i] != 0)
                    {
                        count++;
                        if (i == j && outDeg[i] > 0)
                        {
                            outDeg[i] = outDeg[i] - 1;
                        }
                    }
                }

                inDeg[i] = count;
                this.TotalOfEdges += count;
            }

            Console.WriteLine($"So canh cua do thi: {this.TotalOfEdges}");
            Console.WriteLine($"So cap dinh xuat hien canh boi: {this.CountMultipleEdge()}");
            Console.WriteLine($"So canh khuyen: {this.LoopEdges}");
            Console.WriteLine($"So dinh treo: {HangingTop(inDeg, outDeg)}");
            Console.WriteLine($"So dinh co lap: {CountIsolatedVertices(inDeg)}");

            var inOutDegStr = string.Empty;
            for (int z = 0; z < inDeg.Length; z++)
            {
                for (int h = 0; h < outDeg.Length; h++)
                {
                    if (h == z)
                    {
                        inOutDegStr += string.Format($"{h}({inDeg[z]}-{outDeg[h]})") + " ";
                        break;
                    }
                }
            }

            Console.WriteLine($"Bac vao - bac ra cua tung dinh:\n {inOutDegStr}");
            Console.WriteLine($"Phan loai do thi: {EnumExtensionMethods.GetEnumDescription((TypeOfGraph()))}");
        }

        private int HangingTop(int[] inDeg, int[] outDeg)
        {
            var result = 0;
            if (!IsUndirectedGraph())
            {
                for(int i= 0; i < outDeg.Length; i++)
                {
                    for (int j = 0; j < inDeg.Length; j++)
                    {
                        if(i == j)
                        {
                            var totalDeg = inDeg[j] + outDeg[i];
                            result += totalDeg == 1 ? 1 : 0;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private int CountMultipleEdge()
        {
            var count = 0;

            if (IsUndirectedGraph())
            {
                for(int i = 0; i < this.AdjacencyMatrix.N; i++)
                {
                    for(int j = i+1; j < this.AdjacencyMatrix.N; j++)
                        count = this.AdjacencyMatrix.Array[i, j] > 1 ? 1 : 0;
                }
            }
            else
            {
                for (int i = 0; i < this.AdjacencyMatrix.N; i++)
                {
                    for (int j = i + 1; j < this.AdjacencyMatrix.N; j++)
                        count = (this.AdjacencyMatrix.Array[i, j] > 1 
                            || this.AdjacencyMatrix.Array[j,i] > 1) ? 1 : 0;
                }
            }

            return count;
        }

        private int CountIsolatedVertices(int[] degrees)
        {
            // đếm số giá trị degrees[0], degrees[1], … bằng 0
            int count = 0;

            for (int i = 0; i < AdjacencyMatrix.N; i++)
                if (degrees[i] == 0)
                    count = count + 1;

            return count;
        }

        private bool IsGraphHasNoLoops()
        {
            // kiểm tra các giá trị a[0][0], a[1][1], … xem có giá
            // trị khác 0 hay không, nếu có thì đồ thị có khuyên
            for (int i = 0; i < AdjacencyMatrix.N && AdjacencyMatrix.Array[i, i] == 0; i++)
                if (i < AdjacencyMatrix.N)
                    return false;

            return true;
        }

        private eTypeOfGraph TypeOfGraph()
        {
            if (IsUndirectedGraph())
            {
                if (this.CountMultipleEdge() > 0)
                    return eTypeOfGraph.MultiGraph;
            }
            else
            {
                if (this.CountMultipleEdge() > 0)
                    return eTypeOfGraph.MultiDirectedGraph;

                return eTypeOfGraph.DirectedGraph;
            }

            return eTypeOfGraph.Unknown;
        }
    }
}

public enum eTypeOfGraph
{
    [Description("Khong xac dinh")]
    Unknown = 0,
    [Description("Don do thi")]
    SingleGraph = 1,
    [Description("Da do thi")]
    MultiGraph = 2,
    [Description("Gia do thi")]
    PseudoGraph = 3,
    [Description("Do thi co huong")]
    DirectedGraph = 4,
    [Description("Do thi vo huong")]
    UndirectedGraph = 5,
    [Description("Da do thi co huong")]
    MultiDirectedGraph = 5
}
