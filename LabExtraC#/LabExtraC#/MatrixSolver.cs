using System;

public static class MatrixSolver
{
    public static Matrix MatrixAddition(Matrix matrix1, Matrix matrix2, bool negate)
    {

        int n1 = matrix1.getSize().n, n2 = matrix2.getSize().n, m1 = matrix1.getSize().m, m2 = matrix2.getSize().m;

        if (m1 != m2 || n1 != n2)
        {
            Console.WriteLine("Размеры матриц не совпадают, сложение невозможно");
            return null;
        }

        Console.WriteLine(n1 + " " + n2 + " " + m1 + " " + m2);/////////////////

        float[,] mat1 = matrix1.getMatrix();
        float[,] mat2 = matrix2.getMatrix();
        float[,] mat3 = new float[n1, m1];

        for (int i = 0; i < n1; i++)
        {
            for (int j = 0; j < m1; j++)
            {
                Console.WriteLine(mat1[i, j]);
                if (negate) mat3[i, j] = mat1[i, j] + mat2[i, j];
                else        mat3[i, j] = mat1[i, j] + mat2[i, j];
            }
        }

        Matrix mat = new Matrix(n1, m1, mat3);

        return mat;
    }
}