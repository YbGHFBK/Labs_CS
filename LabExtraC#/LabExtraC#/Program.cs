using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите матрицы:");

        Matrix matrixA = CreateMatrix(1);
        Matrix matrixB = CreateMatrix(2);

        Console.WriteLine("\nМатрица A:");
        PrintMatrix(matrixA);
        Console.WriteLine("\nМатрица B:");
        PrintMatrix(matrixB);

        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("Выберите действие с матрицами:");
            Console.WriteLine("""
                1. Сложение матриц
                2. Вычитание матриц
                3. Умножение матриц
                4. Найти транспонированную матрицу
                5. Изменить матрицу
                0. Закрыть программу
                """);

            switch (int.Parse(Console.ReadLine()))
            {
                default:
                    Console.WriteLine("Введите число 0-5");
                    break;
                case 0:
                    exit = true;
                    break;

            }
        }
    }

    static Matrix CreateMatrix(int num)
    {
        Console.WriteLine("Введите размер " + num + "-й матрицы:");
        int n = int.Parse(Console.ReadLine());
        int m = int.Parse(Console.ReadLine());

        float[,] mat = new float[n, m];

        for(int i = 0; i < n; i++)
        {
            for(int j = 0; j < m; j++)
            {
                Console.WriteLine("Введите элемент [" + i + "][" +  j + "]");
                mat[i, j] = float.Parse(Console.ReadLine());
            }
        }

        Matrix matrix = new Matrix(n, m);
        matrix.setMatrix(mat);

        return matrix;
    }

    static void PrintMatrix(Matrix matrix)
    {
        float[,] mat = matrix.getMatrix();

        for(int i = 0; i < mat.GetLength(0); i++)
        {
            for(int j = 0; j < mat.GetLength(1); j++)
            {
                Console.Write(mat[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}