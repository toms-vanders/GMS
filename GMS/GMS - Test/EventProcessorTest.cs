using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GMS___Model;
using GMS___Data_Access_Layer;
using GMS___Business_Layer;
using NodaTime;

namespace GMS___Test
{
    /// <summary>
    /// Summary description for EventProcessorTest
    /// </summary>
    [TestClass]
    public class EventProcessorTest
    {
        EventProcessor ep;
        [TestInitialize]
        public void TestInitialize()
        {
            ep = new EventProcessor();
        }

        [TestMethod]
        public void CreateValidEvent()
        {
            //EventProcessor ep = new EventProcessor();
            // Arrange
            Assert.IsTrue(ep.InsertEvent("Super Raid", "Raid", "266,070", new LocalDate(2020, 12, 10),
                "Doing a full raid. Be available for at least 2 hours", 25, "116E0C0E-0035-44A9-BB22-4AE3E23127E5"));

            // Act

            // Assert
        }
    }
}
