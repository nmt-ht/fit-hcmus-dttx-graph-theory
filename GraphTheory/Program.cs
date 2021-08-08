using GraphTheory.Domain.Handlers;
using System;

namespace graph_theory
{
    class Program
    {
        private const string ADJACENCY_LIST_FILE_PATH = @".\Source\adjacency-list.txt";
        private const string ADJACENCY_MATRIX_FILE_PATH = @".\Source\adjacency-matrix.txt";

        private const string ADJACENCY_LIST_FILE_PATH_RESULT = @".\Source\Result\adjacency-list.txt";
        private const string ADJACENCY_MATRIX_FILE_PATH_RESULT= @".\Source\Result\adjacency-matrix.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Exercise 1:\n");
            AdjacencyMatrixBiz adjacencyMatrixBiz = new AdjacencyMatrixBiz();
            adjacencyMatrixBiz.AdjacencyMatrixHandling(ADJACENCY_MATRIX_FILE_PATH);

            Console.WriteLine("Exercise 2:\n");
            AdjacencyListBiz adjacencyListBiz = new AdjacencyListBiz();
            adjacencyListBiz.AdjacencyListHandling(ADJACENCY_LIST_FILE_PATH);

            Console.WriteLine("Exercise 3:\n");
            if(adjacencyMatrixBiz.AdjacencyMatrix != null)
            {
                var al = adjacencyMatrixBiz.ConvertToAdjacencyList(adjacencyMatrixBiz.AdjacencyMatrix);
                adjacencyListBiz.AdjacencyList = al;
                adjacencyListBiz.PrintToScreen();
                adjacencyMatrixBiz.WriteToFile(ADJACENCY_LIST_FILE_PATH_RESULT, al);
            }
            else
            {
                Console.WriteLine("Cannot convert AM to AL.");
            }
           
            Console.WriteLine("Exercise 4:\n");
            if (adjacencyListBiz.AdjacencyList != null)
            {
                var am = adjacencyListBiz.ConvertToAdjacencyMatrix(adjacencyListBiz.AdjacencyList);
                adjacencyMatrixBiz.AdjacencyMatrix = am;
                adjacencyMatrixBiz.PrintToScreen();
                adjacencyListBiz.WriteToFile(ADJACENCY_MATRIX_FILE_PATH_RESULT, am);
            }
            else
            {
                Console.WriteLine("Cannot convert AL to AM.");
            }

            Console.ReadLine();
        }
    }
}
