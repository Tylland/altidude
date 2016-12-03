using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Altidude.Infrastructure;

namespace Infrastructure.Test
{
    [TestClass]
    public class UserLevelServiceTest
    {
        [TestMethod]
        public void UserLevelBoundaries()
        {
            var levelService = new InMemoryUserLevelService();

            Assert.AreEqual(1, levelService.CalcLevel(0).Level);
            Assert.AreEqual(1, levelService.CalcLevel(100).Level);
            Assert.AreEqual(2, levelService.CalcLevel(101).Level);
            Assert.AreEqual(2, levelService.CalcLevel(300).Level);
            Assert.AreEqual(3, levelService.CalcLevel(301).Level);
            Assert.AreEqual(3, levelService.CalcLevel(600).Level);
            Assert.AreEqual(4, levelService.CalcLevel(601).Level);
            Assert.AreEqual(4, levelService.CalcLevel(1100).Level);
        }
    }
}
