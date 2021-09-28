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

        public double getPointIntersectT(Vector3D center, double Radius = 0)
        {
            Vector3D co = center - Origin; //Vecteur qui part de ray.origin vers center (pour les points)
            double b = Vector3D.DotProduct(Direction, co);
            double c = Vector3D.DotProduct(co, co) -(Radius * Radius);
            double delta = b * b - c; //Get discriminant (a modifier : -(4*c) ???)
            if (delta < 0)
            {
                return 0;
            }
            double t1 = new double();
            double dsquared = Math.Sqrt(delta);
            if (delta == 0)
            {
                t1 = -b / 2;
                if (t1 <= 0)
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
                if (t1 <= 0 & t2 <= 0)
                {
                    return 0;
                }
                else if (t1 > 0)
                {
                    if (t2 <= 0)
                    {
                        return t1;
                    }
                    else if (t1 == t2)
                    {
                        return t1;
                    }
                    else
                    {
                        double diff = t1 - t2;
                        if (diff > 0)
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

        public bool isSphereHit(Vector3D center, double Radius = 0)
        {
            bool hit = true;
            Vector3D co = Origin - center; //Vecteur qui part de center vers ray.origine(pour les sphere)
            double b = Vector3D.DotProduct(Direction, co);
            double c = Vector3D.DotProduct(co, co) - (Radius * Radius);
            double delta = b * b - c; //Get discriminant (a modifier : -(4*c) ???)
            if (delta < 0)
                hit = false;
            return hit;
        }

        public double getSphereIntersectT(Vector3D center, double Radius = 0)
        {
            
            Vector3D co =  Origin - center ; //Vecteur qui part de center vers ray.origine(pour les sphere)
            double b = Vector3D.DotProduct(Direction, co);
            double c = Vector3D.DotProduct(co, co) - (Radius * Radius);
            double delta = b * b - c; //Get discriminant (a modifier : -(4*c) ???)
            if (delta < 0)
            {
                return 0;
            }
            double t1 = new double();
            double dsquared = Math.Sqrt(delta);
            if (delta == 0)
            {
                t1 = -b / 2;
                if (t1 <= 0)
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
                if (t1 <= 0 & t2 <= 0)
                {
                    return 0;
                }
                else if (t1 > 0)
                {
                    if (t2 <= 0)
                    {
                        return t1;
                    }
                    else if (t1 == t2)
                    {
                        return t1;
                    }
                    else
                    {
                        double diff = t1 - t2;
                        if (diff > 0)
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
            double c = Vector3D.DotProduct(co, co) - (Radius * Radius);
            double delta = b * b - c; //Get discriminant (a modifier : -(4*c) ???)
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

    public class LightSource
    {
        public Vector3D Origin;

        public LightSource(Vector3D origin)
        {
            Origin = origin;
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
                    
                    
                    pixels[index + 0] = 255;
                    pixels[index + 1] = 255;
                    pixels[index + 2] = 255;
                    pixels[index + 3] = 255;
                }
            }

            List<Sphere> sphereList = new List<Sphere>();
            Sphere sphere1 = new Sphere(1.0, new Vector3D(0, 0, -10));
            Sphere sphere2 = new Sphere(1.0, new Vector3D(1, 1, -11));
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
                        //double t = sphere.getIntersectT(ray);
                        double t = ray.getSphereIntersectT(sphere.Center, sphere.Radius);
                        if (t !=0 & t < intersectMin)
                        {
                            intersectMin = t;

                            //Calculate intersection point & its norm to sphere center
                            Vector3D pointIntersec = cameraPos+(dir*t);
                            Vector3D normalIntersec = pointIntersec - sphere.Center;
                            normalIntersec.Normalize();

                            //place light source and calculate the direction vector to the intersect point
                            LightSource lightSource = new LightSource(new Vector3D(-3, -1, -4));

                            //calculate the intersect point with offset
                            double x = (lightSource.Origin - pointIntersec).X* 0.0001;
                            double y = (lightSource.Origin - pointIntersec).Y* 0.0001;
                            double z = (lightSource.Origin - pointIntersec).Z* 0.0001;
                            Vector3D intersecOffseted = pointIntersec + new Vector3D(x,y,z);

                            //Calculate direction vector light-intersect
                            Vector3D co = lightSource.Origin - intersecOffseted;
                            Ray intersecRay = new Ray(intersecOffseted, co);
                            double tLight = intersecRay.getPointIntersectT(lightSource.Origin);


                            if (tLight == 0)
                            {
                               
                                double tShadow = 0;
                                double tShadowMax = 0;
                                
                                bool hit = false;
                                
                                foreach (Sphere sphereObstacle in sphereList)
                                {
                                    /*
                                    hit = intersecRay.isSphereHit(sphereObstacle.Center, sphereObstacle.Radius);
                                    if (hit)
                                        break;
                                    */
                                    tShadowMax = 0;
                                    tShadow = intersecRay.getSphereIntersectT(sphereObstacle.Center, sphereObstacle.Radius);
                                    if (tShadow>tShadowMax)
                                    {
                                        tShadowMax = tShadow;
                                    }
                                }
                                
                                /*
                                 A faire : tShadowMax est toujours égal à zéro, cela veut dire qu'il prend en compte son intersection avec sa sphere 
                                ((t négatif ??) et se bloque net avant de tester les autres spheres)
                                Peut etre un probleme avec la direction du ray et de co (inverser co et inverser les t ??)
                                 */
                                if(tShadowMax==0)
                                {
                                    //compute the quantity of light given an intensity
                                    double intensity = 50;
                                    double shading = Vector3D.DotProduct(normalIntersec, co) * intensity;

                                    //Clamp
                                    if (shading < 0)
                                    {
                                        shading = 0;
                                    }
                                    if (shading > 255)
                                    {
                                        shading = 255;
                                    }

                                    //Print
                                    pixels[index + 0] = (byte)shading;
                                    pixels[index + 1] = 0;
                                    pixels[index + 2] = 0;
                                    pixels[index + 3] = 255;
                                }
                                else
                                {
                                    //Print
                                    pixels[index + 0] = 0;
                                    pixels[index + 1] = 0;
                                    pixels[index + 2] = 0; //red for debug
                                    pixels[index + 3] = 255;
                                }
                            }
                            else
                            {
                                //Print
                                pixels[index + 0] = 0;
                                pixels[index + 1] = 0;
                                pixels[index + 2] = 0;
                                pixels[index + 3] = 255;
                            }
                            
                            /*
                            //Trace ray from intersection point and search for light source or obstacles
                            int nb_meridiens = 10;
                            int nb_paralleles = 10;
                            for (int j = 0; j < nb_paralleles; j++)  
                            {
                                double phi = ((Math.PI * j) / nb_paralleles);
                                for (int i = 0; i < nb_meridiens; i++)
                                {

                                    double angle = ((2*Math.PI * i) / nb_meridiens);

                                    double decalage = Vector3D.DotProduct(normalIntersec, new Vector3D(1,1,1));
                                    double decalageInv = Math.Acos(decalage);

                                    double x = Math.Cos(angle+decalageInv) * Math.Sin(phi);
                                    double y = Math.Sin(angle+decalageInv) * Math.Sin(phi);
                                    double z = Math.Cos(phi);

                                    Vector3D rayDir = new Vector3D(x, y, z);
                                    rayDir.Normalize();
                                    Ray rayIntersec = new Ray(pointIntersec, rayDir);

                                    Vector3D co = rayIntersec.Origin - lightSource.Origin; //Get initial position
                                    double b = Vector3D.DotProduct(rayIntersec.Direction, co);
                                    double c = Vector3D.DotProduct(co, co)-1*1;
                                    double delta = b * b - c; //Get discriminant (a modifier : -(4*c) ???)
                                    if (delta > 0)
                                    {
                                        //compute the quantity of light given an intensity
                                        double intensity = 200;
                                        double shading = -(Vector3D.DotProduct(normalIntersec, rayIntersec.Direction) * intensity);

                                        //Clamp
                                        if (shading < 0)
                                        {
                                            shading = 0;
                                        }
                                        if (shading > 255)
                                        {
                                            shading = 255;
                                        }

                                        //Print
                                        pixels[index + 0] = (byte)shading;
                                        pixels[index + 1] = 0;
                                        pixels[index + 2] = 0;
                                        pixels[index + 3] = 255;
                                    }

                                }
                            }
                            */

                            
                            
                            
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
