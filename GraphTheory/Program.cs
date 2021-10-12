using GraphTheory.Domain.Handlers;
using System;

namespace graph_theory
{
    class Program
    {
        private const string ADJACENCY_LIST_FILE_PATH = @".\Source\adjacency-list.txt";
        private const string ADJACENCY_MATRIX_FILE_PATH = @".\Source\adjacency-matrix.txt";
        private const string ADJACENCY_MATRIX_FILE_PATH_2 = @".\Source\input-2.txt";

        private const string ADJACENCY_LIST_FILE_PATH_RESULT = @".\Source\Result\adjacency-list.txt";
        private const string ADJACENCY_MATRIX_FILE_PATH_RESULT= @".\Source\Result\adjacency-matrix.txt";

        static void Main(string[] args)
        {
            #region BT01
            //Console.WriteLine("Exercise 1:");
            //AdjacencyMatrixBiz adjacencyMatrixBiz = new AdjacencyMatrixBiz();
            //adjacencyMatrixBiz.SetParametter(eExerciseNumber.One);
            //adjacencyMatrixBiz.AdjacencyMatrixHandling(ADJACENCY_MATRIX_FILE_PATH);
            //Console.WriteLine();

            //Console.WriteLine("Exercise 2:");
            //AdjacencyMatrixBiz adjacencyMatrixBiz_2 = new AdjacencyMatrixBiz();
            //adjacencyMatrixBiz_2.SetParametter(eExerciseNumber.Two);
            //adjacencyMatrixBiz_2.AdjacencyMatrixHandling(ADJACENCY_MATRIX_FILE_PATH_2);
            //Console.WriteLine();
            #endregion

            #region BT002
            AdjacencyMatrixBiz adjacencyMatrixBiz = new AdjacencyMatrixBiz();
            adjacencyMatrixBiz.AdjacencyMatrixHandling(ADJACENCY_MATRIX_FILE_PATH);

            int start;
            int goal;
            Console.WriteLine($"-----DFS-----");
            Console.WriteLine($"Please input start:");
            start =  int.Parse(Console.ReadLine().ToString());
            if(start < 0)
            {
                Console.WriteLine($"Please enter start and it is greater/equal than 0:");
                start = int.Parse(Console.ReadLine().ToString());
            }
            Console.WriteLine($"Please input goal:");
            goal = int.Parse(Console.ReadLine().ToString());
            if (goal < 0)
            {
                Console.WriteLine($"Please enter goal and it is greater/equal than 0:");
                goal = int.Parse(Console.ReadLine().ToString());
            }
            
            adjacencyMatrixBiz.InitDFS(start, goal);

            Console.WriteLine();

            Console.WriteLine($"-----BFS-----");
            Console.WriteLine($"Please input start:");
            start = int.Parse(Console.ReadLine().ToString());
            if (start < 0)
            {
                Console.WriteLine($"Please enter start and it is greater/equal than 0:");
                start = int.Parse(Console.ReadLine().ToString());
            }
            Console.WriteLine($"Please input goal:");
            goal = int.Parse(Console.ReadLine().ToString());
            if (goal < 0)
            {
                Console.WriteLine($"Please enter goal and it is greater/equal than 0:");
                goal = int.Parse(Console.ReadLine().ToString());
            }

            adjacencyMatrixBiz.BFS(start, goal);

            Console.WriteLine();
            #endregion

            #region Temporary, unusing codes
            //Console.WriteLine("Exercise 2:");
            //AdjacencyListBiz adjacencyListBiz = new AdjacencyListBiz();
            //adjacencyListBiz.AdjacencyListHandling(ADJACENCY_LIST_FILE_PATH);
            //Console.WriteLine();

            //Console.WriteLine("Exercise 3: There is console result.\n Please also check the result by the following path: GraphTheory\\bin\\Debug\\net5.0\\Source\\Result\\adjacency-list.txt");
            //if (adjacencyMatrixBiz.AdjacencyMatrix != null)
            //{
            //    var al = adjacencyMatrixBiz.ConvertToAdjacencyList(adjacencyMatrixBiz.AdjacencyMatrix);
            //    adjacencyListBiz.AdjacencyList = al;
            //    adjacencyListBiz.PrintToScreen();
            //    adjacencyMatrixBiz.WriteToFile(ADJACENCY_LIST_FILE_PATH_RESULT, al);
            //}
            //else
            //{
            //    Console.WriteLine("Cannot convert AM to AL.");
            //}

            //Console.WriteLine();

            //var message = $"Exercise 4: There is console result.\n Please also check the result by the following path: GraphTheory\\bin\\Debug\\net5.0\\Source\\Result\\adjacency-matrix.txt";
            //Console.WriteLine(message);
            //if (adjacencyListBiz.AdjacencyList != null)
            //{
            //    var am = adjacencyListBiz.ConvertToAdjacencyMatrix(adjacencyListBiz.AdjacencyList);
            //    adjacencyMatrixBiz.AdjacencyMatrix = am;
            //    adjacencyMatrixBiz.PrintToScreen();
            //    adjacencyListBiz.WriteToFile(ADJACENCY_MATRIX_FILE_PATH_RESULT, am);
            //}
            //else
            //{
            //    Console.WriteLine("Cannot convert AL to AM.");
            //}

            #endregion

            Console.ReadLine();
        }
    }
}
