using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static Objet sphere3 = new Objet();

        static void Main(string[] args)
        {
            List<Objet> objectList = new List<Objet>();
            Objet sphere1 = new Sphere(4,0,"coucou");
            Objet sphere2 = new Sphere(5, 1, "hola");
            objectList.Add(sphere1);
            objectList.Add(sphere2);
            sphere3 = sphere1;
            sphere1.Id += 1;



            sansRien();
            passageParRef(ref objectList);
            /*
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
            */
        }

        public class Objet
        {
            public string Type;
            public int Id;
        }

        public class Sphere : Objet
        {
            public int Size;

            public Sphere(int size, int id, string type)
            {
                Size = size;
                Type = type;
                Id = id;
            }

            public int getSize()
            {
                return Size;
            }
        }

        public static void sansRien()
        {
            Sphere sph = sphere3 as Sphere;
            
            Console.WriteLine("Sphere1 Size :" + sphere3.Id);
        }

        public static void passageParRef(ref List<Objet> liste)
        {
            Console.WriteLine("Sphere from list 1 id :" + liste[0].Id);
        }

        
    }
}
