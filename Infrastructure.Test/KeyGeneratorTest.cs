using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altidude.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Infrastructure.Test
{
    [TestClass]
    public class KeyGeneratorTest
    {
        [TestMethod]
        public void GenerateUniqueKey()
        {
            Console.WriteLine(KeyGenerator.GetUniqueKey());
        }
    }
}
