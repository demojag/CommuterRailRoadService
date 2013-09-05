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

        public String GetPathWithMaximumStops(String source, Int32 stops)
        {
            var stopsLeft = stops - 1;
            string result = string.Empty;
            var node = Nodes.Single(n => n.Source == source);

            foreach (var conn in node.Connections)
            {
                if (stopsLeft == 0) break;
                result = node.Source + "-" + GetPathWithMaximumStops(conn.Destination.Source, stopsLeft);
            }

            return result;
        }
    }
}
