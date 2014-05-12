using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using ConsoleApplication1.Model;

namespace ConsoleApplication1
{
    internal class Program
    {
        private static List<Road> theMap = initiateMap();

        private static List<Road> initiateMap()
        {
                //Setting up the map
                var themap = new List<Road>();
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

        //Start point and goal point
        private static Road _startPoint;
        private static Road _goal;
        //interesting roads that can be part of the rute
        private static List<Road> openlist = new List<Road>();
        private static List<Road> closedList = new List<Road>();

        private static void Main(string[] args)
        {
            _startPoint = new Road("start", new Point(35, 80), new Point(35, 80));
            _goal = new Road("goal", new Point(45, 70), new Point(45, 70));
            Road currentRoad = null;
            openlist.Add(_startPoint);
            theMap.Add(_goal);
            bool pathNotFound = true;

            while (true)
            {
                while (pathNotFound)
                {
                    //Pick the first element gives us the road with the smallest F cost
                    //sorts the openlist before next itteration
                    openlist = openlist.OrderBy(r => r.F).ToList();
                    currentRoad = openlist.First();
                    openlist.Remove(currentRoad);
                    closedList.Add(currentRoad);
                    List<Road> tempOpenList = new List<Road>(openlist);

                    //Each road that we can go ot from the current square are added to the walkAbleRoad list.
                    List<Road> walkAbleRoads = new List<Road>();
                    foreach (Road road in theMap)
                    {
                        if (road.StartingPoint == currentRoad.Endpoint)
                        {
                            walkAbleRoads.Add(road);
                        }
                    }
                    //Check if some of the walkable roads are on the openlist otherwise add them and record F,G and H cost
                    foreach (Road walkAbleRoad in walkAbleRoads)
                    {
                        if (closedList.Contains(walkAbleRoad))
                        {
                        }
                        else
                        {
                            if (tempOpenList.Count != 0)
                            {
                                if (checkWithOpenlist(walkAbleRoad) == null)
                                {
                                    AddToOpenlist(walkAbleRoad, currentRoad);
                                }
                                else
                                {
                                    Road openRoad = checkWithOpenlist(walkAbleRoad);
                                    if (walkAbleRoad.G < openRoad.G)
                                    {
                                        openlist.Remove(openRoad);
                                        AddToOpenlist(walkAbleRoad, currentRoad);
                                    }
                                }
                            }
                            else
                            {
                                AddToOpenlist(walkAbleRoad, currentRoad);
                            }
                        }
                    }
                    //error in checking if goal is in closed list
                    if (closedListContains(_goal) != null)
                    {
                        Road road = closedListContains(_goal);
                        Console.WriteLine(road);
                        while (road.ParentRoad != null)
                        {
                            Console.WriteLine(road.ParentRoad);
                            road = road.ParentRoad;
                        }
                        pathNotFound = false;
                    }

                    if (openlist.Count == 0)
                    {
                        Console.Write("Path not found");
                        pathNotFound = false;
                    }

                }  
            }
        }

        private static Road closedListContains(Road goal)
        {
            foreach (Road road in closedList)
            {
                if (road.Endpoint == goal.Endpoint && road.StartingPoint == goal.StartingPoint)
                {
                    return road;
                }
            }
            return null;
        }

        private static Road checkWithOpenlist(Road road)
        {
            foreach (Road openRoad in openlist)
            {
                if (openRoad.Endpoint == road.Endpoint && openRoad.StartingPoint == road.StartingPoint)
                {
                    return openRoad;
                }
            }
            return null;
        }

        // Method to add a walkable road to the open list
        private static void AddToOpenlist(Road walkAbleRoad, Road currentRoad)
        {
            Road tempWalkAbleRoad = new Road();
            tempWalkAbleRoad.Endpoint = walkAbleRoad.Endpoint;
            tempWalkAbleRoad.StartingPoint = walkAbleRoad.StartingPoint;
            tempWalkAbleRoad.StreetName = walkAbleRoad.StreetName;
            tempWalkAbleRoad.ParentRoad = currentRoad;
            tempWalkAbleRoad.G = tempWalkAbleRoad.ParentRoad.G +
                                 StraightLineHeuristic(tempWalkAbleRoad.StartingPoint, tempWalkAbleRoad.Endpoint);
            tempWalkAbleRoad.H = StraightLineHeuristic(tempWalkAbleRoad, _goal);
            tempWalkAbleRoad.F = tempWalkAbleRoad.G + tempWalkAbleRoad.H;
            openlist.Add(tempWalkAbleRoad);
        }

        // StraightLineDistance end to end
        private static double StraightLineHeuristic(Road road, Road goal)
        {
            return Math.Sqrt(Math.Pow(goal.Endpoint.X - road.Endpoint.X, 2) + Math.Pow(goal.Endpoint.Y - road.Endpoint.Y, 2));
        }

        //Point to Point straight ligne distance
        private static double StraightLineHeuristic(Point startPoint, Point endPoint)
        {
            return Math.Sqrt(Math.Pow(endPoint.X - startPoint.X, 2) + Math.Pow(endPoint.Y - startPoint.Y, 2));
        }
    }
}
