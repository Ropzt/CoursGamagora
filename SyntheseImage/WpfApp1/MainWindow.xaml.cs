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

        public double getIntersectT(Ray ray)
        {
            
            Vector3D co = ray.Origin - Center; //Get initial position
            double b = Vector3D.DotProduct(ray.Direction, co); 
            double c = Vector3D.DotProduct(co, co) - Radius * Radius;
            double delta = b * b - c; //Get discriminant
            if (delta < 0)
            {
                return 0;
            }
            double t1 = new double();
            double dsquared = Math.Sqrt(delta);
            if (delta == 0)
            {
                t1 = -b / 2;
                if(t1<=0)
                {
                    return 0;
                }
                else
                {
                    return t1;
                }
            }
            else
            {
                 t1 = -b + dsquared;
                double t2 = -b - dsquared;
                if(t1<=0 & t2<=0)
                {
                    return 0;
                }
                else if( t1>0)
                {
                    if(t2<=0)
                    {
                        return t1;
                    }
                    else if(t1==t2)
                    {
                        return t1;
                    }
                    else
                    {
                        double diff = t1 - t2;
                        if(diff>0)
                        {
                            return t2;
                        }
                        else
                        {
                            return t1;
                        }
                    }
                }
                else
                {
                    return t2;
                }
            }
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
            Sphere sphere2 = new Sphere(1.0, new Vector3D(1, 1, -9));
            sphereList.Add(sphere1);
            sphereList.Add(sphere2);

            Vector3D cameraPos = new Vector3D(0,0,5);

            
                for (int h = 0; h < height; h++)
                {
                    for (int w = 0; w < width; w++)
                    {
                        int index = (h * width + w) * 4;
                        Vector3D pixPos = new Vector3D(w / 255.0 - 1.0, h / 255.0 - 1.0, 0);
                        Vector3D dir = pixPos - cameraPos;
                        dir.Normalize();

                        Ray ray = new Ray(cameraPos, dir);
                        int sphereCount = 0;
                        double intersectMin = double.MaxValue;
                        foreach (Sphere sphere in sphereList)
                        {
                            double t = sphere.getIntersectT(ray);
                            if (t !=0 & t < intersectMin)
                            {
                                intersectMin = t;
                                pixels[index + 0] = 255;
                                pixels[index + 1] = (byte)(50 * sphereCount);
                                pixels[index + 2] = (byte)(50 * sphereCount);
                                pixels[index + 3] = 255;
                            }
                            sphereCount++;
                        }
                    }
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
