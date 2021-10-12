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

        public bool isRayHittingInsideTriangle(Vector3D pointIntersec, Triangle triangle)
        {
            Vector3D C; // vector perpendicular to triangle's plane 

            // edge 0
            Vector3D edge0 = triangle.v1 - triangle.v0;
            Vector3D v0Intersec = pointIntersec - triangle.v0;
            C = Vector3D.CrossProduct(v0Intersec, edge0);
            if( Vector3D.DotProduct(triangle.Normal, C) < 0 )
            {
                return false;
            }
            

            // edge 1
            Vector3D edge1 = triangle.v2 - triangle.v1;
            Vector3D v1Intersec = pointIntersec - triangle.v1;
            C = Vector3D.CrossProduct(v1Intersec, edge1);
            if (Vector3D.DotProduct(triangle.Normal, C) < 0)
            {
                return false;
            }
            
            // edge 2
            Vector3D edge2 = triangle.v0 - triangle.v2;
            Vector3D v2Intersec = pointIntersec - triangle.v2;
            C = Vector3D.CrossProduct(  v2Intersec, edge2);
            if (Vector3D.DotProduct(triangle.Normal, C) < 0)
            {
                return false;
            }
            return true;

        }
        
        public double getIntersectTriangleT(Triangle triangle)
        {
            double determinant = Vector3D.DotProduct(Direction, triangle.Normal);
            if (determinant < 1e-7)
            {
                double t = (Vector3D.DotProduct(triangle.Normal, triangle.v0 - Origin )  / determinant);
                //double t = ((Vector3D.DotProduct(triangle.Normal, Origin) + Vector3D.DotProduct(triangle.Normal, triangle.v0)) /determinant); // Probleme avec cette formule, t n'est pas assez grand et donc mon pointIntersec se retrouve en -4 au lieu de -16
                //double t = (Vector3D.DotProduct((Center - Origin), triangle.Normal) / determinant);
                
                Vector3D pointIntersec = Origin + (Direction * t);
                

                if (isRayHittingInsideTriangle(pointIntersec, triangle))
                {
                    return t;
                }
                else
                {
                    return 0;
                }
            }        
            else
            {
                return 0;
            }
        }

    }

}
