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
        public int colorR;
        public int colorG;
        public int colorB;
        public string Texture;
    }

    public class Plane : Object
    {
        public Vector3D Center;
        public Vector3D Normal;

        public Plane(Vector3D center, Vector3D normal, int red, int green, int blue, string texture = "Opaque")
        {
            Center = center;
            Normal = normal;

            Type = "Plane";
            colorR = red;
            colorG = green;
            colorB = blue;
            Texture = texture;
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

        public Sphere(double radius, Vector3D center, int red, int green, int blue, string texture = "Opaque")
        {
            Radius = radius;
            Center = center;

            Type = "Sphere";
            colorR = red;
            colorG = green;
            colorB = blue;
            Texture = texture;
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
        List<Object> objectList;

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

        void InitializeBackground()
        {
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
        }

        void AddObjects()
        {
            objectList = new List<Object>();


            Object sphere1 = new Sphere(1.0, new Vector3D(0, -1, -11), 230, 0, 255, "Mirroir");
            Object sphere2 = new Sphere(1.0, new Vector3D(1, 1, -11), 255, 0, 0, "Mirroir");
            Object sphere3 = new Sphere(1.0, new Vector3D(-1, 1, -11), 0, 255, 0, "Mirroir");
            Object rightFace = new Plane(new Vector3D(2.5, 0, 0), new Vector3D(-1, 0, 0), 0, 255, 0, "Mirroir");
            Object leftFace = new Plane(new Vector3D(-2.5, 0, 0), new Vector3D(1, 0, 0), 0, 0, 255, "Mirroir");
            Object topFace = new Plane(new Vector3D(0, -2, 0), new Vector3D(0, 1, 0), 0, 0, 255, "Mirroir");
            Object bottomFace = new Plane(new Vector3D(0, 2, 0), new Vector3D(0, -1, 0), 255, 0, 0, "Mirroir");
            Object backFace = new Plane(new Vector3D(0, 0, -16), new Vector3D(0, 0, 1), 255, 255, 255, "Mirroir");


            objectList.Add(sphere1);
            objectList.Add(sphere2);
            objectList.Add(sphere3);
            objectList.Add(rightFace);
            objectList.Add(leftFace);
            objectList.Add(topFace);
            objectList.Add(bottomFace);
            objectList.Add(backFace);
        }

        void colorPixels(Object obj, int index, double eclairageTotal, byte blueReflected, byte greenReflected, byte redReflected)
        {
            //Print
            if (obj.Texture == "Opaque")
            {
                pixels[index + 0] = (byte)(eclairageTotal * (obj.colorB / 255));
                pixels[index + 1] = (byte)(eclairageTotal * (obj.colorG / 255));
                pixels[index + 2] = (byte)(eclairageTotal * (obj.colorR / 255)); //j'aime le violet
                pixels[index + 3] = 255;
            }
            else
            {

                pixels[index + 0] = (byte)(eclairageTotal * (blueReflected / 255));
                pixels[index + 1] = (byte)(eclairageTotal * (greenReflected / 255));
                pixels[index + 2] = (byte)(eclairageTotal * (redReflected / 255)); //j'aime le violet
                pixels[index + 3] = 255;

            }
        }

        double computeTShadowsMax(Object obj, Ray intersecRay)
        {
            double tShadow = 0;
            double tShadowMax = 0;

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
            return tShadowMax;
        }

        void computeLightAndShadows(int index, Object obj, Sphere lightSphere, Vector3D pointIntersec, Vector3D normalIntersec, Vector3D intersecOffseted, byte blueReflected, byte greenReflected, byte redReflected)
        {
            /*
             Loop multiple ray for spherical lightSource to get diffuse shadows
            */
            double eclairageTotal = new double();
            //Trace ray from intersection point and search for light source or obstacles
            int nb_meridiens = 20;
            int nb_paralleles = 20;
            int nb_rays2Light = nb_meridiens * nb_paralleles;
            double decalage = Vector3D.DotProduct(normalIntersec, new Vector3D(0, 0, 1));
            double decalageInv = Math.Acos(decalage);

            for (int j = 0; j < nb_paralleles; j++)
            {
                double phi = ((Math.PI * j) / nb_paralleles);
                for (int i = 0; i < nb_meridiens; i++)
                {
                    double angle = ((2 * Math.PI * i) / nb_meridiens);


                    double xDir = Math.Cos(angle + decalageInv) * Math.Sin(phi);
                    double yDir = Math.Sin(angle + decalageInv) * Math.Sin(phi);
                    double zDir = Math.Cos(phi);
                    Vector3D rayMarker = new Vector3D(xDir, yDir, zDir);

                    //Calculate direction vector from intersect to light.Center
                    Vector3D directorVector = rayMarker - pointIntersec;
                    directorVector.Normalize();
                    Ray intersecRay = new Ray(intersecOffseted, directorVector);
                    double tLight = intersecRay.getSphereIntersectT(lightSphere.Center, lightSphere.Radius);

                    if (tLight != 0)
                    {
                        double tShadowMax = computeTShadowsMax(obj, intersecRay);

                        if (tShadowMax == 0)
                        {
                            //compute the quantity of light given an intensity
                            double intensity = 750 / nb_rays2Light;
                            double eclairage = Vector3D.DotProduct(normalIntersec, directorVector) * (intensity / tLight);
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
                        colorPixels(obj, index, eclairageTotal, blueReflected, greenReflected, redReflected);
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


        void Render()
        {
            InitializeBackground();

            AddObjects();
            

            Vector3D cameraPos = new Vector3D(0,0,6);

            int seed = 1312;
            Random rand = new Random(seed);

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




                            /*
                             REFRACTION / REFLECTION
                            */

                            double cosineTeta = Vector3D.DotProduct(normalIntersec, dir);
                            //Clamp
                            if (cosineTeta > 1)
                                cosineTeta = 1;
                            if (cosineTeta < -1)
                                cosineTeta = -1;
                            /* Inverse l'image, a comprendre
                            if (cosineTeta < 0)
                                cosineTeta = -cosineTeta;
                            */
                            //Reflection
                            Vector3D b = cosineTeta * normalIntersec;
                            Vector3D reflectionVector = dir - b - b;

                            //Refraction
                            double refractionIndice = 1/1.5; // 1 = air / 1.5 = glass
                            Vector3D refractionVector = new Vector3D();
                            double k = 1 - refractionIndice * refractionIndice * (1 - cosineTeta * cosineTeta);
                            if (k >= 0)
                                refractionVector =  refractionIndice * dir + (refractionIndice * cosineTeta - Math.Sqrt(k)) * normalIntersec;
                            /*TODO : 
                             * Aleatoirement choisir entre refraction et reflection (sauf si un =0)
                             * if Refraction -> Loop l'aléatoire
                             * cumuler les couleurs que l'on trouve
                            */

                            //Check et aléatoire

                            float probaReflection = 0.7f;

                            byte blueReflected = 0;
                            byte greenReflected = 0;
                            byte redReflected = 0;

                            if (obj.Texture == "Mirroir")
                            {
                                if (!Vector3D.Equals(reflectionVector, new Vector3D()))
                                {
                                    double x4Reflection = (cameraPos - pointIntersec).X * 0.000001;
                                    double y4Reflection = (cameraPos - pointIntersec).Y * 0.000001;
                                    double z4Reflection = (cameraPos - pointIntersec).Z * 0.000001;
                                    Vector3D intersec4Reflection = pointIntersec + new Vector3D(x4Reflection, y4Reflection, z4Reflection);

                                    Ray rayReflected = new Ray(intersec4Reflection, reflectionVector);
                                    double intersectMinReflected = double.MaxValue;
                                    foreach (Object objReflected in objectList)
                                    {
                                        Sphere sphereReflected = objReflected as Sphere;
                                        Plane planeReflected = objReflected as Plane;
                                        double tReflected = new double();
                                        switch (objReflected.Type)
                                        {
                                            case "Plane":
                                                tReflected = planeReflected.getIntersectT(rayReflected);
                                                break;
                                            case "Sphere":
                                                tReflected = rayReflected.getSphereIntersectT(sphereReflected.Center, sphereReflected.Radius); ;
                                                break;
                                        }

                                        if (tReflected != 0 & tReflected < intersectMinReflected)
                                        {
                                            intersectMinReflected = tReflected;
                                            blueReflected  = (byte)objReflected.colorB;
                                            greenReflected = (byte)objReflected.colorG;
                                            redReflected   = (byte)objReflected.colorR;
                                        }
                                    }
                                }
                            }

                            
                            /*
                            if (Vector3D.Equals(refractionVector, new Vector3D()) & !Vector3D.Equals(reflectionVector, new Vector3D()) )
                            {
                                //faire intersect avec ce nouveau vecteur directeur pour voir si on tape quelque chose
                            }
                            else if (Vector3D.Equals(reflectionVector, new Vector3D()) & !Vector3D.Equals(refractionVector, new Vector3D()) )
                            {
                                //faire intersect avec ce nouveau vecteur directeur pour voir si on tape quelque chose et retrouvé la couleur de l'objet intersecté
                            }
                            else if( !Vector3D.Equals(reflectionVector, new Vector3D()) & !Vector3D.Equals(refractionVector, new Vector3D()) )
                            {
                                float proba = rand.Next(1, 11) / 10;
                                if(proba > probaReflection)
                                {
                                    //faire intersect avec reflection vector pour voir si on tape quelque choseet retrouvé la couleur de l'objet intersecté
                                }
                                else
                                {
                                    //faire intersect avec refraction vector pour voir si on tape quelque chose et loop until theres no refraction left
                                }
                            }
                            */

                            /*
                                ECLAIRAGE
                            */

                            
                            
                            Sphere lightSphere = new Sphere(2.0, new Vector3D(0, 0, -7), 255, 255, 255);
                            double x = (lightSphere.Center - pointIntersec).X * 0.0001;
                            double y = (lightSphere.Center - pointIntersec).Y * 0.0001;
                            double z = (lightSphere.Center - pointIntersec).Z * 0.0001;
                            Vector3D intersecOffseted = pointIntersec + new Vector3D(x, y, z);

                            computeLightAndShadows(index, obj, lightSphere, pointIntersec, normalIntersec, intersecOffseted, blueReflected, greenReflected, redReflected);
                            
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
