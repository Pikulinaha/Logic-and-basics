using System;

namespace GraphLab
{
    class Program
    {
        static int[,] CreateAdjacencyMatrix(int n, double edgeProbability)
        {
            int[,] matrix = new int[n, n];
            Random rand = new Random();

            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {
                    double randomValue = rand.NextDouble();
                    if (randomValue < edgeProbability)
                    {
                        matrix[i, j] = 1;
                        matrix[j, i] = 1;
                    }
                }
            }
            return matrix;
        }

        static void PrintMatrix(int[,] matrix, int n)
        {
            Console.WriteLine($"Матрица смежности ({n}x{n}):");
            Console.Write("   ");
            for (int i = 0; i < n; i++)
            {
                Console.Write($"{i + 1,2} ");
            }
            Console.WriteLine();

            for (int i = 0; i < n; i++)
            {
                Console.Write($"{i + 1,2} ");
                for (int j = 0; j < n; j++)
                {
                    Console.Write($"{matrix[i, j],2} ");
                }
                Console.WriteLine();
            }
        }

        static int GraphSize(int[,] matrix, int n)
        {
            int edges = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {
                    if (matrix[i, j] == 1)
                    {
                        edges++;
                    }
                }
            }
            return edges;
        }

        static int VertexDegree(int[,] matrix, int n, int vertex)
        {
            int degree = 0;

            for (int i = 0; i < n; i++)
            {
                if (matrix[vertex, i] == 1)
                {
                    if (vertex == i)
                    {
                        degree += 2;
                    }
                    else
                    {
                        degree += 1;
                    }
                }
            }
            return degree;
        }

        static void PrintVertexDegrees(int[,] matrix, int n)
        {
            Console.WriteLine("\nСтепени вершин:");
            for (int i = 0; i < n; i++)
            {
                int degree = VertexDegree(matrix, n, i);
                Console.WriteLine($"Вершина {i + 1}: степень {degree}");
            }
        }
        static bool IsDominantVertex(int[,] matrix, int n, int vertex)
        {

            for (int i = 0; i < n; i++)
            {
                if (i != vertex && matrix[vertex, i] != 1)
                {
                    return false;
                }
            }
            return true;
        }

        static void FindSpecialVertices(int[,] matrix, int n)
        {
            Console.WriteLine("\nСпециальные вершины:");


            Console.Write("Изолированные вершины (степень 0): ");
            bool hasIsolated = false;
            for (int i = 0; i < n; i++)
            {
                if (VertexDegree(matrix, n, i) == 0)
                {
                    Console.Write($"{i + 1} ");
                    hasIsolated = true;
                }
            }
            if (!hasIsolated) Console.Write("нет");
            Console.WriteLine();


            Console.Write("Концевые вершины (степень 1): ");
            bool hasEnd = false;
            for (int i = 0; i < n; i++)
            {
                if (VertexDegree(matrix, n, i) == 1)
                {
                    Console.Write($"{i + 1} ");
                    hasEnd = true;
                }
            }
            if (!hasEnd) Console.Write("нет");
            Console.WriteLine();

            Console.Write($"Доминирующие вершины (соединены со всеми остальными): ");


            bool hasDominant = false;
            for (int i = 0; i < n; i++)
            {
                if (IsDominantVertex(matrix, n, i))
                {
                    Console.Write($"{i + 1} ");
                    hasDominant = true;
                }
            }
            if (!hasDominant) Console.Write("нет");
            Console.WriteLine();
        }

        static void ShowMenu()
        {
            Console.WriteLine("\n=== МЕНЮ ===");
            Console.WriteLine("1 - Показать матрицу смежности");
            Console.WriteLine("2 - Вывести размер графа G, изолированные, концевые и доминирующие вершины");
            Console.WriteLine("3 - Ввести новое количество вершин");
            Console.WriteLine("4 - Закрыть программу");
            Console.Write("Выберите опцию: ");
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Random rand = new Random();

            int n = 0;
            int[,] adjMatrix = null;
            double edgeProbability = 0.0;

            Console.WriteLine("\n=== Генератор неориентированного графа ===\n");


            while (true)
            {
                Console.Write("Введите количество вершин: ");
                if (int.TryParse(Console.ReadLine(), out n) && n > 0)
                {
                    break;
                }
                Console.WriteLine("Ошибка: количество вершин должно быть положительным числом.");
            }


            while (true)
            {
                Console.Write("Введите вероятность ребра (от 0.1 до 0.9): ");
                if (double.TryParse(Console.ReadLine(), out edgeProbability) &&
                    edgeProbability >= 0.1 && edgeProbability <= 0.9)
                {
                    break;
                }
                Console.WriteLine("Ошибка: вероятность должна быть числом от 0.1 до 0.9.");
            }
            Console.WriteLine($"Сгенерированная вероятность ребра: {edgeProbability:F2} ({(edgeProbability * 100):F0}%)");

            adjMatrix = CreateAdjacencyMatrix(n, edgeProbability);

            int choice;
            do
            {
                ShowMenu();
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine();
                            PrintMatrix(adjMatrix, n);
                            break;

                        case 2:
                            Console.WriteLine();
                            int graphEdges = GraphSize(adjMatrix, n);
                            Console.WriteLine($"\nРазмер графа G: |E(G)| = {graphEdges} ребер");
                            PrintVertexDegrees(adjMatrix, n);
                            FindSpecialVertices(adjMatrix, n);
                            break;

                        case 3:
                            while (true)
                            {
                                Console.Write("\nВведите новое количество вершин: ");
                                if (int.TryParse(Console.ReadLine(), out n) && n > 0)
                                {
                                    break;
                                }
                                Console.WriteLine("Ошибка: количество вершин должно быть положительным числом.");
                            }


                            while (true)
                            {
                                Console.Write("Введите вероятность ребра (от 0.1 до 0.9): ");
                                if (double.TryParse(Console.ReadLine(), out edgeProbability) &&
                                    edgeProbability >= 0.1 && edgeProbability <= 0.9)
                                {
                                    break;
                                }


                                Console.WriteLine("Ошибка: вероятность должна быть числом от 0.1 до 0.9.");
                            }
                            Console.WriteLine($"Сгенерированная вероятность ребра: {edgeProbability:F2} ({(edgeProbability * 100):F0}%)");
                            adjMatrix = CreateAdjacencyMatrix(n, edgeProbability);
                            Console.WriteLine("Новый граф создан успешно!");
                            break;

                        case 4:
                            Console.WriteLine("\nПрограмма завершена...");
                            break;

                        default:
                            Console.WriteLine("Неверный выбор! Пожалуйста, выберите опцию от 1 до 4.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ввод! Пожалуйста, введите число от 1 до 4.");
                }

            } while (choice != 4);
        }
    }
}