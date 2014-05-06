using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApplication1.Model;

namespace ConsoleApplication1
{
    class Program
    {
            private static ObservableCollection<Road> theMap = initiateMap();
            //Start point and goal point
            private static Road start_point;
            private static Road goal;
            //interesting roads that can be part of the rute
            private static ObservableCollection<Road> openlist = new ObservableCollection<Road>();
            private static ObservableCollection<Road> closedList = new ObservableCollection<Road>();
       
        static void Main(string[] args)
        {
            start_point = new Road("start",new Point(35, 80), new Point(35, 80));
            goal = new Road("goal",new Point(45,70),new Point(45,70));
            //Road currentRoad = null;
            openlist.Add(start_point);

            while (true) 
            {
                Road currentRoad = openlist.First();
                openlist.Remove(currentRoad);
                closedList.Remove(currentRoad);
                foreach (Road road in theMap)
                {
                    if(currentRoad.Endpoint == road.StartingPoint)
                    {
                        foreach (Road openListRoad in openlist)
                        {
                            if (road.StartingPoint == openListRoad.StartingPoint && road.Endpoint == openListRoad.Endpoint)
                            {
                                if (openListRoad.G > road.G)
                                {
                                    openListRoad.ParentRoad = currentRoad;
                                    calculateGHF(openListRoad);
                                    //resrot the openlist
                                }
                            }
                             else
                             {
                                road.ParentRoad = currentRoad;
                                Road tempRoad = calculateGHF(currentRoad);
                                road.G = tempRoad.G;
                                road.H = tempRoad.H;
                                road.F = tempRoad.F;
                                openlist.Add(road);
                                //resort list
                        }
                        }
                        
                    }
                }
            }



            foreach (Road road in theMap)
            {
                if (start_point.Endpoint == road.StartingPoint)
                {
                    road.ParentRoad = start_point;
                }
            }
            openlist.Clear();
            closedList.Add(start_point);

            //The A* algorithm
            while (currentRoad.StreetName != "goal")
            {
                //sort openList by f cost
                currentRoad = openlist.First();
                openlist.Remove(currentRoad);
                closedList.Add(currentRoad);
                exploreMap(currentRoad);
                openlist.Remove(currentRoad);


            }


        }

        //Calculates the F-cost
        private static double fCost(Road currentRoad)
        {
                return currentRoad.F = currentRoad.G + heuristic(currentRoad, goal);
        }
        
        //explore new roads
        private static void exploreMap(Road currentRoad)
        {
            foreach (var road in theMap)
            {
                if (road.StartingPoint == currentRoad.Endpoint)
                {
                    Road tempRoad = new Road(road.StreetName,road.StartingPoint,road.Endpoint);
                    tempRoad.ParentRoad = currentRoad;
                    calculateGHF(currentRoad, tempRoad);
                    addRoadToOpenlist(tempRoad, currentRoad);
                }
            }
        }

        private static Road calculateGHF(Road currentRoad)
        {
            Road tempRoad = new Road();
            tempRoad.H = heuristic(currentRoad, goal);
            tempRoad.G = currentRoad.ParentRoad.G + roadcost(currentRoad);
            tempRoad.F = fCost(tempRoad);
            return tempRoad;
        }

        private static void addRoadToOpenlist(Road tempRoad, Road currentRoad)
        {
            foreach (var road in openlist)
            {
                if (tempRoad == road)
                {
                    if (tempRoad.G > road.G)
                    {

                    }
                    else
                    {
                        tempRoad.ParentRoad = currentRoad;
                        calculateGHF(currentRoad,tempRoad);
                        openlist.Add(tempRoad);
                        openlist.Remove(road);
                    }
                }
            }
        }

        //straight line heuristic (start to start point)
        private static double heuristic (Road currentRoad, Road goal)
        {
            double straightLineDist = Math.Sqrt(Math.Pow(goal.Endpoint.X - currentRoad.StartingPoint.X, 2)+Math.Pow(goal.Endpoint.Y - currentRoad.StartingPoint.Y,2));
            double currentRoadCost = roadcost(currentRoad);
            return currentRoadCost + straightLineDist;

        }

        //Calculates the distance of a road
        private static double roadcost(Road currentRoad)
        {
            double cost = Math.Sqrt(Math.Pow(currentRoad.Endpoint.X - currentRoad.StartingPoint.X, 2) + Math.Pow(currentRoad.Endpoint.Y - currentRoad.StartingPoint.Y, 2));
            return cost;
        }

        private static ObservableCollection<Road> initiateMap()
        {
            //Setting up the map
            var themap = new ObservableCollection<Road>();
            themap.Add(new Road("Vestervoldgade", new Point(10, 70), new Point(20, 50)));
            themap.Add(new Road("Vestervoldgade", new Point(20, 50), new Point(10, 70)));
            themap.Add(new Road("Vestervoldgade", new Point(20, 50), new Point(35, 35)));
            themap.Add(new Road("Vestervoldgade", new Point(35, 35), new Point(20, 50)));
            themap.Add(new Road("Studie stræde", new Point(20, 50), new Point(45, 70)));
            themap.Add(new Road("Studie stræde", new Point(45, 70), new Point(70, 85)));
            themap.Add(new Road("Skt. Peders Stræde", new Point(10, 70), new Point(35, 80)));
            themap.Add(new Road("Skt. Peders Stræde", new Point(35, 80), new Point(50, 90)));
            themap.Add(new Road("Skt. Peders Stræde", new Point(65, 100), new Point(50, 90)));
            themap.Add(new Road("Vestergade", new Point(55, 55), new Point(35, 35)));
            themap.Add(new Road("Vestergade", new Point(80, 70), new Point(55, 55)));
            themap.Add(new Road("Noerregade", new Point(60, 150), new Point(65, 110)));
            themap.Add(new Road("Noerregade", new Point(65, 110), new Point(65, 100)));
            themap.Add(new Road("Noerregade", new Point(65, 100), new Point(70, 85)));
            themap.Add(new Road("Noerregade", new Point(70, 85), new Point(80, 70)));
            themap.Add(new Road("Larsbjoernsstraede", new Point(45, 70), new Point(55, 55)));
            themap.Add(new Road("Larsbjoernsstraede", new Point(45, 70), new Point(35, 80)));
            themap.Add(new Road("Teglgaardsstraede", new Point(25, 100), new Point(35, 80)));
            themap.Add(new Road("Larslejstraede", new Point(50, 90), new Point(35, 120)));
            themap.Add(new Road("Noerre voldgade", new Point(10, 70), new Point(25, 100)));
            themap.Add(new Road("Noerre voldgade", new Point(25, 100), new Point(10, 70)));
            themap.Add(new Road("Noerre voldgade", new Point(25, 100), new Point(35, 120)));
            themap.Add(new Road("Noerre voldgade", new Point(35, 120), new Point(25, 100)));
            themap.Add(new Road("Noerre voldgade", new Point(35, 120), new Point(60, 150)));
            themap.Add(new Road("Noerre voldgade", new Point(60, 150), new Point(35, 120)));

            return themap;
        }
    }
}
