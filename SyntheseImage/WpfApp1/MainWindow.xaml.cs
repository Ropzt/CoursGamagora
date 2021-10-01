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
            
            Vector3D co = Origin - center; //Vecteur qui part de center vers ray.origine(pour les sphere)
            double b = Vector3D.DotProduct(Direction,co);
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
            Sphere sphere1 = new Sphere(0.5, new Vector3D(0, 0.5, -9));
            Sphere sphere2 = new Sphere(1.0, new Vector3D(1, 1, -11));
            Sphere sphere3 = new Sphere(1.0, new Vector3D(-1, 1, -11));
            sphereList.Add(sphere1);
            sphereList.Add(sphere2);
            sphereList.Add(sphere3);

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

                            double shadingTotal = new double();
                            
                            Sphere lightSphere = new Sphere(1.0, new Vector3D(0, 0, -5));
                            double x = (lightSphere.Center - pointIntersec).X * 0.0001;
                            double y = (lightSphere.Center - pointIntersec).Y * 0.0001;
                            double z = (lightSphere.Center - pointIntersec).Z * 0.0001;
                            Vector3D intersecOffseted = pointIntersec + new Vector3D(x, y, z);


                            //Loop multiple ray for spherical lightSource to get diffuse shadows

                            /*
                            //Calculate direction vector from intersect to light.Center
                            Vector3D co = lightSphere.Center - pointIntersec;
                            co.Normalize();
                            Ray intersecRay = new Ray(intersecOffseted, co);
                            double tLight = intersecRay.getSphereIntersectT(lightSphere.Center, lightSphere.Radius);


                            if (tLight != 0)
                            {
                               
                                double tShadow = 0;
                                double tShadowMax = 0;
                                
                                bool hit = false;

                                foreach (Sphere sphereObstacle in sphereList)
                                {

                                    //tShadowMax = 0;
                                    tShadow = intersecRay.getSphereIntersectT(sphereObstacle.Center, sphereObstacle.Radius);
                                    if (tShadow > tShadowMax)
                                    {
                                        tShadowMax = tShadow;
                                    }
                                }

                                
                                if (tShadowMax==0)
                                {
                                    //compute the quantity of light given an intensity
                                    double intensity = 1000;
                                    double shading = Vector3D.DotProduct(normalIntersec, co)  * (intensity/tLight);
                                    shadingTotal += shading;

                                    //Clamp
                                    if (shadingTotal < 0)
                                    {
                                        shadingTotal = 0;
                                    }
                                    if (shadingTotal > 255)
                                    {
                                        shadingTotal = 255;
                                    }

                                    //Print
                                    pixels[index + 0] = (byte)shadingTotal;
                                    pixels[index + 1] = 0;
                                    pixels[index + 2] = (byte)(shadingTotal/1.5); //j'aime le violet
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
                            */
                            
                            //Trace ray from intersection point and search for light source or obstacles
                            int nb_meridiens = 20;
                            int nb_paralleles = 20;
                            int nb_rays2Light = nb_meridiens * nb_paralleles;
                            double decalage = Vector3D.DotProduct(normalIntersec, new Vector3D(0,0,1));
                            double decalageInv = Math.Acos(decalage);

                            for (int j = 0; j < nb_paralleles; j++)  
                            {
                                double phi = ((Math.PI * j) / nb_paralleles);
                                for (int i = 0; i < nb_meridiens; i++)
                                {
                                    double angle = ((2*Math.PI * i) / nb_meridiens);

                                    double xDir = lightSphere.Radius * Math.Cos(angle+decalageInv) * Math.Sin(phi);
                                    double yDir = lightSphere.Radius * Math.Sin(angle+decalageInv) * Math.Sin(phi);
                                    double zDir = lightSphere.Radius * Math.Cos(phi);

                                    Vector3D pointOnLightSphere = new Vector3D(xDir, yDir, zDir);

                                    //Calculate direction vector from intersect to light.Center
                                    Vector3D co = pointOnLightSphere - pointIntersec;
                                    co.Normalize();
                                    Ray intersecRay = new Ray(intersecOffseted, co);
                                    double tLight = intersecRay.getSphereIntersectT(lightSphere.Center, lightSphere.Radius);

                                    if (tLight != 0)
                                    {
                               
                                        double tShadow = 0;
                                        double tShadowMax = 0;
                                
                                        bool hit = false;

                                        foreach (Sphere sphereObstacle in sphereList)
                                        {

                                            //tShadowMax = 0;
                                            tShadow = intersecRay.getSphereIntersectT(sphereObstacle.Center, sphereObstacle.Radius);
                                            if (tShadow > tShadowMax)
                                            {
                                                tShadowMax = tShadow;
                                            }
                                        }

                                
                                        if (tShadowMax==0)
                                        {
                                            //compute the quantity of light given an intensity
                                            double intensity = 1000/nb_rays2Light;
                                            double shading = Vector3D.DotProduct(normalIntersec, co)  * (intensity/tLight);
                                            shadingTotal += shading;

                                            //Clamp
                                            if (shadingTotal < 0)
                                            {
                                                shadingTotal = 0;
                                            }
                                            if (shadingTotal > 255)
                                            {
                                                shadingTotal = 255;
                                            }

                                            //Print
                                            pixels[index + 0] = (byte)shadingTotal;
                                            pixels[index + 1] = 0;
                                            pixels[index + 2] = (byte)(shadingTotal/1.5); //j'aime le violet
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

                                }
                            }
                            




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
