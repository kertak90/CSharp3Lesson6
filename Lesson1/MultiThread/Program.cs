
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MultiThread
{

//1. Даны 2 двумерных матрицы.Размерность 100х100 каждая. Напишите приложение, производящее параллельное умножение матриц. 
//    Матрицы заполняются случайными целыми числами от 0 до10.
//2. *В некой директории лежат файлы.По структуре они содержат 3 числа, разделенные пробелами. Первое число — целое, обозначает действие, 
//    1 — умножение и 2 — деление, остальные два — числа с плавающей точкой. Написать многопоточное приложение, выполняющее вышеуказанные 
//    действия над числами и сохраняющее результат в файл result.dat.Количество файлов в директории заведомо много.

    class Program
    {
        static async void Main(string[] args)
        {
            Matrix A = new Matrix(2, 2);
            Matrix B = new Matrix(2, 2);
            Matrix C = await A.multiplyMatrix(B);
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
            for(int i=0; i<_sizeX; i++)
            {
                for(int j=0; j<_sizeY; j++)
                {
                    matrix[i, j] = rnd.Next(0, 11);
                }
            }
        }

        public async Task<Matrix> multiplyMatrix (Matrix B)
        {
            if (this._sizeX != B._sizeY) return null;

            Matrix newMatrix = new Matrix(this._sizeY, B._sizeX);
            int sum = 0;
            for(int i=0;i<_sizeX;i++)
            {
                for(int j=0;j<_sizeY;j++)
                {
                    Matrix tempMatrixA = this;
                    Matrix tempMatrixB = B;
                    sum = await getMatrixElement(tempMatrixA, tempMatrixB, i, j);
                    newMatrix.matrix[i, j] = sum;
                }
            }
            return newMatrix;
        }

        public async Task<int> getMatrixElement(Matrix A, Matrix B, int x, int y)
        {
            int sum = 0;
            for(int i=0; i<_sizeX; i++)
            {
                sum += A.matrix[x, i] * B.matrix[i, y];
            }
            return sum;
        }

        public override string ToString()
        {
            var str = "";
            for(int i=0;i<_sizeX;i++)
            {
                for(int j=0;j<_sizeY;j++)
                {
                    str += this.matrix[i, j] + " ";
                }
                str += "\n";
            }
            return str;
        }
    }
}
