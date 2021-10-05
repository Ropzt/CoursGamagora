using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int seed = 12;
            Random seededRand = new Random(seed);
            int sum = new int();
            int max = 100000;
            for(int i =0;i<max;i++)
            {
                int randNumber = seededRand.Next(0,2);
                sum += randNumber;
            }

            float total = (float)sum / (float)max;
            Console.WriteLine(total);
        }
    }
}
