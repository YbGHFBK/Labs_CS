using System;

public class Matrix
{
    int n, m;
    float[,] matrix;

    public Matrix(int n, int m)
    {
        this.n = n;
        this.m = m;
        matrix = new float[n, m];
    }

    public Matrix(int n, int m, float[,] matrix) : this(n, m)
    {
        this.matrix = matrix;
    }

    public void setMatrix(float[,] matrix)
    {
        this.matrix = matrix;
    }

    public float[,] getMatrix()
    {
        return matrix;
    }

    public (int n, int m) getSize()
    {
        return (this.n, this.m);
    }

}