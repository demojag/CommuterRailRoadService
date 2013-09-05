using System;
using System.Collections.Generic;

namespace CommuterRailRoadService.Core
{
    public class Node
    {
        private readonly String _station;
        private readonly List<Edge> _connections;

        public Node(String source, List<Edge> conn)
        {

            _station = source;
            _connections = conn;
        }

        public List<Edge> Connections
        {
            get { return _connections; }
        }

        public string Source
        {
            get { return _station; }
        }
    }
}
