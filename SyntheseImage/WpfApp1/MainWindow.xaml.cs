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
using System.Diagnostics;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    

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
            /*
            Object sphere1 = new Sphere(1.0, new Vector3D(0, -1, -11), 255, 255, 255, "Mirroir");
            Object sphere2 = new Sphere(1.0, new Vector3D(1, 1, -10), 255, 255, 255, "Mirroir");
            Object sphere3 = new Sphere(1.0, new Vector3D(-1, 1, -11), 255, 255, 255, "Mirroir");
            */
            Object triangle1 = new Triangle(new Vector3D(-1, 1, -16), new Vector3D(1, 1, -16), new Vector3D(0, -1, -16), 255, 0, 0);

            Object rightFace = new Plane(new Vector3D(2.5, 0, 0), new Vector3D(-1, 0, 0), 0, 255, 0);
            Object leftFace = new Plane(new Vector3D(-2.5, 0, 0), new Vector3D(1, 0, 0), 0, 0, 255);
            Object topFace = new Plane(new Vector3D(0, -2, 0), new Vector3D(0, 1, 0), 0, 0, 255);
            Object bottomFace = new Plane(new Vector3D(0, 2, 0), new Vector3D(0, -1, 0), 255, 0, 0);
            Object backFace = new Plane(new Vector3D(0, 0, -16), new Vector3D(0, 0, 1), 255, 255, 255);

            /*
            objectList.Add(sphere1);
            objectList.Add(sphere2);
            objectList.Add(sphere3);
            */
            objectList.Add(triangle1);
            objectList.Add(rightFace);
            objectList.Add(leftFace);
            objectList.Add(topFace);
            objectList.Add(bottomFace);
            objectList.Add(backFace);
        }



        void colorPixels(ref Object obj, ref int index, ref double eclairageTotal, ref byte blueReflected, ref byte greenReflected, ref byte redReflected)
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
                /*
                Trace.WriteLine("refract");
                Trace.WriteLine("pix1 = " + pixels[index + 0]);
                Trace.WriteLine("pix2 = " + pixels[index + 1]);
                Trace.WriteLine("pix3 = " + pixels[index + 2]);
                Trace.WriteLine("eclairage = " + eclairageTotal);*/
            }
        }

        void colorPixelsFinal(ref Object obj, ref int index, ref double eclairageTotal, ref byte blueReflected, ref byte greenReflected, ref byte redReflected)
        {
            //Trace.WriteLine("refract Final");
            //Print
            if (obj.Texture == "Opaque")
            {
                
                //if (obj.Type == "Triangle") ;
                    //Trace.WriteLine("bleu * ECLAIRAGE = " + (eclairageTotal * (obj.colorG / 255)));
                pixels[index + 0] = (byte)(eclairageTotal * (obj.colorB / 255));
                pixels[index + 1] = (byte)(eclairageTotal * (obj.colorG / 255));
                pixels[index + 2] = (byte)(eclairageTotal * (obj.colorR / 255)); //j'aime le violet
                pixels[index + 3] = 255;
                
            }
            else
            {
                pixels[index + 0] = ((byte)(eclairageTotal * (blueReflected / 255)) > pixels[index + 0]) ? pixels[index + 0] : (byte)(eclairageTotal * (blueReflected / 255));
                pixels[index + 1] = ((byte)(eclairageTotal * (greenReflected / 255)) > pixels[index + 1]) ? pixels[index + 1] : (byte)(eclairageTotal * (greenReflected / 255));
                pixels[index + 2] = ((byte)(eclairageTotal * (redReflected / 255)) > pixels[index + 2]) ? pixels[index + 2] : (byte)(eclairageTotal * (redReflected / 255));

                pixels[index + 3] = 255;

                /*
                Trace.WriteLine("refract Final");
                Trace.WriteLine("pix1 = " + pixels[index + 0]);
                Trace.WriteLine("pix2 = " + pixels[index + 1]);
                Trace.WriteLine("pix3 = " + pixels[index + 2]);
                Trace.WriteLine("eclairage = " + eclairageTotal);
                */
            }
        }

        void ReflectionLoop(Vector3D normalIntersec, Vector3D dir, Vector3D cameraPos, Vector3D pointIntersec, ref double eclairageTotal, ref int index, ref Sphere lightSphere,ref Vector3D intersecOffseted, ref byte blueReflected, ref byte greenReflected, ref byte redReflected, ref int recursionLVL, ref int recursionMax)
        {
            double cosineTeta = Vector3D.DotProduct(normalIntersec, dir);
            //Clamp
            if (cosineTeta > 1)
                cosineTeta = 1;
            if (cosineTeta < -1)
                cosineTeta = -1;
            Vector3D b = cosineTeta * normalIntersec;
            Vector3D reflectionVector = dir - b - b;

            if (!Vector3D.Equals(reflectionVector, new Vector3D()))
            {
                //Offset intersection point to prevent acne
                double x4Reflection = (cameraPos - pointIntersec).X * 0.000001;
                double y4Reflection = (cameraPos - pointIntersec).Y * 0.000001;
                double z4Reflection = (cameraPos - pointIntersec).Z * 0.000001;
                Vector3D intersec4Reflection = pointIntersec + new Vector3D(x4Reflection, y4Reflection, z4Reflection);

                //Ray casted from intersection point
                Ray rayReflected = new Ray(intersec4Reflection, reflectionVector);

                //Initialize minimal t
                double intersectMinReflected = double.MaxValue;

                //Check all objects for intersection by the reflection ray
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

                    //if there's intersection, is the target a mirror or not ? Yes  -> recursion, No -> get colors
                    if (tReflected != 0 & tReflected < intersectMinReflected)
                    {
                        intersectMinReflected = tReflected;

                        //Calculate intersection point & its norm to sphere center

                        Vector3D newIntersec = pointIntersec + (reflectionVector * tReflected);

                        Vector3D newNormalIntersec = new Vector3D();
                        switch (objReflected.Type)
                        {
                            case "Plane":
                                newNormalIntersec = planeReflected.Normal;
                                break;
                            case "Sphere":
                                newNormalIntersec = newIntersec - sphereReflected.Center;
                                break;
                        }

                        newNormalIntersec.Normalize();

                        double x = (lightSphere.Center - newIntersec).X * 0.0001;
                        double y = (lightSphere.Center - newIntersec).Y * 0.0001;
                        double z = (lightSphere.Center - newIntersec).Z * 0.0001;
                        Vector3D newIntersecOffseted = newIntersec + new Vector3D(x, y, z);

                        if ((objReflected.Texture == "Mirroir") & (recursionLVL < recursionMax))
                        {
                            
                            

                            recursionLVL++;
                            computeLightAndShadows(ref eclairageTotal, ref index, objReflected, ref lightSphere, newIntersec, newNormalIntersec, newIntersecOffseted, ref blueReflected, ref greenReflected, ref redReflected);

                            ReflectionLoop(newNormalIntersec, reflectionVector, cameraPos, newIntersec, ref eclairageTotal, ref index, ref lightSphere, ref intersecOffseted, ref blueReflected, ref greenReflected, ref redReflected, ref recursionLVL, ref recursionMax);
                        }
                        else
                        {
                            blueReflected = (byte)objReflected.colorB;
                            greenReflected = (byte)objReflected.colorG;
                            redReflected = (byte)objReflected.colorR;
                            computeLightAndShadows(ref eclairageTotal, ref index, objReflected, ref lightSphere, newIntersec, newNormalIntersec, newIntersecOffseted, ref blueReflected, ref greenReflected, ref redReflected);

                        }
                    }
                }
            }
        }

        void RefractionIn(Object obj, Vector3D normalIntersec, Vector3D dir, Vector3D pointIntersec, Vector3D precedentPoint, ref Sphere lightSphere, ref double eclairageTotal, ref int index, ref byte blueReflected, ref byte greenReflected, ref byte redReflected)
        {
            //possible causes of error:
            // intersectOffseted is offseted in the wrong direction
            //
            double cosineTeta = Vector3D.DotProduct(normalIntersec, dir);
            double refractionIndice = new double();
            //Clamp
            if (cosineTeta > 1)
                cosineTeta = 1;
            if (cosineTeta < -1)
                cosineTeta = -1;
            if (cosineTeta < 0)
            {
                cosineTeta = -cosineTeta;
                refractionIndice = 1 / 1.5; // 1 = air / 1.5 = glass
            }
            else
            {
                refractionIndice =  1.5 / 1; // 1 = air / 1.5 = glass
                normalIntersec = -normalIntersec;
            }
                

            
            Vector3D refractionVector = new Vector3D();
            double k = 1 - refractionIndice * refractionIndice * (1 - cosineTeta * cosineTeta);
            if (k >= 0)
                refractionVector = refractionIndice * dir + (refractionIndice * cosineTeta - Math.Sqrt(k)) * normalIntersec;

            //Trace.WriteLine("refractionVectorIn = "+refractionVector);

            if (!Vector3D.Equals(refractionVector, new Vector3D()))
            {
                double x = (pointIntersec - precedentPoint).X * 0.0001;
                double y = (pointIntersec - precedentPoint).Y * 0.0001;
                double z = (pointIntersec - precedentPoint).Z * 0.0001;
                Vector3D intersecOffseted4Refraction = pointIntersec + new Vector3D(x, y, z);

                Ray insideMatterRay = new Ray(intersecOffseted4Refraction, refractionVector); //maybe need to use intersecOffseted
                double tRefraction = new double();
                Plane planeObj = obj as Plane;
                Sphere sphereObj = obj as Sphere;
                Vector3D objCenter = new Vector3D();
                switch (obj.Type)
                {
                    case "Plane":
                        objCenter = planeObj.Center;
                        tRefraction = planeObj.getIntersectT(insideMatterRay);
                        break;
                    case "Sphere":
                        objCenter = sphereObj.Center;
                        tRefraction = insideMatterRay.getSphereIntersectT(sphereObj.Center, sphereObj.Radius); 
                        break;
                }

                if (tRefraction != 0)
                {
                    
                    Vector3D newIntersecRefraction = pointIntersec + (refractionVector * tRefraction); //maybe intersecOffseted instead of pointIntersec

                    double xNew = (newIntersecRefraction - intersecOffseted4Refraction).X * 0.0001;
                    double yNew = (newIntersecRefraction - intersecOffseted4Refraction).Y * 0.0001;
                    double zNew = (newIntersecRefraction - intersecOffseted4Refraction).Z * 0.0001;
                    Vector3D newIntersecRefractionOffseted = newIntersecRefraction + new Vector3D(xNew, yNew, zNew);

                    Vector3D newNormalIntersecRefraction = new Vector3D();
                    switch (obj.Type)
                    {
                        case "Plane":
                            newNormalIntersecRefraction = planeObj.Normal;
                            break;
                        case "Sphere":
                            newNormalIntersecRefraction =  newIntersecRefraction - objCenter; //maybe invert
                            break;
                    }
                    newNormalIntersecRefraction.Normalize();

                    RefractionOut(ref obj, newNormalIntersecRefraction, refractionVector, newIntersecRefractionOffseted, intersecOffseted4Refraction, ref lightSphere, ref eclairageTotal, ref index, ref blueReflected, ref greenReflected, ref redReflected);
                }
                /*
                else
                {
                    blueReflected = 250;
                    greenReflected = 250;
                    redReflected = 250;
                    eclairageTotal = 253;
                    colorPixels(ref obj, ref index, ref eclairageTotal, ref blueReflected, ref greenReflected, ref redReflected);
                }
                */

            }
            

        }


        void RefractionOut(ref Object obj, Vector3D normalIntersec, Vector3D dir, Vector3D pointIntersecOffseted, Vector3D precedentPos,ref Sphere lightSphere, ref double eclairageTotal, ref int index, ref byte blueReflected, ref byte greenReflected, ref byte redReflected)
        {
            
            double cosineTeta = Vector3D.DotProduct(normalIntersec, dir);
            double refractionIndice = new double();
            //Clamp
            if (cosineTeta > 1)
                cosineTeta = 1;
            if (cosineTeta < -1)
                cosineTeta = -1;
            if (cosineTeta < 0)
            {
                cosineTeta = -cosineTeta;
                refractionIndice = 1 / 1.5; // 1 = air / 1.5 = glass
            }
            else
            {
                refractionIndice = 1.5 / 1; // 1 = air / 1.5 = glass
                normalIntersec = -normalIntersec;
            }

            
            Vector3D refractionVector = new Vector3D();
            double k = 1 - refractionIndice * refractionIndice * (1 - cosineTeta * cosineTeta);
            if (k >= 0)
                refractionVector = refractionIndice * dir + (refractionIndice * cosineTeta - Math.Sqrt(k)) * normalIntersec;

            //Trace.WriteLine("refractionVectorOut = " + refractionVector);

            if (!Vector3D.Equals(refractionVector, new Vector3D()))
            {
                
                Ray outsideMatterRay = new Ray(pointIntersecOffseted, refractionVector); //maybe need to use intersecOffseted
                double tRefraction = new double();
                double tRefractionMin = double.MaxValue;

                foreach (Object objOutMatter in objectList)
                {
                    Plane planeOutMatter = objOutMatter as Plane;
                    Sphere sphereOutMatter = objOutMatter as Sphere;
                    Vector3D objCenter = new Vector3D();
                    switch (objOutMatter.Type)
                    {
                        case "Plane":
                            objCenter = planeOutMatter.Center;
                            tRefraction = planeOutMatter.getIntersectT(outsideMatterRay);
                            break;
                        case "Sphere":
                            objCenter = sphereOutMatter.Center;
                            tRefraction = outsideMatterRay.getSphereIntersectT(sphereOutMatter.Center, sphereOutMatter.Radius); 
                            break;
                    }

                    if (tRefraction != 0 & tRefraction < tRefractionMin)
                    {
                        
                        tRefractionMin = tRefraction;
                        Vector3D newIntersecRefraction = pointIntersecOffseted + (refractionVector * tRefraction);

                        double x = (pointIntersecOffseted - newIntersecRefraction).X * 0.0001;
                        double y = (pointIntersecOffseted - newIntersecRefraction).Y * 0.0001;
                        double z = (pointIntersecOffseted - newIntersecRefraction).Z * 0.0001;
                        Vector3D newIntersecRefractionOffseted = newIntersecRefraction + new Vector3D(x, y, z);

                        Vector3D newNormalIntersecRefraction = new Vector3D();
                        switch (objOutMatter.Type)
                        {
                            case "Plane":
                                newNormalIntersecRefraction = planeOutMatter.Normal;
                                break;
                            case "Sphere":
                                newNormalIntersecRefraction =  newIntersecRefraction - objCenter;
                                break;
                        }

                        
                        newNormalIntersecRefraction.Normalize();

                        
                        blueReflected = (byte)objOutMatter.colorB;
                        greenReflected = (byte)objOutMatter.colorG;
                        redReflected = (byte)objOutMatter.colorR;
                        /*
                        Trace.WriteLine("blue = " + blueReflected);
                        Trace.WriteLine("green = " + greenReflected);
                        Trace.WriteLine("red = " + redReflected);
                        Trace.WriteLine("eclairage = " + eclairageTotal);
                        */
                        computeLightAndShadows(ref eclairageTotal, ref index, objOutMatter, ref lightSphere, newIntersecRefraction, newNormalIntersecRefraction, newIntersecRefractionOffseted, ref blueReflected, ref greenReflected, ref redReflected);
                        

                        /*
                        if (objOutMatter.Texture == "Opaque")
                        {
                            Trace.WriteLine("COUCOUUUUUUUUUUUU2");
                            blueReflected = (byte)objOutMatter.colorB;
                            greenReflected = (byte)objOutMatter.colorG;
                            redReflected = (byte)objOutMatter.colorR;
                            computeLightAndShadows(ref eclairageTotal, ref index, objOutMatter, ref lightSphere, newIntersecRefraction, newNormalIntersecRefraction, newIntersecRefractionOffseted, ref blueReflected, ref greenReflected, ref redReflected);

                        }
                        else
                        {
                           RefractionIn(objOutMatter, newNormalIntersecRefraction, refractionVector, newIntersecRefraction, newIntersecRefractionOffseted, ref lightSphere, ref eclairageTotal, ref index, ref blueReflected, ref greenReflected, ref redReflected); //offseted ??
                        }
                        */


                    }

                }
            }
            

        }
        

        double computeTShadowsMax(Ray intersecRay, double tLight)
        {
            double tShadow = 0;
            double tShadowMin = tLight;

            foreach (Object objectObstacle in objectList)
            {
                Sphere sphereObstacle = objectObstacle as Sphere;
                Plane planeObstacle = objectObstacle as Plane;
                Triangle triangleObstacle = objectObstacle as Triangle;
                switch (objectObstacle.Type)
                {
                    case "Plane":
                        tShadow = planeObstacle.getIntersectT(intersecRay);
                        
                        break;
                    case "Sphere":
                        tShadow = intersecRay.getSphereIntersectT(sphereObstacle.Center, sphereObstacle.Radius);
                        break;
                    case "Triangle":
                        tShadow = intersecRay.getIntersectTriangleT(triangleObstacle);
                        break;
                }

                if (tShadow !=0 & (tShadow < tShadowMin))
                {

                    tShadowMin = tShadow;
                }
            }
            return tShadowMin == tLight ? 0 : 1;
        }
        void computeLightAndShadows(ref double eclairageTotal, ref int index, Object obj, ref Sphere lightSphere, Vector3D pointIntersec, Vector3D normalIntersec, Vector3D intersecOffseted, ref byte blueReflected, ref byte greenReflected, ref byte redReflected)
        {
            /*
             Loop multiple ray for spherical lightSource to get diffuse shadows
            */
            
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
                        double tShadowMax = computeTShadowsMax( intersecRay, tLight);
                        //Trace.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" );
                        if (tShadowMax == 0)
                        {
                            //compute the quantity of light given an intensity
                            double intensity = 4000 / nb_rays2Light;
                            //double eclairage = Vector3D.DotProduct(normalIntersec, directorVector) * (intensity / tLight);
                            Vector3D intersecOnLight = pointIntersec + (directorVector*tLight);
                            double distance = Math.Pow((intersecOnLight.X - pointIntersec.X ), 2f) + Math.Pow((intersecOnLight.Y - pointIntersec.Y), 2f) + Math.Pow((intersecOnLight.Z - pointIntersec.Z), 2f);
                            double eclairage = Vector3D.DotProduct(normalIntersec, directorVector) * (intensity / distance);
                            eclairageTotal += eclairage;
                            //Trace.WriteLine("eclairage added = " + eclairage);
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
                        colorPixels(ref obj, ref index, ref eclairageTotal, ref blueReflected, ref greenReflected, ref redReflected);
                    }
                    else
                    {
                        //Trace.WriteLine("tLight down");
                        //Print
                        pixels[index + 0] = 0;
                        pixels[index + 1] = 0;
                        pixels[index + 2] = 0;
                        pixels[index + 3] = 255;
                    }


                }
            }
        }

        void computeLightAndShadowsFinal(ref double eclairageTotal, ref int index, Object obj, ref Sphere lightSphere, Vector3D pointIntersec, Vector3D normalIntersec, Vector3D intersecOffseted, ref byte blueReflected, ref byte greenReflected, ref byte redReflected)
        {
            /*
             Loop multiple ray for spherical lightSource to get diffuse shadows
            */

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
                    /*
                    if(obj.Type == "Triangle")
                    {
                        Trace.WriteLine("tri raymarker =" + rayMarker);
                        Trace.WriteLine("tri director vector =" + directorVector);
                        Trace.WriteLine("tri intersec off =" + intersecOffseted);
                        Trace.WriteLine("tri tLight =" + tLight);
                    }
                    if(obj.Type == "Plane" & normalIntersec==new Vector3D(0,0,1))
                    {
                        Trace.WriteLine("plane raymarker =" + rayMarker);
                        Trace.WriteLine("plane director vector =" + directorVector);
                        Trace.WriteLine("plane intersec off =" + intersecOffseted);
                        Trace.WriteLine("plane tLight =" + tLight);
                    }*/
                    
                    if (tLight != 0)
                    {
                        
                        double tShadowMax = computeTShadowsMax( intersecRay, tLight);
                        if (tShadowMax == 0)
                        {
                            
                            //compute the quantity of light given an intensity
                            double intensity = 10000 / nb_rays2Light;
                            //double eclairage = Vector3D.DotProduct(normalIntersec, directorVector) * (intensity / tLight);
                            Vector3D intersecOnLight = pointIntersec + (directorVector * tLight);
                            double distance = Math.Pow((intersecOnLight.X - pointIntersec.X), 2f) + Math.Pow((intersecOnLight.Y - pointIntersec.Y), 2f) + Math.Pow((intersecOnLight.Z - pointIntersec.Z), 2f);
                            
                            double eclairage = Vector3D.DotProduct(normalIntersec, directorVector) * (intensity / distance);
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
                        colorPixelsFinal(ref obj, ref index, ref eclairageTotal, ref blueReflected, ref greenReflected, ref redReflected);
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
            objectList = new List<Object>();
            AddObjects();
            Vector3D cameraPos = new Vector3D(0,0,6);

            Sphere lightSphere = new Sphere(2.0, new Vector3D(0, 0, -7), 255, 255, 255);

            //Trace.WriteLine("COUCOU");

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
                    double intersectMin = double.MaxValue;

                    
                    foreach(Object obj in objectList)
                    {
                        Sphere sphereObj = obj as Sphere;
                        Plane planeObj = obj as Plane;
                        Triangle triangleObj = obj as Triangle;
                        double t = new double();
                        switch (obj.Type)
                        {
                            case "Plane":
                                t = planeObj.getIntersectT(ray);
                                break;
                            case "Sphere":
                                t = ray.getSphereIntersectT(sphereObj.Center, sphereObj.Radius); 
                                break;
                            case "Triangle":
                                t = ray.getIntersectTriangleT(triangleObj);
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
                                    normalIntersec = pointIntersec - sphereObj.Center;
                                    break;
                                case "Triangle":
                                    normalIntersec = triangleObj.Normal;
                                    break;
                            }
                            
                            normalIntersec.Normalize();

                            

                            //Initialize Recursion LVL for mirrors and transparent objects
                            int recursionLVL = 1;
                            int recursionMax = 4;

                            /*
                             REFRACTION / REFLECTION
                            */

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

                            double x = (lightSphere.Center - pointIntersec).X * 0.0001;
                            double y = (lightSphere.Center - pointIntersec).Y * 0.0001;
                            double z = (lightSphere.Center - pointIntersec).Z * 0.0001;
                            Vector3D intersecOffseted = pointIntersec + new Vector3D(x, y, z);

                            double eclairageTotal = new double();

                            

                            /*
                            if (obj.Texture == "Mirroir")
                            {
                                ReflectionLoop(normalIntersec, dir, cameraPos, pointIntersec, ref eclairageTotal, ref index, ref lightSphere, ref intersecOffseted, ref blueReflected, ref greenReflected, ref redReflected, ref recursionLVL, ref recursionMax);
                                //RefractionIn(obj, normalIntersec, dir, pointIntersec, cameraPos, ref lightSphere, ref eclairageTotal, ref index, ref blueReflected, ref greenReflected, ref redReflected); //offseted ??

                            }
                            */

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

                            computeLightAndShadowsFinal(ref eclairageTotal, ref index, obj, ref lightSphere, pointIntersec, normalIntersec, intersecOffseted, ref blueReflected, ref greenReflected, ref redReflected);
                            
                        }
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
