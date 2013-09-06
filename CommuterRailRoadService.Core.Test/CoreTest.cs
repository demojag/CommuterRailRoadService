using System;
using NUnit.Framework;

namespace CommuterRailRoadService.Core.Test
{
    [TestFixture]
    public class CoreTest
    {
        private Graph _map;

        [SetUp]
        public void SetUp()
        {
            _map = new Graph("AB5,BC4,CD8,DC8,DE6,AD5,CE2,EB3,AE7");

        }

        [Test]
        [TestCase("A-B-C", "9")]
        [TestCase("A-D", "5")]
        [TestCase("A-D-C", "13")]
        [TestCase("A-E-B-C-D", "22")]
        [TestCase("A-E-D", "NO SUCH ROUTE")]
        public void FindDistancesBetweenNodes(String path, String expected)
        {
            var result = _map.GetPath(path);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void FindPathWithMaximumStops()
        {
            var result = _map.GetPathWithMaximumStops("C","C", 3, false);
            Assert.AreEqual(2, result);
        }

        [Test]
        public void FindPathWithExactStops()
        {
            var result = _map.GetPathWithMaximumStops("A", "C", 4, true);
            Assert.AreEqual(3, result);
        }

        [Test]
        [TestCase("A","C", 9)]
        [TestCase("B","B", 9)]
        public void FindShortestPath(String source, String dest, Int32 expected)
        {
            var result = _map.GetShortestPath(source,dest);
            Assert.AreEqual(expected, result);
        }

    }

}
