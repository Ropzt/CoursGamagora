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
    public class Object
    {
        public string Type;
        public int colorR;
        public int colorG;
        public int colorB;
        public string Texture;
    }

    public class Boite : Object
    {
        public Boite()
        {
            Type = "Boite";
        }
    }

    public class Triangle : Object
    {
        public Vector3D v0;
        public Vector3D v1; 
        public Vector3D v2;
        public Vector3D Normal;

        public Triangle(Vector3D p0, Vector3D p1, Vector3D p2, int red, int green, int blue, string texture = "Opaque")
        {
            v0 = p0;
            v1 = p1;
            v2 = p2;
            
            Vector3D v0v1 = v1 - v0;
            Vector3D v0v2 = v2 - v0;
            Normal = Vector3D.CrossProduct(v0v2, v0v1);
            //Normal = triangleNormal;

            Type = "Triangle";
            colorR = red;
            colorG = green;
            colorB = blue;
            Texture = texture;
        }
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
            double determinant = Vector3D.DotProduct(ray.Direction, Normal);
            if (determinant < 1e-7)
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
}
