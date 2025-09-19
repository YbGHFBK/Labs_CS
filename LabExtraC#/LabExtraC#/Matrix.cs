using System;

public class Matrix
{
    int n, m;
    float[,] matrix;

    public Matrix(int n, int m)
    {
        matrix = new float[n, m];
    }

    public void setMatrix(float[,] matrix)
    {
        this.matrix = matrix;
    }

    public float[,] getMatrix()
    {
        return matrix;
    }

}