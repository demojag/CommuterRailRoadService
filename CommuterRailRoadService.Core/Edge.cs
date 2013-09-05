using System;

namespace CommuterRailRoadService.Core
{
    public class Edge
    {
        private readonly Node _destination;
        private readonly Int32 _weight;

        public Edge(Node dest, Int32 weight)
        {
            _weight = weight;
            _destination = dest;
        }

        public Node Destination
        {
            get { return _destination; }
        }

        public Int32 Weight
        {
            get { return _weight; }
        }
    }
}
