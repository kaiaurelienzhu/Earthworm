using System;
using System.Collections.Generic;
using System.Linq;
using Earthworm;
using GMap.NET;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Earthworm_unitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //Arrange
            var min = new PointLatLng(1, 2);
            var max = new PointLatLng(3, 4);
            var points = new List<PointLatLng>();
            points.Add(min);
            points.Add(new PointLatLng(max.Lat, min.Lng));
            points.Add(max);
            points.Add(new PointLatLng(min.Lat, max.Lng));
            //Act
            var createRectangle = helpers_UI.CreateRectangle(min, max);
            //Assert
            Assert.AreEqual(points.First().Lat, createRectangle.First().Lat);
            Assert.AreEqual(points.First().Lng, createRectangle.First().Lng);
            Assert.IsTrue(createRectangle.Count == points.Count);
            Assert.AreEqual(points.ElementAt(1).Lat, createRectangle.ElementAt(1).Lat);
            Assert.AreEqual(points.ElementAt(1).Lng, createRectangle.ElementAt(1).Lng);
            Assert.AreEqual(points.ElementAt(2).Lat, createRectangle.ElementAt(2).Lat);
            Assert.AreEqual(points.ElementAt(2).Lng, createRectangle.ElementAt(2).Lng);
            Assert.AreEqual(points.ElementAt(3).Lat, createRectangle.ElementAt(3).Lat);
            Assert.AreEqual(points.ElementAt(3).Lng, createRectangle.ElementAt(3).Lng);
        }
    }
}
