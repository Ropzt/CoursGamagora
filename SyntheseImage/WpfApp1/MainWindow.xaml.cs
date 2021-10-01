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

    public class Object
    {
        public string Type;


    }

    public class Plane : Object
    {
        public Vector3D Center;
        public Vector3D Normal;

        public Plane(Vector3D center, Vector3D normal)
        {
            Center = center;
            Normal = normal;
            Type = "Plane";
        }

        public double getIntersectT(Ray ray)
        {
            double determinant = Vector3D.DotProduct(ray.Direction,Normal);
            if(determinant < 1e-7)
            {
                double t = (Vector3D.DotProduct((Center - ray.Origin), Normal) / determinant);
                return t;
            }
            else
            {
                return 0;
            }
        }
    }

    public class Sphere : Object
    {
        public double Radius;
        public Vector3D Center;

        public Sphere(double radius, Vector3D center)
        {
            Radius = radius;
            Center = center;
            Type = "Sphere";
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
        int width = 510;
        int height = 510;

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

            
            Object sphere1 = new Sphere(1.0, new Vector3D(0, -1, -11));
            Object sphere2 = new Sphere(1.0, new Vector3D(1, 1, -11));
            Object sphere3 = new Sphere(1.0, new Vector3D(-1, 1, -11));
            /*
            List<Sphere> sphereList = new List<Sphere>();
            sphereList.Add(sphere1);
            sphereList.Add(sphere2);
            sphereList.Add(sphere3);
            */
            Object rightFace = new Plane(new Vector3D(2.5, 0, 0),new Vector3D(-1,0,0));
            Object leftFace = new Plane(new Vector3D(-2.5, 0, 0), new Vector3D(1, 0, 0));
            Object topFace = new Plane(new Vector3D(0, -2, 0), new Vector3D(0, 1, 0));
            Object bottomFace = new Plane(new Vector3D(0, 2, 0), new Vector3D(0, -1, 0));
            Object backFace = new Plane(new Vector3D(0, 0, -16), new Vector3D(0, 0, 1));

            List<Object> objectList = new List<Object>();
            objectList.Add(sphere1);
            objectList.Add(sphere2);
            objectList.Add(sphere3);
            objectList.Add(rightFace);
            objectList.Add(leftFace);
            objectList.Add(topFace);
            objectList.Add(bottomFace);
            objectList.Add(backFace);

            

            Vector3D cameraPos = new Vector3D(0,0,5.5);

            
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    int index = (h * width + w) * 4;
                    Vector3D pixPos = new Vector3D(w / 255.0 - 0.9999, h / 255.0 - 0.9999, 0.0000001);
                    
                    Vector3D dir = pixPos - cameraPos;
                    dir.Normalize();

                    Ray ray = new Ray(cameraPos, dir);
                    int sphereCount = 0;
                    double intersectMin = double.MaxValue;

                    //foreach (Sphere sphere in sphereList)
                    foreach(Object obj in objectList)
                    {
                        Sphere sphereObj = obj as Sphere;
                        Plane planeObj = obj as Plane;
                        double t = new double();
                        switch (obj.Type)
                        {
                            case "Plane":
                                t = planeObj.getIntersectT(ray);
                                break;
                            case "Sphere":
                                t = ray.getSphereIntersectT(sphereObj.Center, sphereObj.Radius); ;
                                break;
                        }
                            
                                
                        
                        if (t !=0 & t < intersectMin)
                        {
                            intersectMin = t;

                            //Calculate intersection point & its norm to sphere center
                            Vector3D pointIntersec = cameraPos+(dir*t);

                            Vector3D normalIntersec = new Vector3D();
                            switch (obj.Type)
                            {
                                case "Plane":
                                    normalIntersec =  planeObj.Normal;
                                    break;
                                case "Sphere":
                                    sphereObj = obj as Sphere;
                                    normalIntersec = pointIntersec - sphereObj.Center;
                                    break;
                            }
                            
                            normalIntersec.Normalize();

                            double eclairageTotal = new double();
                            
                            Sphere lightSphere = new Sphere(3.0, new Vector3D(0, 0, -6));
                            double x = (lightSphere.Center - pointIntersec).X * 0.0001;
                            double y = (lightSphere.Center - pointIntersec).Y * 0.0001;
                            double z = (lightSphere.Center - pointIntersec).Z * 0.0001;
                            Vector3D intersecOffseted = pointIntersec + new Vector3D(x, y, z);


                            /*
                             Loop multiple ray for spherical lightSource to get diffuse shadows
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

                                        foreach (Object objectObstacle in objectList)
                                        {
                                            Sphere sphereObstacle = obj as Sphere;
                                            Plane planeObstacle = obj as Plane;
                                            switch (obj.Type)
                                            {
                                                case "Plane":
                                                    tShadow = planeObstacle.getIntersectT(intersecRay);
                                                    break;
                                                case "Sphere":
                                                    tShadow = intersecRay.getSphereIntersectT(sphereObstacle.Center, sphereObstacle.Radius);
                                                    break;
                                            }

                                            if (tShadow > tShadowMax)
                                            {
                                                tShadowMax = tShadow;
                                            }
                                        }


                                        if (tShadowMax == 0)
                                        {
                                            //compute the quantity of light given an intensity
                                            double intensity = 750 / nb_rays2Light;
                                            double eclairage = Vector3D.DotProduct(normalIntersec, co) * (intensity / tLight);
                                            eclairageTotal += eclairage;

                                            //Clamp
                                            if (eclairageTotal < 0)
                                            {
                                                eclairageTotal = 0;
                                            }
                                            if (eclairageTotal > 255)
                                            {
                                                eclairageTotal = 255;
                                            }

                                        }
                                        //Print
                                        pixels[index + 0] = (byte)eclairageTotal;
                                        pixels[index + 1] = 0;
                                        pixels[index + 2] = (byte)(eclairageTotal / 1.5); //j'aime le violet
                                        pixels[index + 3] = 255;
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
