using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MatrixCalculate
{
    class Program
    {
        static async void Main(string[] args)
        {
            Matrix A = new Matrix(10, 10);
            Matrix B = new Matrix(10, 10);
            Matrix C = await A.multiplyMatrixAsync(B);
            Console.WriteLine(A.ToString());
            Console.WriteLine(B.ToString());
            Console.WriteLine(C.ToString());
            Console.ReadLine();
        }
    }

    public class Matrix
    {
        private int _sizeX;
        private int _sizeY;
        public int[,] matrix;

        public Matrix(int sizeX, int sizeY)
        {
            _sizeX = sizeX;
            _sizeY = sizeY;
            matrix = new int[sizeX, sizeY];
            initializeMatrix();
        }

        private void initializeMatrix()
        {
            Random rnd = new Random();
            for (int i = 0; i < _sizeX; i++)
            {
                for (int j = 0; j < _sizeY; j++)
                {
                    matrix[i, j] = rnd.Next(0, 11);
                }
            }
        }

        public async Task<Matrix> multiplyMatrixAsync(Matrix B)
        {
            if (this._sizeX != B._sizeY) return null;
            var tasks = new List<Task>();

            Matrix newMatrix = new Matrix(this._sizeY, B._sizeX);
            int sum = 0;
            for (int i = 0; i < _sizeX; i++)
            {
                for (int j = 0; j < _sizeY; j++)
                {
                    Matrix tempMatrixA = this;
                    Matrix tempMatrixB = B;
                    tasks.Add(Task.Run(() => newMatrix.matrix[i, j] = getMatrixElement(tempMatrixA, tempMatrixB, i, j)));
                }
            }
            await Task.WhenAll(tasks);
            return newMatrix;
        }

        public int getMatrixElement(Matrix A, Matrix B, int x, int y)
        {
            Console.WriteLine($"Поток: {Thread.CurrentThread.ManagedThreadId} начинает работу");
            int sum = 0;
            for (int i = 0; i < _sizeX; i++)
            {
                sum += A.matrix[x, i] * B.matrix[i, y];
            }
            Console.WriteLine($"Поток: {Thread.CurrentThread.ManagedThreadId} закончил работу");
            return sum;
        }

        public override string ToString()
        {
            var str = "";
            for (int i = 0; i < _sizeX; i++)
            {
                for (int j = 0; j < _sizeY; j++)
                {
                    str += this.matrix[i, j] + " ";
                }
                str += "\n";
            }
            return str;
        }
    }
}
