using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ThreadFileReader
{
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
                var files = Directory.GetFiles(Path).ToList();
               
                var dataFiles = files.Where(p => p.Contains(".txt")).ToList();

                using (StreamWriter SW = new StreamWriter("result.dat"))
                {
                    foreach (var file in dataFiles)
                    {
                        using (StreamReader SR = new StreamReader(file))
                        {
                            while(!SR.EndOfStream)
                            {
                                var str = SR.ReadLine();
                                string[] strArray = str.Split(' ');
                                Double.TryParse(strArray[1], out var number1);
                                Double.TryParse(strArray[2], out var number2);
                                double result = 0;
                                char operation = ' ';
                                if (strArray[0] == "1")
                                {
                                    result = number1 * number2;
                                    operation = '*';
                                }

                                else if (strArray[0] == "2" && number2 != 0)
                                {
                                    result = number1 / number2;
                                    operation = '/';
                                }
                                else continue;
                                SW.WriteLine($"{number1} {operation} {number2} = {result}");
                            }
                        }                        
                    }
                    SW.Close();
                }
            }
        }

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
                }
            }
        }
    }
}
