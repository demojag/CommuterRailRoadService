using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace CommuterRailRoadService.Core
{
    public class Graph
    {
        public readonly ICollection<Node> Nodes;

        public Graph(string mapDefinition)
        {
            Nodes = new Collection<Node>();
            InitNodes(mapDefinition);
            CreateNodesConnections(mapDefinition);
        }

        private void CreateNodesConnections(string mapDefinition)
        {
            var items = mapDefinition.Split(',');
            foreach (var node in items)
            {
                var tmp = Nodes.Single(x => x.Source == node[0].ToString());
                tmp.Connections.Add(new Edge(Nodes.Single(x => x.Source == node[1].ToString()), int.Parse(node[2].ToString())));
            }
        }

        private void InitNodes(String mapDefinition)
        {
            var items = mapDefinition.Trim().Split(',');
            foreach (var item in items)
            {
                if (!Nodes.Any(x => x.Source == item[0].ToString()))
                {
                    Nodes.Add(new Node(item[0].ToString(CultureInfo.InvariantCulture), new List<Edge>()));
                }
            }
        }

        public String GetPath(String path)
        {
            var pathToFind = path.Trim().Split('-');
            var total = 0;
            try
            {
                for (var i = 0; i < pathToFind.Length; i++)
                {
                    if (pathToFind.Length != (i + 1))
                        total += Nodes.Single(x => x.Source == pathToFind[i])
                             .Connections.Single(y => y.Destination.Source == pathToFind[i + 1])
                             .Weight;
                }
            }
            catch (InvalidOperationException)
            {
                //TODO find better solution
                return "NO SUCH ROUTE";
            }

            return total.ToString(CultureInfo.InvariantCulture);
        }

        public Int32 GetPathWithMaximumStops(String source, String destination, Int32 stops, Boolean exactStops)
        {

            int pathFound = 0;


            pathFound = InternalPathFinder(source, destination, stops, ref pathFound, exactStops);

            return pathFound;
        }

        public Int32 GetShortestPath(String source, String destination)
        {

            int lenght = 0;
            List<Int32> pathFounds = new List<int>();
            InternalPathFinderCount(source, String.Empty, destination, ref lenght, ref pathFounds);

            return pathFounds.Min();
        }

        private void InternalPathFinderCount(string source, string prev, string destination, ref int lenght, ref List<int> minimumFound)
        {
            Node node = Nodes.Single(n => n.Source == source);

            foreach (var conn in node.Connections)
            {

                if (conn.Destination.Source != prev)
                {
                    lenght += conn.Weight;
                    if (conn.Destination.Source == destination)
                    {
                        minimumFound.Add(lenght);
                        lenght -= conn.Weight;
                        break;
                    }
                    InternalPathFinderCount(conn.Destination.Source, node.Source, destination, ref lenght, ref minimumFound);
                    lenght -= conn.Weight;
                }
            }

        }

        private int InternalPathFinder(string source, string destination, int stops, ref int pathFound, bool exactStops)
        {
            Node node = Nodes.Single(n => n.Source == source);

            if (stops != 0)
            {
                stops--;
                foreach (var conn in node.Connections)
                {
                    if (exactStops) pathFound += CheckExactStopsDestination(destination, stops, conn);
                    else pathFound += CheckStopsDestination(destination, conn);
                    InternalPathFinder(conn.Destination.Source, destination, stops, ref pathFound, exactStops);
                }
            }
            return pathFound;
        }

        private static Int32 CheckStopsDestination(string destination, Edge conn)
        {
            return conn.Destination.Source == destination ? 1 : 0;
        }

        private static Int32 CheckExactStopsDestination(string destination, int stops, Edge conn)
        {
            if (conn.Destination.Source == destination && stops == 0)
            {
                return 1;
            }
            return 0;
        }

    }
}
