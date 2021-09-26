using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public class Ray
    {
        public Vector3D Origin;
        public Vector3D Direction;

        public Ray(Vector3D origin, Vector3D direction)
        {
            Origin = origin;
            Direction = direction;
        }
    }

    public class Sphere
    {
        public double Radius;
        public Vector3D Center;

        public Sphere(double radius, Vector3D center)
        {
            Radius = radius;
            Center = center;
        }

        public bool Hit(Ray ray)
        {
            Vector3D co = ray.Origin - Center;
            double b = Vector3D.DotProduct(ray.Direction, co);
            double c = Vector3D.DotProduct(co, co) - Radius * Radius;
            double disc = b * b - c;
            return disc > 0;
        }
    }
    public partial class MainWindow : Window
    {
        WriteableBitmap bitmap;
        byte[] pixels;
        int width = 500;
        int height = 500;

        public MainWindow()
        {
            InitializeComponent();
            CreateImage();
            Render();
            SetImage();

        }

        void CreateImage()
        {
            bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Pbgra32, null);
            pixels = new byte[(int)(width * height * 4)];
        }

        void Render()
        {
            //Initialize Background
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    int index = (h * width + w) * 4;
                    
                    
                        pixels[index + 0] = 0;
                        pixels[index + 1] = 0;
                        pixels[index + 2] = 0;
                        pixels[index + 3] = 255;
                }
            }

            List<Sphere> sphereList = new List<Sphere>();
            Sphere sphere1 = new Sphere(1.0, new Vector3D(0, 0, -10));
            Sphere sphere2 = new Sphere(3.8, new Vector3D(2, 2, -20));
            sphereList.Add(sphere1);
            sphereList.Add(sphere2);

            Vector3D cameraPos = new Vector3D(0,0,5);

            int sphereCount=0;
            foreach(Sphere sphere in sphereList)
            {
                for (int h = 0; h < height; h++)
                {
                    for (int w = 0; w < width; w++)
                    {
                        int index = (h * width + w) * 4;
                        Vector3D pixPos = new Vector3D(w / 255.0 - 1.0, h / 255.0 - 1.0, 0);
                        Vector3D dir = pixPos - cameraPos;
                        dir.Normalize();

                        Ray ray = new Ray(cameraPos, dir);

                        if (sphere.Hit(ray))
                        {
                            pixels[index + 0] = 255;
                            pixels[index + 1] = (byte)(50*sphereCount);
                            pixels[index + 2] = (byte)(50*sphereCount);
                            pixels[index + 3] = 255;
                        }
                        
                    }
                }
                sphereCount++;
            }
            
        }

        void SetImage()
        {
            int stride = width * 4;
            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, stride,0,0);
            image.Source = bitmap;
        }
    }
}
