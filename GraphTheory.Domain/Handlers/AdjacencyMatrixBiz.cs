using GraphTheory.Domain.Models;
using System;
using System.ComponentModel;
using System.IO;
using GraphTheory.Domain.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace GraphTheory.Domain.Handlers
{
    /// <summary>
    /// Read matrix from the file, check sysmmetric and print to consonle app
    /// reference: checking the graph is cycle - https://www.geeksforgeeks.org/detect-cycle-undirected-graph/
    /// </summary>
    public class AdjacencyMatrixBiz
    {

        private const string SYMMETRIC_MATRIX = "Ma tran doi xung.";
        private const string ASYMMETRIC_MATRIX = "Ma tran khong doi xung.";
        private const string DIRECTED_GRAPH = "Do thi co huong.";
        private const string UNDIRECTED_GRAPH = "Do thi vo huong.";
        private const string COMPLETED_GRAPH = "Day la do thi day du K{0}";
        private const string UNCOMPLETED_GRAPH = "Day la khong do thi day du";

        public AdjacencyMatrix AdjacencyMatrix { get; set; }
        private AdjacencyList AdjacencyList { get; set; }
        private int LoopEdges { get; set; } = 0;
        private int TotalOfEdges { get; set; } = 0;
        public int[] DegreeArray { get; set; }
        private int TotalOfDegrees { get; set; } = 0;
        private eExerciseNumber ExerciseNumber { get; set; }
        private string VisitedVerties { get; set; } = string.Empty;
        public void SetParametter(eExerciseNumber exerciseNumber)
        {
            this.ExerciseNumber = exerciseNumber;
        }
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

            this.AdjacencyList = ConvertToAdjacencyList(this.AdjacencyMatrix);
            Console.WriteLine();
            Console.WriteLine(IsUndirectedGraph() ? UNDIRECTED_GRAPH : DIRECTED_GRAPH);
            Console.WriteLine();

            if (IsUndirectedGraph())
                NumberOfConnectedComponents();

            switch (this.ExerciseNumber)
            {
                case eExerciseNumber.One:
                    Console.WriteLine(IsUndirectedGraph() ? UNDIRECTED_GRAPH : DIRECTED_GRAPH);

                    if (!this.IsUndirectedGraph())
                        PrintInfo4DirecredGraph();
                    else
                        PrintInfo4UnDirecredGraph();

                    break;
                case eExerciseNumber.Two:
                    Console.WriteLine(IsCompeletedGraph() ? string.Format(COMPLETED_GRAPH, this.AdjacencyMatrix.N) : UNCOMPLETED_GRAPH);

                    if (IsRegularGraph())
                        Console.WriteLine($"Day la do thi chinh qui {this.DegreeArray[0]}-chinh quy");

                    Console.WriteLine(IsCycleGraph() ? $"Day la do thi vong C{this.AdjacencyMatrix.N}" : "Day khong phai la do thi vong");
                    break;
            }
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

        #region Dirrected Graph
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

        private int[] CountDegrees()
        {
            int[] degrees = new int[this.AdjacencyMatrix.N];
            for (int i = 0; i < AdjacencyMatrix.N; i++)
            {
                int count = 0;
                for (int j = 0; j < AdjacencyMatrix.N; j++)
                    if (AdjacencyMatrix.Array[i, j] != 0)
                    {
                        count += AdjacencyMatrix.Array[i, j];
                        if (i == j) // xet truong hop canh khuyen
                        {
                            count += AdjacencyMatrix.Array[i, i];
                            this.LoopEdges++;
                        }
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
            Console.WriteLine($"Phan loai do thi: {EnumExtensionMethods.GetEnumDescription((GetTypeOfGraph()))}");
        }

        private int HangingTop(int[] inDeg, int[] outDeg)
        {
            var result = 0;
            if (!IsUndirectedGraph())
            {
                for (int i = 0; i < outDeg.Length; i++)
                {
                    for (int j = 0; j < inDeg.Length; j++)
                    {
                        if (i == j)
                        {
                            var totalDeg = inDeg[j] + outDeg[i];
                            result += totalDeg == 1 ? 1 : 0;
                            break;
                        }
                    }
                }
            }
            else
            {
                if (this.DegreeArray.Length == 0)
                    return 0;

                for (int i = 0; i < this.DegreeArray.Length; i++)
                {
                    if (DegreeArray[i] == 1)
                        result++;
                }
            }

            return result;
        }

        private int CountMultipleEdge()
        {
            var count = 0;

            if (IsUndirectedGraph())
            {
                for (int i = 0; i < this.AdjacencyMatrix.N; i++)
                {
                    for (int j = i + 1; j < this.AdjacencyMatrix.N; j++)
                        count += this.AdjacencyMatrix.Array[i, j] > 1 ? 1 : 0;
                }
            }
            else
            {
                for (int i = 0; i < this.AdjacencyMatrix.N; i++)
                {
                    for (int j = i + 1; j < this.AdjacencyMatrix.N; j++)
                        count += (this.AdjacencyMatrix.Array[i, j] > 1
                            || this.AdjacencyMatrix.Array[j, i] > 1) ? 1 : 0;
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

        private eTypeOfGraph GetTypeOfGraph()
        {
            if (IsUndirectedGraph())
            {
                if (this.CountMultipleEdge() > 0 && !this.IsGraphHasNoLoops())
                    return eTypeOfGraph.MultiGraph;

                return eTypeOfGraph.PseudoGraph;
            }
            else
            {
                if (this.CountMultipleEdge() > 0)
                    return eTypeOfGraph.MultiDirectedGraph;

                return eTypeOfGraph.DirectedGraph;
            }
        }

        private void PrintInfo4UnDirecredGraph()
        {
            Console.WriteLine($"So dinh cua do thi: {this.AdjacencyMatrix.N}");
            GetTotalOfEdges();
            Console.WriteLine($"So canh cua do thi: {this.TotalOfEdges}");
            Console.WriteLine($"So cap dinh xuat hien canh boi: {this.CountMultipleEdge()}");
            Console.WriteLine($"So canh khuyen: {this.LoopEdges}");
            Console.WriteLine($"So dinh treo: {HangingTop(null, null)}");
            Console.WriteLine($"So dinh co lap: {CountIsolatedVertices(this.DegreeArray)}");

            var degreeOfEdge = string.Empty;

            for (int i = 0; i < this.DegreeArray.Length; i++)
            {
                degreeOfEdge += string.Format($"{i}({this.DegreeArray[i]})") + " ";
            }

            Console.WriteLine($"Bac cua tung dinh:\n {degreeOfEdge}");
            Console.WriteLine($"Phan loai do thi: {EnumExtensionMethods.GetEnumDescription((GetTypeOfGraph()))}");
        }

        private void GetTotalOfEdges()
        {
            this.DegreeArray = CountDegrees();

            if (this.DegreeArray.Length == 0)
                return;

            for (int i = 0; i < this.DegreeArray.Length; i++)
            {
                this.TotalOfDegrees += this.DegreeArray[i];
            }

            this.TotalOfEdges = this.TotalOfDegrees / 2;
        }
        #endregion

        #region Completed Graph
        //Total number of edges in a complete graph of N vertices = (n*(n–1))/2. n are nodes of graph.
        private int TotalOfEdges4CompletedGraph()
        {
            int n = this.AdjacencyMatrix.N;

            return n * (n - 1) / 2;
        }

        private bool IsCompeletedGraph()
        {
            var result = false;
            this.DegreeArray = CountDegrees();

            if (this.DegreeArray.Length == 0)
                return false;

            for (int i = 0; i < this.DegreeArray.Length; i++)
            {
                this.TotalOfDegrees += this.DegreeArray[i];
            }

            if ((this.TotalOfDegrees / 2) == TotalOfEdges4CompletedGraph())
                result = true;

            return result;
        }

        private bool IsRegularGraph()
        {
            var result = true;

            if (this.DegreeArray == null)
                return false;

            for (int i = 0; i < this.DegreeArray.Length; i++)
            {
                for (int j = i + 1; j < this.DegreeArray.Length; j++)
                {
                    if (this.DegreeArray[i] != this.DegreeArray[j])
                        return false;
                }
            }

            return result;
        }

        private bool IsCyclicUtil(int v, bool[] visited, int parent)
        {
            visited[v] = true;

            foreach (int i in this.AdjacencyList.AdjacentVertices[v])
            {
                if (!visited[i])
                {
                    if (IsCyclicUtil(i, visited, v))
                        return true;
                }
                else if (i != parent)
                    return true;
            }
            return false;
        }

        private bool IsCycleGraph()
        {
            if (this.AdjacencyMatrix == null || this.AdjacencyMatrix.N < 3 || !IsRegularGraph() || (this.DegreeArray.Length > 0 && this.DegreeArray[0] != 2))
                return false;

            this.AdjacencyList = ConvertToAdjacencyList(this.AdjacencyMatrix);

            bool[] visited = new bool[this.AdjacencyList.N];
            for (int i = 0; i < this.AdjacencyList.N; i++)
                visited[i] = false;

            for (int u = 0; u < this.AdjacencyList.N; u++)
            {
                // Don't recur for u if already visited
                if (!visited[u])
                    if (IsCyclicUtil(u, visited, -1))
                        return true;
            }

            return false;
        }
        #endregion

        #region MyRegion
        public void BFS(int s, int g)
        {
            bool[] visited = new bool[this.AdjacencyList.N];
            int[] parent = new int[this.AdjacencyList.N];

            this.VisitedVerties = string.Empty;
            var startSnapshot = s;
            for (int i = 0; i < this.AdjacencyList.N; i++)
            {
                parent[i] = -1;
            }

            //create queue for BFS
            Queue<int> queue = new Queue<int>();
            visited[s] = true;
            queue.Enqueue(s);
            //parent[s] = s;

            //loop through all nodes in queue
            while (queue.Count != 0 && s != g)
            {
                //Deque a vertex from queue
                s = queue.Dequeue();
                this.VisitedVerties += s + " ";

                //Get all adjacent vertices of s
                foreach (int next in AdjacencyList.AdjacentVertices[s])
                {
                    if (!visited[next])
                    {
                        visited[next] = true;
                        queue.Enqueue(next);
                        parent[next] = s;
                    }
                }
            }

            if (s == g)
            {
                Console.WriteLine("Danh sach dinh da duyet theo thu tu:");
                Console.WriteLine(VisitedVerties.Substring(0, VisitedVerties.Length - 1));

                var path = string.Empty;
                while (parent[s] != -1)
                {
                    path += s + " <- ";
                    s = parent[s];

                    if (s == startSnapshot)
                    {
                        path += s;
                        break;
                    }
                }

                Console.WriteLine(!string.IsNullOrEmpty(path) ? "Duong di in kieu nguoc:\n" + path : "Khong co duong di.");
                return;
            }
        }

        int snapshotStart = 0;
        bool isSnapshotStarted = false;
        private void DFS(int start, int goal, bool[] visited, int[] parent)
        {
            visited[start] = true;
            VisitedVerties += start + " ";

            if(!isSnapshotStarted)
            {
                snapshotStart = start;
                isSnapshotStarted = true;
            }
            
            if (start == goal)
            {
                Console.WriteLine("Danh sach dinh da duyet theo thu tu:");
                Console.WriteLine(VisitedVerties.Substring(0, VisitedVerties.Length - 1));

                var path = string.Empty;
                while (parent[start] != -1)
                {
                    path += start + " <- ";
                    start = parent[start];

                    if (start == snapshotStart)
                    {
                        path += snapshotStart;
                        break;
                    }
                }

                Console.WriteLine(!string.IsNullOrEmpty(path) ? "Duong di in kieu nguoc:\n" + path : "Khong co duong di.");
                return;
            }

            foreach (int i in this.AdjacencyList.AdjacentVertices[start])
            {
                if (!visited[i])
                {
                    parent[i] = start;
                    DFS(i, goal, visited, parent);
                }
            }
        }

        public void InitDFS(int start, int goal)
        {
            bool[] visited = new bool[AdjacencyList.N];
            int[] parent = new int[this.AdjacencyList.N];

            for (int i = 0; i < this.AdjacencyList.N; i++)
            {
                parent[i] = -1;
            }

            DFS(start, goal, visited, parent);
        }

        private void NumberOfConnectedComponents()
        {
            // Mark all the vertices as not visited
            bool[] visited = new bool[this.AdjacencyList.N];
            for (int v = 0; v < this.AdjacencyList.N; v++)
                visited[v] = false;

            for (int v = 0; v < this.AdjacencyList.N; v++)
            {
                if (visited[v] == false)
                {
                    DFSUtil(v, visited);

                    Console.WriteLine("\n");
                }
            }
        }

        void DFSUtil(int v, bool[] visited)
        {
            visited[v] = true;
            Console.Write(v + " ");

            foreach (var n in  this.AdjacencyList.AdjacentVertices[v])
            {
                if (!visited[n])
                    DFSUtil(n, visited);
            }
        }

        #endregion
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


public enum eExerciseNumber
{
    One = 1,
    Two = 2
}