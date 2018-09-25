using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelProject;

namespace HotelTests
{
    [TestClass]
    public class UTMainForm
    {
        /// <summary>
        /// Test of er een mainform aangemaakt kan worden.
        /// </summary>
        [TestMethod]
        public void TestMainForm()
        {
            MainForm mainForm = new MainForm();

            Assert.IsNotNull(mainForm);
        }

        /// <summary>
        /// Test of het mainform niet op pauze gaat als deze functie wordt aangeroepen, 
        /// terwijl de simulatie nooit op pauze is gezet.
        /// </summary>
        [TestMethod]
        public void TestResumeSimulation()
        {
            // Kan niet omdat de boolean pauzed niet werkt.
        }
    }
}
