using ApprovalTests.Asp.Mvc;
using MusicalDicePlayer.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MusicalDiceTests.MusicalPlayerTests.TestableControllers
{
    public class TestableHomeController : TestableController<HomeController>
    {
        public TestableHomeController(HomeController homeController) : base(homeController) { }

        public ActionResult TestIndex()
        {
            return ControllerUnderTest.Index();
        }
    }
}
