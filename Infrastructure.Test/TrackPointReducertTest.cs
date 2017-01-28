using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Altidude.Contracts.Types;
using System.Collections.Generic;
using Altidude.Infrastructure;
using KellermanSoftware.CompareNetObjects;

namespace Infrastructure.Test
{
    [TestClass]
    public class TrackPointReducertTest
    {
        [TestMethod]
        public void ReduceTrackPointsToGivenNumber()
        {
            var time = DateTime.Now;

            var points = new List<TrackPoint> { new TrackPoint(0, 0, 0, 0, time), new TrackPoint(0, 0, 12, 10, time), new TrackPoint(0, 0, 20, 20, time), new TrackPoint(0, 0, 12, 30, time) };
            var expectedPoints = new List<TrackPoint> { new TrackPoint(0, 0, 0, 0, time), new TrackPoint(0, 0, 20, 20, time), new TrackPoint(0, 0, 12, 30, time) };

            var reducer = new TrackPointNumberReducer(1);

            var recudedPoints = reducer.Process(points.ToArray());

            var compareLogic = new CompareLogic(new ComparisonConfig { MaxDifferences = 10 });

            var compareResult = compareLogic.Compare(expectedPoints.ToArray(), recudedPoints);

            Assert.IsTrue(compareResult.AreEqual, compareResult.DifferencesString);
        }
    }
}