using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите матрицы:");

        Matrix matrixA = CreateMatrix(1);
        Matrix matrixB = CreateMatrix(2);
        Matrix matrixC = null;

        Console.WriteLine("\nМатрица A:");
        PrintMatrix(matrixA);
        Console.WriteLine("\nМатрица B:");
        PrintMatrix(matrixB);

        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\nВыберите действие с матрицами:");
            Console.Write("""

                1. Сложение матриц
                2. Вычитание матриц
                3. Умножение матриц
                4. Умножение матрицы на число
                5. Найти транспонированную матрицу
                6. Изменить матрицу
                0. Закрыть программу

                Ваш выбор: 
                """);

            switch (int.Parse(Console.ReadLine()))
            {
                default:
                    Console.WriteLine("Введите число 0-5");
                    break;


                case 0:
                    exit = true;
                    break;


                case 1:
                    matrixC = MatrixSolver.MatrixAddition(matrixA, matrixB, false);
                    if (matrixC == null) Console.WriteLine("Размеры матриц не совпадают, сложение невозможно");
                    else
                    {
                        Console.WriteLine("\nC = A + B:");
                        PrintMatrix(matrixC);
                    }
                    break;


                case 2:

                    Console.WriteLine("""

                        Выберите:
                        1. C = A - B
                        2. C = B - A
                        """);

                    while (!exit) {
                        switch (int.Parse(Console.ReadLine()))
                        {
                            default:
                                Console.WriteLine("Введите цифру 1 или 2");
                                break;
                            case 1:
                                exit = true;
                                matrixC = MatrixSolver.MatrixAddition(matrixA, matrixB, true);
                                if (matrixC == null) Console.WriteLine("Размеры матриц не совпадают, сложение невозможно");
                                else
                                {
                                    Console.WriteLine("\nC = A - B:");
                                    PrintMatrix(matrixC);
                                }
                                break;
                            case 2:
                                exit = true;
                                matrixC = MatrixSolver.MatrixAddition(matrixB, matrixA, true);
                                if (matrixC == null) Console.WriteLine("Размеры матриц не совпадают, сложение невозможно");
                                else
                                {
                                    Console.WriteLine("\nC = B - A:");
                                    PrintMatrix(matrixC);
                                }
                                break;
                        }
                    }

                    exit = false;
                    break;


                case 3:
                    Console.WriteLine("""

                        Выберите:
                        1. C = A * B
                        2. C = B * A
                        """);

                    while (!exit)
                    {
                        switch (int.Parse(Console.ReadLine()))
                        {
                            default:
                                Console.WriteLine("Введите цифру 1 или 2");
                                break;
                            case 1:
                                exit = true;
                                matrixC = MatrixSolver.MatrixMultiplication(matrixA, matrixB);
                                if (matrixC == null) Console.WriteLine("Количество столбцов первой матрицы не совпадает с количеством столбцов второй матрицы, умножение невозможно");
                                else
                                {
                                    Console.WriteLine("\nC = A * B:");
                                    PrintMatrix(matrixC);
                                }
                                break;
                            case 2:
                                exit = true;
                                matrixC = MatrixSolver.MatrixMultiplication(matrixB, matrixA);
                                if (matrixC == null) Console.WriteLine("Количество столбцов первой матрицы не совпадает с количеством столбцов второй матрицы, умножение невозможно");
                                else
                                {
                                    Console.WriteLine("\nC = B * A:");
                                    PrintMatrix(matrixC);
                                }
                                break;
                        }
                    }

                    exit = false;
                    break;

                case 4:
                    Console.WriteLine("""

                        Выберите какую матрицу умножить
                        1. A
                        2. B
                        """);

                    while (!exit)
                    {
                        switch (int.Parse(Console.ReadLine()))
                        {
                            default:
                                Console.WriteLine("Введите цифру 1 или 2");
                                break;
                            case 1:
                                {
                                    exit = true;

                                    float num = float.Parse(Console.ReadLine());

                                    MatrixSolver.MatrixMultiplication(matrixA, num);

                                    Console.WriteLine("\nA * " + num + ":");
                                    PrintMatrix(matrixA);

                                    break;
                                }
                            case 2:
                                {
                                    exit = true;

                                    float num = float.Parse(Console.ReadLine());

                                    MatrixSolver.MatrixMultiplication(matrixB, num);

                                    Console.WriteLine("\nB * " + num + ":");
                                    PrintMatrix(matrixB);
                                    
                                    break;
                                }
                        }
                    }

                    exit = false;

                    break;


                case 5:
                    Console.WriteLine("""

                        Выберите какую матрицу транспонировать:
                        1. A
                        2. B
                        """);

                    while (!exit)
                    {
                        switch (int.Parse(Console.ReadLine()))
                        {
                            default:
                                Console.WriteLine("Введите цифру 1 или 2");
                                break;
                            case 1:
                                exit = true;
                                matrixC = MatrixSolver.MatrixTransposition(matrixA);
                                
                                Console.WriteLine("\nA^T:");
                                PrintMatrix(matrixC);
                                
                                break;
                            case 2:
                                exit = true;
                                matrixC = MatrixSolver.MatrixTransposition(matrixB);
                                
                                Console.WriteLine("\nB^T:");
                                PrintMatrix(matrixC);
                                
                                break;
                        }
                    }

                    exit = false;
                    break;

                case 6:
                    Console.WriteLine("""

                        Выберите какую матрицу изменить:
                        1. A
                        2. B
                        """);

                    while (!exit)
                    {
                        switch (int.Parse(Console.ReadLine()))
                        {
                            default:
                                Console.WriteLine("Введите цифру 1 или 2");
                                break;
                            case 1:
                                exit = true;
                                EditMatrix(matrixA);

                                PrintMatrix(matrixA);

                                break;
                            case 2:
                                exit = true;
                                EditMatrix(matrixB);

                                PrintMatrix(matrixB);

                                break;
                        }
                    }

                    exit = false;
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

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                Console.WriteLine("Введите элемент [" + i + "][" + j + "]");
                mat[i, j] = float.Parse(Console.ReadLine());
            }
        }

        return new Matrix(n, m, mat);
    }

    static void EditMatrix(Matrix matrix)
    {
        Console.WriteLine("Введите размер матрицы:");
        int n = int.Parse(Console.ReadLine());
        int m = int.Parse(Console.ReadLine());

        float[,] mat = new float[n, m];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                Console.WriteLine("Введите элемент [" + i + "][" + j + "]");
                mat[i, j] = float.Parse(Console.ReadLine());
            }
        }

        matrix.setSize(n, m);
        matrix.setMatrix(mat);
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