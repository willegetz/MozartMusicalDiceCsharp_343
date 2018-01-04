using ApprovalTests.Asp;
using ApprovalTests.Asp.Mvc;
using ApprovalTests.Reporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicalDiceTests.MusicalPlayerTests.TestableControllers;
using MusicalDiceTests.MusicalPlayerTests.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalDiceTests.MusicalPlayerTests.Views
{
    // TODO: Start Without Debugging to launch IISExpress and be able to use the MvcApprovals.

    [TestClass]
    [UseReporter(typeof(BeyondCompare4Reporter))]
    public class HomeViewsTests
    {
        [TestMethod]
        public void IndexView()
        {
            PortFactory.MvcPort = 49300;
            MvcApprovals.VerifyMvcPage<TestableHomeController>(home => home.TestIndex, CustomMvcScrubbers.CopyrightScrubber);
        }
    }
}
