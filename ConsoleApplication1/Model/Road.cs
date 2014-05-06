using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleApplication1.Model
{
    class Road
    {
        private string street_name;
        private double f;
        private double g;
        private double h;
        private Road parentRoad = null;
        private Point startingPoint;
        private Point endpoint;

        public Road(String street_name, Point startingPoint, Point endPoint)
        {
            this.StreetName = street_name;
            this.StartingPoint = startingPoint;
            this.Endpoint = endPoint;
        }


        public string StreetName
        {
            get { return street_name; }
            set { street_name = value; }
        }

        public double F
        {
            get { return f; }
            set { f = value; }
        }

        public double G
        {
            get { return g; }
            set { g = value; }
        }

        public double H
        {
            get { return h; }
            set { h = value; }
        }

        public Point StartingPoint
        {
            get { return startingPoint; }
            set { startingPoint = value; }
        }

        public Point Endpoint
        {
            get { return endpoint; }
            set { endpoint = value; }
        }

        public Road ParentRoad
        {
            get { return parentRoad; }
            set { parentRoad = value; }
        }
    }
}
