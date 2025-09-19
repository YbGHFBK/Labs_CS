using System;

public static class MatrixSolver
{
    public static Matrix MatrixAddition(Matrix matrix1, Matrix matrix2, bool negate)
    {

        int n1 = matrix1.getSize().n, n2 = matrix2.getSize().n, m1 = matrix1.getSize().m, m2 = matrix2.getSize().m;

        if (m1 != m2 || n1 != n2) return null;

        float[,] mat1 = matrix1.getMatrix();
        float[,] mat2 = matrix2.getMatrix();
        float[,] mat3 = new float[m1, n1];

        for (int i = 0; i < n1; i++)
        {
            for (int j = 0; j < m1; j++)
            {
                if (negate) mat3[i, j] = mat1[i, j] - mat2[i, j];
                else        mat3[i, j] = mat1[i, j] + mat2[i, j];
            }
        }

        return new Matrix(n1, m1, mat3);
    }

    public static Matrix MatrixMultiplication(Matrix matrix1, Matrix matrix2)
    {
        int n1 = matrix1.getSize().n, n2 = matrix2.getSize().n, m1 = matrix1.getSize().m, m2 = matrix2.getSize().m;

        if(m1 != n2) return null;

        float[,] mat1 = matrix1.getMatrix();
        float[,] mat2 = matrix2.getMatrix();
        float[,] mat3 = new float[n1, m2];

        for (int i = 0; i < n1; i++)
        {
            for (int j = 0; j < m2; j++)
            {

                float sum = 0f;
                for (int k = 0; k < m1; k++)
                {
                    sum += mat1[i, k] * mat2[k, j];
                }
                mat3[i, j] = sum;

            }
        }

        return new Matrix(n2, m1, mat3);
    }

    public static Matrix MatrixTransposition(Matrix matrix1)
    {
        float[,] mat1 = matrix1.getMatrix();

        (int n, int m) = matrix1.getSize();

        float[,] mat2 = new float[m, n];

        for(int i = 0; i < m; i++)
        {
            for(int j = 0; j < n; j++)
            {
                mat2[i, j] = mat1[j, i];
            }
        }

        return new Matrix(m, n, mat2);
    }
}