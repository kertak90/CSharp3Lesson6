using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace ThreadFileReader
{
    //2. *В некой директории лежат файлы.По структуре они содержат 3 числа, разделенные пробелами. Первое число — целое, обозначает действие, 
    //    1 — умножение и 2 — деление, остальные два — числа с плавающей точкой. Написать многопоточное приложение, выполняющее вышеуказанные 
    // 
    class Program
    {
        static void Main(string[] args)
        {
            FileAriphmeticOperation obj = new FileAriphmeticOperation();
            //obj.CreateFiles();
            obj.ReadFiles(".");
            Console.ReadLine();
        }
    }
    public class FileAriphmeticOperation
    {
        public async void ReadFiles(string Path)
        {
            if(Directory.Exists(Path))
            {               
                var files = Directory.GetFiles(Path).ToList();                  //Получаю список всех файлов в папке
               
                var dataFiles = files.Where(p => p.Contains(".txt")).ToList();  //выбираю из общего списка файлов только текстовые

                Parallel.ForEach(dataFiles, file=> Writeresult(file));          //Для каждого текстового файла запускаю поток по расчету выражений
                
            }
        }
        object obj = new object();                                              //Для блокировки единовременнго обращения к файлу result.dat
        public void Writeresult(string file)
        {
            lock(obj)
            {
                Console.WriteLine($"Поток: {Thread.CurrentThread.ManagedThreadId} начинает работу с файлом {file}");
                using (StreamWriter SW = new StreamWriter("result.dat", true))  //Открываю поток для записи в файл result.dat
                {
                    using (StreamReader SR = new StreamReader(file))            //Открываю поток для чтения текстового файла с выражениями
                    {
                        while (!SR.EndOfStream)                                 //Цикл для построчного чтения и расчета выражения
                        {
                            var str = SR.ReadLine();
                            string[] strArray = str.Split(' ');
                            if(!Double.TryParse(strArray[1], out var number1))
                                continue;
                            if(!Double.TryParse(strArray[2], out var number2))
                                continue;
                            double result = 0;
                            char operation = ' ';
                            if (strArray[0] == "1")                             //Операция умножения
                            {
                                result = number1 * number2;
                                operation = '*';
                            }
                            else if (strArray[0] == "2" && number2 != 0)        //Операция деления
                            {
                                result = number1 / number2;
                                operation = '/';
                            }
                            else continue;
                            SW.WriteLine($"{number1} {operation} {number2} = {result}");
                        }
                    }
                }
                Console.WriteLine($"Поток: {Thread.CurrentThread.ManagedThreadId} завершает работу с файлом {file}");
            }                       
        }

        /// <summary>
        /// Метод для рандомного создания 10 текстовых файлов
        /// </summary>
        public void CreateFiles()
        {
            for (int i = 1; i <= 10; i++)
            {
                using (StreamWriter SW = new StreamWriter($"{i}.txt"))
                {
                    string[] operations = new string[] {"1", "2"};
                    Random rnd = new Random();
                    for (int j = 0; j < 100; j++)
                    {
                        SW.WriteLine($"{rnd.Next(1, 3)} {rnd.Next(0, 10) * rnd.NextDouble()} {rnd.Next(0, 10) * rnd.NextDouble()}");
                    }
                    SW.Close();
                }
            }
        }
    }
}
